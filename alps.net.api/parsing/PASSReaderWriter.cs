using alps.net.api.parsing.graph;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Ontology;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Reflection;

namespace alps.net.api.parsing
{
    /// <summary>
    /// The main parser class (Singleton).
    /// 
    /// To load a model contained in an owl/rdf formatted file, either use
    /// <br></br>
    /// - loadOWLParsingStructure() and pass path references to used ontology classes (i.e. the standard-pass-ont.owl) and afterwards
    /// loadModels(paths) with path references to the owl files containing the models or
    /// <br></br>
    /// - loadModels(paths, true) with path references to the owl files containing the models.
    /// <br></br>
    /// Loading the parsing structure is expensive, so it is advised to do it seperately and not reload it if not neccessary.
    /// </summary>
    public class PASSReaderWriter : IPASSReaderWriter
    {
        /// <summary>
        /// The element factory gets an uri that should be parsed and a list of possible instances of classes this uri can be instanciated with.
        /// The list of possible instances is stored inside the parsingDict.
        /// the element factory than decides which instance to use.
        /// </summary>
        private IPASSProcessModelElementFactory<IParseablePASSProcessModelElement> elementFactory = new BasicPASSProcessModelElementFactory();

        /// <summary>
        /// The matcher creates a mapping between owl classes (found in an owl file) and c# classes (found in the current assembly)
        /// </summary>
        private readonly IParsingTreeMatcher matcher = new ParsingTreeMatcher();

        /// <summary>
        /// A parsing dictionary that contains the mapping between ontology classes and a list of possible c# classes
        /// </summary>
        private IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict
            = new Dictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>>();

        /// <summary>
        /// Holds the current instance of the PASSReaderWriter class.
        /// The class is only instanciated once.
        /// </summary>
        private static PASSReaderWriter readerWriter;

        /// <summary>
        /// Get the current instance of the PASSReaderWriter class.
        /// The class structure must only be loaded once, afterwards each class can instantly load models
        /// without overwriting the parsing structure when fetching this instance.
        /// </summary>
        /// <returns>The instance of this PASSReaderWriter class</returns>
        public static PASSReaderWriter getInstance()
        {
            if (readerWriter is null)
            {
                readerWriter = new PASSReaderWriter();
            }
            return readerWriter;
        }

        /// <summary>
        /// The private constructor that initializes logs
        /// </summary>
        private PASSReaderWriter()
        {
            string logName = "logfile.txt";
            string path = Directory.GetCurrentDirectory();
            string cutPath = path + "logs\\";

            Directory.CreateDirectory(cutPath);

            if (File.Exists(cutPath + logName))
            {
                File.Delete(cutPath + logName);
            }
            Console.WriteLine("Writing logs to " + cutPath + logName);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                //.WriteTo.Console()
                .WriteTo.File(cutPath + logName)
                .CreateLogger();
        }


        public void addAssemblyToCheckForTypes(Assembly assembly)
        {
            ReflectiveEnumerator.addAssemblyToCheckForTypes(assembly);
        }

        public IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> getParsingDict()
        {
            var newParsingDict = new Dictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>>();
            foreach (var pair in parsingDict)
            {
                newParsingDict.Add(pair.Key, new List<(ITreeNode<IParseablePASSProcessModelElement>, int)>(pair.Value));
            }
            return newParsingDict;
        }

        public void loadOWLParsingStructure(IList<string> filepathsToOWLFiles)
        {
            IList<OntologyGraph> owlStructureGraphs = new List<OntologyGraph>();
            Log.Information("Beginnig to create parsing structure matching...");
            foreach (string filepath in filepathsToOWLFiles)
            {
                try
                {
                    // Create a new OntologyGraph
                    OntologyGraph owlGraph = new OntologyGraph();
                    // Load files into it
                    owlGraph.LoadFromFile(filepath);
                    owlStructureGraphs.Add(owlGraph);
                }
                catch (RdfParseException parseException)
                {
                    Log.Error("Parser Error when reading the new File " + parseException);
                    Console.WriteLine();
                    // TODO error loggen
                }
            }
            Log.Information("Read all structure defining ontology graphs.");
            parsingDict = matcher.loadOWLParsingStructure(owlStructureGraphs);

        }

        public IList<IPASSProcessModel> loadModels(IList<string> filepaths, IGraphFactory modelGraphFactory = null, bool overrideOWLParsingStructure = false)
        {
            Console.Write("Reading input owl files...");

            //IList<IPASSProcessModel> passProcessModels = new List<IPASSProcessModel>();
            IList<OntologyGraph> loadedModelGraphs = new List<OntologyGraph>();
            IList<OntologyGraph> owlStructureGraphs = new List<OntologyGraph>();

            foreach (string filepath in filepaths)
            {
                try
                {

                    // Create a new OntologyGraph
                    OntologyGraph owlGraph = new();
                    // Load files into it
                    owlGraph.LoadFromFile(filepath);
                    owlStructureGraphs.Add(owlGraph);


                    if (!isStandardPass(owlGraph.BaseUri.ToString())) { loadedModelGraphs.Add(owlGraph); }
                    Log.Information("Done reading the new File: " + filepath);
                }
                catch (System.UriFormatException uriException)
                {
                    Log.Information("Tried to pre-clean the file due to a bad XMLNS: " + filepath);
                    //try this again if there is a problem with the input file but try some known fixes before
                    try
                    {
                        preCleanFile(filepath);
                        // Create a new OntologyGraph
                        OntologyGraph owlGraph = new();
                        // Load files into it
                        owlGraph.LoadFromFile(filepath);
                        owlStructureGraphs.Add(owlGraph);
                        if (!isStandardPass(owlGraph.BaseUri.ToString())) { loadedModelGraphs.Add(owlGraph); }
                        Log.Information("Done reading the new File : " + filepath);
                    }
                    catch (RdfParseException parseException)
                    {
                        Log.Error("Parser Error when reading the new File " + parseException);
                        Console.WriteLine("Error reading file.");
                        return null;
                    }
                    catch (IOException parseException)
                    {
                        Log.Error("Parser Error when reading the new File " + parseException);
                        Console.WriteLine("Error reading file.");
                        return null;
                    }
                }
                catch (RdfParseException parseException)
                {
                    Log.Error("Parser Error when reading the new File " + parseException);
                    Console.WriteLine("Error reading file.");
                    return null;
                }
                catch (IOException parseException)
                {
                    Log.Error("Parser Error when reading the new File " + parseException);
                    Console.WriteLine("Error reading file.");
                    return null;
                }
            }

            Console.WriteLine("Done.");
            if (overrideOWLParsingStructure)
                parsingDict = matcher.loadOWLParsingStructure(owlStructureGraphs);

            // Stores all found models across all graphs
            IList<IPASSProcessModel> passProcessModels = new List<IPASSProcessModel>();

            Console.Write("Instantiating in memory models...");
            ConsoleProgressBar bar = new ConsoleProgressBar();
            int count = 0;

            foreach (OntologyGraph graph in loadedModelGraphs)
            {
                IDictionary<string, IList<string>> namedIndividualsDict = findAllNamedIndividualTriples(graph);
                count++;
                bar.Report((double)count / filepaths.Count * 2);

                // Get models with elements from the current graph and merge it in the list of all models
                passProcessModels = passProcessModels.Union(createClassInstancesFromNamedIndividuals(graph, namedIndividualsDict, modelGraphFactory)).ToList();
                count++;
                bar.Report((double)count / filepaths.Count * 2);
            }
            Console.WriteLine("Done.");
            Console.WriteLine("Finished in-memory model creation");

            return passProcessModels;
        }

        /// <summary>
        /// The .NetRDF library has a few problems with input files that are basically stupid
        /// E.g. an <rdf:RDF xmlns="" at the beginning of an RDF dokument  may cause an 
        /// Invalid URI exception. Sadly some tools like Protegée generate exactly stuff like that
        /// This method simple tries to clean known problems 
        /// </summary>
        /// <param name="filepath">Path to the file to be cleanes</param>
        private void preCleanFile(string filePath)
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            string xmlnsConstruct = "xmlns=\"\"";

            for (int i = 0; i < Math.Min(lines.Length, 10); i = i + 1) // Change the number 5 to the desired number of lines to check
            {
                if (lines[i].Contains(xmlnsConstruct))
                {
                    lines[i] = lines[i].Replace(xmlnsConstruct, "");
                }
            }

            // Save the modified lines back to the file
            File.WriteAllLines(filePath, lines);
        }

        /// <summary>
        /// Verifies whether a triple (as string) is part of a standard pass definition owl or a model
        /// </summary>
        /// <param name="someString">a triple converted to string</param>
        /// <returns>true if it is part of a standard pass document, false if it is only part of a normal model</returns>
        private bool isStandardPass(Triple triple)
        {
            return isStandardPass(triple.ToString());
        }

        /// <summary>
        /// Verifies whether a triple (as string) is part of a standard pass definition owl or a model
        /// </summary>
        /// <param name="someString">a triple converted to string</param>
        /// <returns>true if it is part of a standard pass document, false if it is only part of a normal model</returns>
        private bool isStandardPass(string trpl)
        {
            return (trpl.Contains("standard") && trpl.Contains("pass")) || (trpl.Contains("abstract") && trpl.Contains("pass"));
        }

        /// <summary>
        /// Finds and creates all the named individuals in the given files and creates a new list with all the individuals
        /// </summary>
        private IDictionary<string, IList<string>> findAllNamedIndividualTriples(IGraph graph)
        {
            IEnumerable<Triple> triplesWithNamedIndividualSubject;
            IList<Triple> namedIndividualsList = new List<Triple>();
            IDictionary<string, IList<string>> namedIndividualsDict = new Dictionary<string, IList<string>>();

            // Iterate over triples in the graph
            foreach (Triple triple in graph.Triples)
            {
                // Add named individuals
                if (triple.Object.ToString().Contains("NamedIndividual") && triple.Subject.ToString().Contains("#") && !isStandardPass(triple))
                {
                    namedIndividualsList.Add(triple);
                }
            }


            foreach (Triple t in namedIndividualsList)
            {
                triplesWithNamedIndividualSubject = graph.Triples.WithSubject(t.Subject);

                foreach (Triple l in triplesWithNamedIndividualSubject)
                {
                    // Get the one triple that specifies type and does not contain NamedIndividual as object
                    if (l.Predicate.ToString().Contains("type") && !l.Object.ToString().Contains("NamedIndividual"))
                    {
                        //this.namedIndiviualsType.Add(l);
                        if (!namedIndividualsDict.ContainsKey(t.Subject.ToString()))
                        {
                            namedIndividualsDict.Add(l.Subject.ToString(), new List<string> { l.Object.ToString() });
                        }
                        else
                        {
                            namedIndividualsDict[l.Subject.ToString()].Add(l.Object.ToString());
                        }
                    }
                }
            }

            return namedIndividualsDict;

        }

        /// <summary>
        /// Method that creates the empty Instances of the Objects which later get completed by the completetObjects() method
        /// </summary>
        /// <param name="namedIndividualsDict">A dictionary containing the uri for each NamedIndividual as key and the type(s) as value (in the form of a list)</param>
        /// <param name="graph">The graph used for parsing</param>
        /// <returns></returns>
        private IList<IPASSProcessModel> createClassInstancesFromNamedIndividuals(IGraph graph, IDictionary<string, IList<string>> namedIndividualsDict, IGraphFactory modelGraphFactory)
        {
            IDictionary<string, IParseablePASSProcessModelElement> createdElements = new Dictionary<string, IParseablePASSProcessModelElement>();
            IList<IPASSProcessModel> passProcessModels = new List<IPASSProcessModel>();

            //Object:     owl:Ontology
            //Predicate:  rdf:type
            // The base uri for the current parsing graph
            string baseUri = graph.Triples.WithObject(graph.GetUriNode("owl:Ontology")).First().Subject.ToString();

            foreach (KeyValuePair<string, IList<string>> pair in namedIndividualsDict)
            {

                // Generates a new modelElement and returns the type this element is instantiated with
                // (i.e. an abstract DoState has the types "AbstractState" and "DoState", but is instantiated with a DoState, so this method returns "DoState"
                string elementType = elementFactory.createInstance(parsingDict, pair.Value, out IParseablePASSProcessModelElement modelElement);

                // If the factory found a fitting element
                if (modelElement is not null)
                {
                    // The model element receives its triples which define all its characteristics in the form of incomplete triples
                    // Incomplete triples carry no information about the subject, as the subjects uri can change during parsing.
                    IList<Triple> elementTriples = new List<Triple>(graph.GetTriplesWithSubject(graph.GetUriNode(new Uri(pair.Key))));
                    IList<IPASSTriple> elementIncompleteTriples = new List<IPASSTriple>();

                    foreach (Triple triple in elementTriples)
                    {
                        var subj = NodeHelper.getNodeContent(triple.Subject);
                        var pred = NodeHelper.getNodeContent(triple.Predicate);
                        var obj = NodeHelper.getObjAsStringWithExtra(triple.Object);
                        if (obj == null)
                        {
                            var strObj = NodeHelper.getNodeContent(triple.Object);
                            if (strObj != null) elementIncompleteTriples.Add(new PASSTriple(subj, pred, strObj));
                        }
                        else
                        {
                            elementIncompleteTriples.Add(new PASSTriple(subj, pred, obj));
                        }
                    }

                    modelElement.addTriples(elementIncompleteTriples);

                    // Important! The ModelComponentID is overwritten by the suffix of the elements uri (= "baseuri#suffix").
                    if (elementTriples.Count > 0)
                        modelElement.setModelComponentID(StaticFunctions.removeBaseUri(elementTriples[0].Subject.ToString(), baseUri));

                    if (modelElement is IPASSProcessModel passProcessModel)
                    {
                        passProcessModels.Add(passProcessModel);
                        passProcessModel.setModelGraph(modelGraphFactory);
                        passProcessModel.setBaseURI(baseUri);
                        IPASSGraph modelBaseGraph = passProcessModel.getBaseGraph();

                        // Add all the triples to the graph that describe the owl file directly (version iri, imports...)
                        foreach (Triple triple in graph.Triples.WithSubject(graph.GetUriNode(new Uri(baseUri))))
                        {
                            var subj = NodeHelper.getNodeContent(triple.Subject);
                            var pred = NodeHelper.getNodeContent(triple.Subject);
                            var obj = NodeHelper.getObjAsStringWithExtra(triple.Object);
                            if (obj == null)
                            {
                                var strObj = NodeHelper.getNodeContent(triple.Object);
                                if (strObj != null) modelBaseGraph.addTriple(new PASSTriple(subj, pred, strObj));
                            }
                            else modelBaseGraph.addTriple(new PASSTriple(subj, pred, obj));
                        }
                    }
                    else
                    {
                        // if the factory could not find a fitting element
                        // TODO set Model before adding Attributes
                        //modelElement.addAllAttributes(this, pair.Key); 
                        //modelElement.setModelComponentID(pair.Key);
                    }
                    createdElements.Add(modelElement.getModelComponentID(), modelElement);
                }
            }

            // Now all elements are instanciated and received their describing triples.
            // Triples that do not point to other elements have already been parsed (i.e. hasModelComponentID, hasComment ...)

            if (passProcessModels.Count == 0)
            {
                IPASSProcessModel passProcessModell = new PASSProcessModel(graph.BaseUri.ToString());
                passProcessModell.createUniqueModelComponentID("defaultModelID");
                passProcessModels.Add(passProcessModell);
            }

            // To parse all triples that link to other elements, the model is involved
            // The model is parsed top down; Every element that needs the reference to another can ask the model
            // by passing the suffix of the uri of the required element (which is also the ModelComponentID).
            foreach (IPASSProcessModel model in passProcessModels)
            {
                if (model is IParseablePASSProcessModelElement parseable)
                    parseable.completeObject(ref createdElements);
            }

            // ? Was tun mit elementen die in keinem Model sind? Wie prüfen ?

            return passProcessModels;
        }


        public string exportModel(IPASSProcessModel model, string filepath, out IGraph exportGraph)
        {
            // Get the graph hold by the model and use the export function given by the library
            string fullPath = (filepath.EndsWith(".owl")) ? filepath : filepath + ".owl";
            model.getBaseGraph().exportTo(fullPath);
            if (model.getBaseGraph() is VdsRdfGraph graph)
                exportGraph = graph.getGraph();
            else exportGraph = null;
            return fullPath;
        }

        public string exportModel(IPASSProcessModel model, string filepath)
        {
            return exportModel(model, filepath, out IGraph graph);
        }

        public void setModelElementFactory(IPASSProcessModelElementFactory<IParseablePASSProcessModelElement> elementFactory)
        {
            if (elementFactory is null) return;
            this.elementFactory = elementFactory;
        }
    }
}
