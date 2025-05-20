using alps.net.api.StandardPASS;
using alps.net.api.util;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF.Ontology;

namespace alps.net.api.parsing
{

    public interface IParsingTreeMatcher
    {
        /// <summary>
        /// Creates a dictionary that maps owl classes with c# classes.<br></br>
        /// The owl classes are extracted from given class-defining owl files (i.e. the standard-pass-ont, abstract-pass-ont).<br></br>
        /// The c# classes are evaluated dynamically at runtime from all classes known to this assembly.<br></br>
        /// </summary>
        /// <param name="owlStructureGraphs">A list of filepaths to class-defining owl files</param>
        /// <param name="bar">A progress bar</param>
        /// <returns></returns>
        public IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> loadOWLParsingStructure
            (IList<OntologyGraph> owlStructureGraphs);


    }
    /// <summary>
    /// This class creates trees for the owl class hierarchy and the c# class hierarchy dynamically at runtime.
    /// Afterwards, the nodes in the trees are mapped together.
    /// The mapping is used by the parser to instantiate owl class instances with c# class instances
    /// </summary>
    public class ParsingTreeMatcher : IParsingTreeMatcher
    {

        public IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> loadOWLParsingStructure
            (IList<OntologyGraph> owlStructureGraphs)
        {
            Log.Information("Merging all input graphs...");
            Console.Write("Generating class mapping for parser...");
            ConsoleProgressBar consoleBar = new();
            consoleBar.Report(0);
            OntologyGraph parsingStructureOntologyGraph = new();

            // Merge the input of all files in one big graph
            foreach (OntologyGraph owlGraph in owlStructureGraphs)
            {
                parsingStructureOntologyGraph.Merge(owlGraph);
            }

            consoleBar.Report(0.25);

            Log.Information("Creating owl inheritance tree from merged graphs...");

            // Create the inheritance tree for the loaded owl classes
            // The base classes are the classes that have only child classes, no parent classes.
            // They are possible base classes for the tree structure
            IList<OntologyClass> baseClasses = createOWLInheritanceTree(parsingStructureOntologyGraph);

            consoleBar.Report(0.5);
            Log.Information("Dynamically created owl class tree");
            Log.Information("Creating c# class inheritance tree from classes known to the assembly...");

            // Create the inheritance tree for the c# classes by recursively finding child classes to the PASSProcessModelElement class
            // Does also find child classes of external projects if they registered themself at the ReflectiveEnumerator class.
            ITreeNode<IParseablePASSProcessModelElement> treeRootNode = createClassInheritanceTree();

            consoleBar.Report(0.75);
            Log.Information("Dynamically created c# class tree");

            // Maps a list of possible c# classes to each ontology class
            IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict
                = new Dictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>>();

            // Map the each base class and its child classes with the c# classes
            foreach (OntologyClass baseClass in baseClasses)
            {
                createParsingStructureFromTrees(parsingDict, baseClass, treeRootNode);
            }

            Log.Information("Finished class mapping");

            consoleBar.Report(1);
            Console.Write("Done.");

            return parsingDict;
        }


        // ################################ OWL class tree creation ################################


        /// <summary>
        /// Creates the inheritance tree for owl classes out of the given files
        /// </summary>
        private IList<OntologyClass> createOWLInheritanceTree(OntologyGraph parsingStructureOntologyGraph)
        {
            IList<OntologyClass> nodesWithoutParents = new List<OntologyClass>();

            // Find a root for the tree by finding alls classes that have no parent
            foreach (OntologyClass ontClass in parsingStructureOntologyGraph.OwlClasses)
            {
                if (!ontClass.ToString().Contains("auto"))
                {
                    // Get all nodes that have no parent and are classes
                    if (hasParentClass(ontClass) == false)
                    {
                        nodesWithoutParents.Add(ontClass);
                    }
                }
            }

            IList<OntologyClass> baseClasses = new List<OntologyClass>();

            // Set the PassProcessModelElement as root
            foreach (OntologyClass ontNode in nodesWithoutParents)
            {
                if (ontNode.ToString().ToLower().Contains("passprocessmodelelement"))
                {
                    baseClasses.Add(ontNode);
                }
            }
            if (baseClasses.Count > 0) return baseClasses;
            Log.Error("No PassProcessModelElement found in the loaded graphs");
            return null;
        }

        /// <summary>
        /// Checks whether an Ontology class has a parent class or not (in the given graph)
        /// </summary>
        /// <param name="ontClass">the Ontology class</param>
        /// <returns>true if a parent class exists, false if not</returns>
        private bool hasParentClass(OntologyClass ontClass)
        {
            if (ontClass.SuperClasses.ToList().Count > 0)
            {
                foreach (OntologyClass superClass in ontClass.SuperClasses.ToList())
                {
                    if (!(superClass.ToString().ToLower().Contains("auto"))) return true;
                }
            }
            return false;
        }


        // ########################################################################################


        // ################################ C# class tree creation ################################


        /// <summary>
        /// Creates the inheritance tree for the c# classes known to this library
        /// </summary>
        private ITreeNode<IParseablePASSProcessModelElement> createClassInheritanceTree()
        {
            // Start with the default root: the PASSProcessModelElement class
            ITreeNode<IParseablePASSProcessModelElement> treeRootNode = new TreeNode<IParseablePASSProcessModelElement>(new PASSProcessModelElement());

            // Search recursively for classes that extend this class and add them to the tree
            findChildsAndAdd(treeRootNode);
            return treeRootNode;
        }

        private void findChildsAndAdd(ITreeNode<IParseablePASSProcessModelElement> node)
        {
            // Get all classes that are known to the current project and that extend the given node
            IEnumerable<IParseablePASSProcessModelElement> enumerable = ReflectiveEnumerator.getEnumerableOfType(node.getContent());
            foreach (IParseablePASSProcessModelElement element in enumerable.ToList())
            {
                node.addChild(new TreeNode<IParseablePASSProcessModelElement>(element));
            }
            foreach (TreeNode<IParseablePASSProcessModelElement> childNode in node.getChildNodes())
            {
                findChildsAndAdd(childNode);
            }
        }


        // ########################################################################################


        // ##################################### Tree mapping #####################################


        /// <summary>
        /// Creates a parsing dictionary containing Ontology urls as keys and instances of c# classes that can be used to parse the ontology classes.
        /// If an ontlogy class cannot be parsed using any path through both trees (i.e. the trees differ in some places), this class and all its childs will be parsed
        /// using the last instance class where the trees did not differ.
        /// </summary>
        /// <param name="ontClass"></param>
        /// <param name="rootNode"></param>
        private void createParsingStructureFromTrees(IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict,
            OntologyClass ontClass, ITreeNode<IParseablePASSProcessModelElement> rootNode)
        {
            // Start with mapping the roots, they are both PASSProcessModelElement
            if (parsingDict.Count == 0) parsingDict.Add(removeUri(ontClass.Resource.ToString()), new List<(ITreeNode<IParseablePASSProcessModelElement>, int)> { (rootNode, 0) });
            // Create a new dictionary for those urls that could not be mapped properly (need that later)
            ICompDict<OntologyClass, string> unmappableDict = new CompDict<OntologyClass, string>();

            // Start to parse childs, passing the parent ontology class and the parent C#-class, as well as the dict off classes already parsed (only the root)
            parseChilds(parsingDict, ontClass, rootNode, unmappableDict);
            Log.Information("##########################################");
            Log.Information("Created parsing structure for owl classes.");
            Log.Information(parsingDict.Count + " classes could be mapped correctly");
            Log.Information(unmappableDict.Count + " were not mapped correctly");
            Log.Information("##########################################");
            foreach (KeyValuePair<OntologyClass, string> pair in unmappableDict)
            {
                mapRestWithParentNode(parsingDict, pair.Key, pair.Value);
            }
        }


        /// <summary>
        /// Tries to find C#-classes that can parse given urls of the ontology
        /// both structures (the ontology inheritance classes as well as the c# inheritance classes) are structured as a tree
        /// This method is called recursive. It tries to map each child of the given ontology parent class with one or more childs of the given c# parent class.
        /// It is asserted that the c# parent class was previously selected to be able to parse the ontology parent class (else this would make no sense)
        /// this algorithm is trying to map both tree structures together, containing the mapping inside a parsing dictionary.
        /// If a part of the Ontology tree cannot be mapped, the algorithm marks this url in another dict and does not try to map the childs of this element.
        /// </summary>
        /// <param name="parsingDict">Keeps all the valid mappings found down to the current class</param>
        /// <param name="parentOntClass">the parent Ontology class that can be parsed with the instance of IPASSProcessModelElement in the current parentNode</param>
        /// <param name="parentNode">A node containing an instance representing a valid parsing class for the Ontology class given by parentOntClass</param>
        /// <param name="unmappableDict">A dict of elements that could not be mapped</param>
        private void parseChilds(IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict,
            OntologyClass parentOntClass, ITreeNode<IParseablePASSProcessModelElement> parentNode,
            ICompDict<OntologyClass, string> unmappableDict)
        {
            List<ITreeNode<IParseablePASSProcessModelElement>> childsBeenParsed = new List<ITreeNode<IParseablePASSProcessModelElement>>();

            // Go through all ontology child classes of the ontology class
            foreach (OntologyClass directSubclass in parentOntClass.DirectSubClasses)
            {
                List<ITreeNode<IParseablePASSProcessModelElement>> parseableClasses = new List<ITreeNode<IParseablePASSProcessModelElement>>();
                string url = removeUri(directSubclass.Resource.ToString());
                // Go through all c# child classes of the parent c# class instance
                foreach (ITreeNode<IParseablePASSProcessModelElement> childNode in parentNode.getChildNodes())
                {
                    // Check if the class is a correct instanciation for the current url
                    if (childNode.getContent().canParse(url) != PASSProcessModelElement.CANNOT_PARSE)
                    {
                        // add it to parseable classes
                        parseableClasses.Add(childNode);
                    }
                }

                // If no parseable class is found:
                // Add the url to the list of classes that could not be parsed correctly.
                // The parent could be parsed correctly (else this method would not have been called for the child),
                // So the unparsed child will be parsed by using the class that can parse the parent
                if (parseableClasses.Count == 0 && !parsingDict.ContainsKey(url))
                {
                    string parentURI = removeUri(parentOntClass.Resource.ToString());
                    unmappableDict.TryAdd(directSubclass, parentURI);
                }
                else
                {
                    foreach (OntologyClass ontClass in unmappableDict.Keys)
                    {
                        if (removeUri(ontClass.Resource.ToString()).Equals(url)) { unmappableDict.Remove(ontClass); break; }
                    }

                    // We only want to store the most specific instantiation of a class.
                    // All classes that are base classes to the currently found class - with our class still being able to parse the url - will be removed from the list
                    List<(ITreeNode<IParseablePASSProcessModelElement>, int)> toBeRemoved = new List<(ITreeNode<IParseablePASSProcessModelElement>, int)>();
                    if (parsingDict.ContainsKey(url))
                    {
                        foreach (ITreeNode<IParseablePASSProcessModelElement> element in parseableClasses)
                        {
                            foreach ((ITreeNode<IParseablePASSProcessModelElement>, int) tuple in parsingDict[url])
                            {
                                ITreeNode<IParseablePASSProcessModelElement> node = tuple.Item1;
                                if (element.isSubClassOf(node))
                                {
                                    toBeRemoved.Add(tuple);
                                }
                            }
                        }
                    }
                    // Remove all unnecessary classes (because currently found classes are more specific)
                    foreach ((ITreeNode<IParseablePASSProcessModelElement>, int) tuple in toBeRemoved)
                    {
                        parsingDict[url].Remove(tuple);
                    }

                    // Add all classes that can parse the url
                    foreach (ITreeNode<IParseablePASSProcessModelElement> element in parseableClasses)
                    {
                        if (parsingDict.ContainsKey(url))
                        {
                            parsingDict[url].Clear();
                        }
                        addToParsingDict(parsingDict, directSubclass, element, 0);
                        childsBeenParsed.Add(element);
                    }

                    // Then parse the childs of the matched pair respectively:
                    // Now passing the current url and the matched instance as parents
                    foreach (ITreeNode<IParseablePASSProcessModelElement> element in parseableClasses)
                    {
                        parseChilds(parsingDict, directSubclass, element, unmappableDict);
                    }
                }
            }

            // This path will be taken if there is a more specific implementation of a class 
            // Example: In owl, FullySpecifiedSubject exists. In C#, FullySpecifiedSubject and the subclass SpecialFullySpecifiedSubject exist,
            // which should also represent normal FullySpecifiedSubjects from the owl.
            // This code adds the SpecialFullySpecifiedSubject also as possible instanciation to the dictionary
            foreach (ITreeNode<IParseablePASSProcessModelElement> childNode in parentNode.getChildNodes())
            {
                // Find a child that was not specifically mapped to another owl class
                if (!childsBeenParsed.Contains(childNode))
                {
                    // Get all childclasses of the childNode
                    Queue<ITreeNode<IParseablePASSProcessModelElement>> allNodes = new Queue<ITreeNode<IParseablePASSProcessModelElement>>();
                    IList<ITreeNode<IParseablePASSProcessModelElement>> mappableElements = new List<ITreeNode<IParseablePASSProcessModelElement>>();
                    allNodes.Enqueue(childNode);
                    while (allNodes.Count > 0)
                    {
                        ITreeNode<IParseablePASSProcessModelElement> currentElement = allNodes.Dequeue();
                        mappableElements.Add(currentElement);
                        foreach (ITreeNode<IParseablePASSProcessModelElement> child in currentElement.getChildNodes())
                        {
                            allNodes.Enqueue(child);
                        }
                    }
                    string url = removeUri(parentOntClass.Resource.ToString());
                    // Takes all the nodes that say they can parse the url and adds them to the dictionary
                    mappableElements.Where(x => x.getContent().canParse(url) != PASSProcessModelElement.CANNOT_PARSE).ToList().ForEach(x => addToParsingDict(parsingDict, parentOntClass, x, 0));
                }
            }
        }




        /// <summary>
        /// Adds mapped elements to the parsing dictionary.
        /// If there exists a list for the given key, the value is being added to the existing list.
        /// </summary>
        /// <param name="parsingDict">the dictionary used for parsing</param>
        /// <param name="ontClass">the Ontology class used (url as key)</param>
        /// <param name="element">the instance that can parse the ontology class (used as value)</param>
        private void addToParsingDict(IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict, OntologyClass ontClass,
            ITreeNode<IParseablePASSProcessModelElement> element, int depth)
        {
            string ontResource = removeUri(ontClass.Resource.ToString());

            // If the key (the name of the owl class) is present in the mapping, add the new found class to the existing list (the value)
            if (parsingDict.ContainsKey(ontResource))
            {
                parsingDict[ontResource].Add((element, depth));
            }

            // If not, create a new entry with a new list containing one element
            else parsingDict.Add(ontResource, new List<(ITreeNode<IParseablePASSProcessModelElement>, int)> { (element, depth) });
        }

        /// <summary>
        /// This method maps all child classes of an owl class with the same c# class.
        /// If no specific c# class exists for an owl class, then the parser assumes that no specific c# class exists for the children of the owl class exists as well.
        /// All owl classes that are more specific than the mappable parent will not have a c# equivalent, so they are all parsed by using the mapped parent c# class.
        /// </summary>
        /// <param name="parsingDict">The parsing dictionary</param>
        /// <param name="ontClass">The ontology class that could not be mapped to a c# class</param>
        /// <param name="parentNodeKey">the node that was mapped with the parent ontology class of the ontClass</param>

        private void mapRestWithParentNode(IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict, OntologyClass ontClass,
            string parentNodeKey)
        {
            mapRestWithParentNode(parsingDict, ontClass, parentNodeKey, 1);
        }

        /// <summary>
        /// Do not use this directly, use: <br></br>
        /// <see cref="mapRestWithParentNode(IDictionary{string, IList{(ITreeNode{IParseablePASSProcessModelElement}, int)}}, OntologyClass, ITreeNode{IParseablePASSProcessModelElement})"/> <br></br>
        /// instead.
        /// </summary>
        private void mapRestWithParentNode(IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict, OntologyClass ontClass,
            string parentNodeKey, int depth)
        {
            string ontResource = removeUri(ontClass.Resource.ToString());
            IList<(ITreeNode<IParseablePASSProcessModelElement>, int)> possibleMappedClasses = parsingDict[parentNodeKey];
            if (!(parsingDict.ContainsKey(ontResource)))
            {
                string possibleClasses = string.Join(";", possibleMappedClasses.ToList().Select(x => x.Item1.getContent().getClassName()));
                Log.Warning("Could not map " + ontResource + " correctly, mapped with " + possibleClasses + " instead");
                foreach ((ITreeNode<IParseablePASSProcessModelElement>, int) possibleMappedClassPair in possibleMappedClasses)
                {
                    addToParsingDict(parsingDict, ontClass, possibleMappedClassPair.Item1, depth);
                }
            }
            foreach (var childOntClass in ontClass.DirectSubClasses.ToList().Where(childOntClass => !(parsingDict.ContainsKey(removeUri(childOntClass.Resource.ToString())))))
            {
                mapRestWithParentNode(parsingDict, childOntClass, parentNodeKey, depth + 1);
            }
        }

        /// <summary>
        /// Removes a base uri from a string if it is concathenated using #
        /// </summary>
        /// <param name="stringWithUri"></param>
        /// <returns></returns>
        private static string removeUri(string stringWithUri)
        {
            string[] splitStr = stringWithUri.Split('#');
            return splitStr[splitStr.Length - 1];
        }
    }
}
