using alps.net.api.util;
using System;
using System.Collections.Generic;
using VDS.RDF;
using VDS.RDF.Ontology;
using VDS.RDF.Writing;

namespace alps.net.api.parsing
{
    /// <summary>
    /// This class is an adapter class for the <see cref="IGraph"/> interface.
    /// It uses an <see cref="OntologyGraph"/> as internal graph.
    /// </summary>
    public class VdsRdfGraph : IPASSGraph
    {


        private ICompDict<string, IPASSGraph.IGraphCallback> elements =
            new CompDict<string, IPASSGraph.IGraphCallback>();

        private readonly ICompDict<string, string> namespaceMappings =
            new CompDict<string, string>
            {
                { "rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#" },
                { "rdfs", "http://www.w3.org/2000/01/rdf-schema#" },
                { "xml", "http://www.w3.org/XML/1998/namespace" },
                { "xsd", "http://www.w3.org/2001/XMLSchema#" },
                { "swrla", "http://swrl.stanford.edu/ontologies/3.3/swrla.owl#" },
                { "abstract-pass-ont", "http://www.imi.kit.edu/abstract-pass-ont#" },
                { "standard-pass-ont", "http://www.i2pm.net/standard-pass-ont#" },
                { "owl", "http://www.w3.org/2002/07/owl#" },
                { "", "http://www.w3.org/1999/02/22-rdf-syntax-ns#" }
            };

        private string baseURI;

        public bool containsNonBaseUri(string input)
        {
            foreach (KeyValuePair<string, string> nameMapping in namespaceMappings)
            {
                if (input.Contains(nameMapping.Value) &&
                    !nameMapping.Key.Equals(ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER.Replace(":", "")))
                    return true;
            }

            return false;
        }

        protected const string EXAMPLE_BASE_URI = "http://www.imi.kit.edu/exampleBaseURI";

        protected IGraph baseGraph;

        public VdsRdfGraph(string baseURI)
        {
            if (baseURI is null)
                this.baseURI = EXAMPLE_BASE_URI;
            else
                this.baseURI = baseURI;
            namespaceMappings.Add(ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER_MAPPING_KEY, baseURI + "#");

            OntologyGraph exportGraph = new();

            // Adding all namespaceMappings (exchange short acronyms like owl: with the complete uri)
            foreach (KeyValuePair<string, string> nameMapping in namespaceMappings)
            {
                exportGraph.NamespaceMap.AddNamespace(nameMapping.Key, new Uri(nameMapping.Value));
            }

            exportGraph.NamespaceMap.AddNamespace("", new Uri(baseURI + "#"));
            exportGraph.BaseUri = new Uri(baseURI);

            INode subjectNode;
            INode predicateNode;
            INode objectNode;
            Triple triple;


            subjectNode = exportGraph.CreateUriNode(exportGraph.BaseUri);
            predicateNode = exportGraph.CreateUriNode("rdf:type");
            objectNode = exportGraph.CreateUriNode("owl:Ontology");
            triple = new Triple(subjectNode, predicateNode, objectNode);
            exportGraph.Assert(triple);


            // Adding import triples for standard pass and abstract pass
            subjectNode = exportGraph.CreateUriNode(exportGraph.BaseUri);
            predicateNode = exportGraph.CreateUriNode("owl:imports");

            objectNode = exportGraph.CreateUriNode(new Uri("http://www.i2pm.net/standard-pass-ont"));
            triple = new Triple(subjectNode, predicateNode, objectNode);
            exportGraph.Assert(triple);
            objectNode = exportGraph.CreateUriNode(new Uri("http://www.imi.kit.edu/abstract-pass-ont"));
            triple = new Triple(subjectNode, predicateNode, objectNode);
            exportGraph.Assert(triple);

            baseGraph = exportGraph;
        }

        public void changeBaseURI(string newUri)
        {
            this.baseURI = newUri ?? EXAMPLE_BASE_URI;

            namespaceMappings[ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER_MAPPING_KEY] = baseURI + "#";
            // baseGraph.NamespaceMap.RemoveNamespace("");
            // baseGraph.NamespaceMap.RemoveNamespace(EXAMPLE_BASE_URI_PLACEHOLDER_MAPPING_KEY);
            baseGraph.NamespaceMap.AddNamespace("", new Uri(baseURI + "#"));
            baseGraph.NamespaceMap.AddNamespace(ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER_MAPPING_KEY, new Uri(baseURI + "#"));
            //exportGraph.NamespaceMap.AddNamespace("", new Uri(baseURI + "#"));
        }

        public IGraph getGraph()
        {
            return baseGraph;
        }


        public void addTriple(IPASSTriple triple)
        {
            INode subjectNode = createNodeIfNotExisting(triple.getSubject());
            INode objectNode = createNodeIfNotExisting(triple.getObjectWithExtra());
            INode predicateNode = createNodeIfNotExisting(triple.getPredicate());
            Triple t = new(subjectNode, predicateNode, objectNode);

            if (baseGraph.Triples.Contains(t)) return;
            baseGraph.Assert(t);
            string subjWithoutUri = t.Subject.ToString().Replace(baseURI + "#", "");
            if (elements.ContainsKey(subjWithoutUri))
            {
                elements[subjWithoutUri].notifyTriple(triple);
            }
        }


        public void removeTriple(IPASSTriple triple)
        {
            INode subjectNode = getNodeOrNull(triple.getSubject());
            INode objectNode = getNodeOrNull(triple.getObjectWithExtra());
            INode predicateNode = getNodeOrNull(triple.getPredicate());
            if (subjectNode is null || objectNode is null || predicateNode is null) { return; }

            Triple t = new(subjectNode, predicateNode, objectNode);

            baseGraph.Retract(t);
        }


        private INode createNodeIfNotExisting(string nodeContent)
        {
            return baseGraph.CreateUriNode(new Uri(nodeContent));
        }

        private INode createNodeIfNotExisting(IStringWithExtra extraString)
        {
            if (extraString == null) { return null; }

            if (extraString.getExtra() is not null && extraString.getExtra() != "")
            {
                if (extraString is LanguageSpecificString l)
                    return baseGraph.CreateLiteralNode(l.getContent(),
                        l.getExtra());
                else if (extraString is DataTypeString d)
                    baseGraph.CreateLiteralNode(d.getContent(),
                        d.getExtra());
            }

            return baseGraph.CreateLiteralNode(extraString.getContent());
        }


        private INode getNodeOrNull(string node)
        {
            return baseGraph.GetUriNode(new Uri(node));
        }

        private INode getNodeOrNull(IStringWithExtra extraString)
        {
            if (extraString == null) { return null; }

            if (extraString is LanguageSpecificString l)
                return baseGraph.GetLiteralNode(l.getContent(),
                    l.getExtra());
            else if (extraString is DataTypeString d)
                baseGraph.GetLiteralNode(d.getContent(),
                    d.getExtra());
            return baseGraph.GetLiteralNode(extraString.getContent());
        }


        public void register(IPASSGraph.IGraphCallback element)
        {
            elements.TryAdd(element.getSubjectName(), element);
        }

        public void unregister(IPASSGraph.IGraphCallback element)
        {
            elements.Remove(element.getSubjectName());
        }

        public void modelComponentIDChanged(string oldID, string newID)
        {
            IList<IPASSGraph.IGraphCallback> elementsToNotify = new List<IPASSGraph.IGraphCallback>();
            foreach (Triple t in baseGraph.Triples)
            {
                if (t.ToString().Contains(oldID))
                {
                    string subjWithoutUri = t.Subject.ToString().Replace(baseURI + "#", "");
                    if (elements.ContainsKey(subjWithoutUri))
                    {
                        elementsToNotify.Add(elements[subjWithoutUri]);
                    }
                }
            }

            foreach (IPASSGraph.IGraphCallback parseable in elementsToNotify)
            {
                parseable.notifyModelComponentIDChanged(oldID, newID);
            }
        }

        public void exportTo(string filepath)
        {
            IRdfWriter writer = new RdfXmlWriter();
            string fullPath = (filepath.EndsWith(".owl")) ? filepath : filepath + ".owl";
            writer.Save(baseGraph, fullPath);
        }

        public string getBaseURI()
        {
            return baseURI;
        }
    }
}