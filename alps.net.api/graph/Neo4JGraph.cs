using alps.net.api.parsing.graph;
using alps.net.api.util;
using System;
using Neo4j.Driver;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace alps.net.api.parsing
{
    /// <summary>
    /// This class is an adapter class for the <see cref="IGraph"/> interface.
    /// It uses a <see cref="Neo4JGraph"/> as internal graph.
    /// </summary>
    public class Neo4JGraph : IPASSGraph
    {
        private string baseuri;
        private readonly IDriver driver;
        private string nameSpacePrefix;

        public Neo4JGraph(string baseURI, string dbUri, string user, string password)
        {
            driver = GraphDatabase.Driver(dbUri, AuthTokens.Basic(user, password));
            this.baseuri = baseURI;
            nameSpacePrefix = baseURI + ParserConstants.URI_SEPARATOR;
            //retrieveBasePrefix(baseURI).Wait();
        }

        private async Task retrieveBasePrefix(string baseuri)
        {
            using var session = driver.AsyncSession();
            var sessionResult = session.RunAsync("CALL n10s.nsprefixes.list();")
                .Result;

            List<int> blockedSpaces = new();
            while (await sessionResult.FetchAsync())
            {
                var record = sessionResult.Current;
                var lastPrefix = "";
                foreach (var pair in record.Values)
                {
                    if (pair.Key.Contains("prefix") && pair.Value is string prefix)
                    {
                        lastPrefix = prefix;
                        if (Regex.IsMatch(prefix, "ns(\\d)+"))
                        {
                            var matches = Regex.Match(prefix, "ns(\\d)+");
                            var number = int.Parse(matches.Groups[matches.Groups.Count - 1].Value);
                            blockedSpaces.Add(number);
                        }
                    }

                    else if (pair.Key.Contains("namespace") && pair.Value is string nameSpace &&
                             nameSpace.Equals(baseuri))
                    {
                        nameSpacePrefix = lastPrefix + ParserConstants.PLACEHOLDER_SEPARATOR;
                        Console.WriteLine($"Namespace already known to Neo4J-server, using {lastPrefix} as prefix");
                        return;
                    }
                }
            }

            blockedSpaces.Sort();
            int i = 0;
            if (blockedSpaces.Count > 0)
            {
                if (blockedSpaces.Count - 1 == blockedSpaces[blockedSpaces.Count - 1]) i = blockedSpaces.Count;
                else
                {
                    while (i < blockedSpaces.Count)
                    {
                        if (blockedSpaces[i] == i) i++;
                        else { break; }
                    }
                }
            }

            nameSpacePrefix = $"ns{i}" + ParserConstants.PLACEHOLDER_SEPARATOR;

            sessionResult = session.RunAsync($"CALL n10s.nsprefixes.add(\"{nameSpacePrefix}\", \"{baseuri}\");")
                .Result;
        }

        public string getBaseURI()
        {
            return baseuri;
        }

        public bool containsNonBaseUri(string input) { return false; }

        public async void addTriple(IPASSTriple t)
        {
            using var session = driver.AsyncSession();
            string subj = StaticFunctions.replaceSpecificBaseUriWithGeneric(t.getSubject(),
                baseuri, nameSpacePrefix);
            string pred = StaticFunctions.replaceSpecificBaseUriWithGeneric(t.getPredicate(),
                baseuri, nameSpacePrefix);
            var obj = t.getObjectWithExtra();
            string objString;

            // If the obj is not a uri, it has to be wrapped with "" instead of <>
            objString = obj is StringWithoutExtra ? "<" + StaticFunctions.replaceSpecificBaseUriWithGeneric(obj.ToString(),
                baseuri, nameSpacePrefix) + ">" : $"\"{obj.getContent()}\"";

            string query = $"WITH '<{subj}> <{pred}> {objString} .' as payload " +
                           "CALL n10s.rdf.import.inline(payload,\"N-Triples\") YIELD terminationStatus, triplesLoaded " +
                           "RETURN terminationStatus, triplesLoaded";

            // Triple format:
            var sessionResult = session.RunAsync(query)
                .Result;

            while (await sessionResult.FetchAsync())
            {
                var record = sessionResult.Current;
                foreach (var pair in record.Values)
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
            }
        }

        public async void removeTriple(IPASSTriple t)
        {
            using var session = driver.AsyncSession();
            string subj = t.getSubject();
            string pred = t.getPredicate();
            var obj = t.getObjectWithExtra();
            string objString;
            if (obj is StringWithoutExtra) objString = $"<{obj}>";
            else objString = $"\"{obj}\"";

            string query = $"WITH '<{subj}> <{pred}> {obj} .' as payload " +
                           "CALL n10s.rdf.delete.inline(payload,\"N-Triples\") YIELD terminationStatus, triplesDeleted " +
                           "RETURN terminationStatus, triplesDeleted";

            // Triple format:
            var sessionResult = session.RunAsync(
                query).Result;
            while (await sessionResult.FetchAsync())
            {
                var record = sessionResult.Current;
                foreach (var pair in record.Values)
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
            }
        }



        /// <summary>
        /// Registers a component to the graph.
        /// When a triple is changed, the affected component will be notified and can react
        /// to the change
        /// </summary>
        /// <param name="element">the element that is registered</param>
        public void register(IPASSGraph.IGraphCallback element)
        {
        }

        /// <summary>
        /// Deregisteres a component previously registered via <see cref="register(IParseablePASSProcessModelElement)"/>
        /// </summary>
        /// <param name="element">the element that is de-registered</param>
        public void unregister(IPASSGraph.IGraphCallback element)
        {
        }

        /// <summary>
        /// Should be called when a modelComponentID is changed.
        /// The model component ids are like primary keys in a database, and many triples must be updated as result.
        /// Also, the other components inside the model will be notified about the change when they are registered.
        /// </summary>
        /// <param name="oldID">the old id</param>
        /// <param name="newID">the new id</param>
        public void modelComponentIDChanged(string oldID, string newID)
        {
        }

        /// <summary>
        /// Exports the current graph as owl to the specified filename.
        /// </summary>
        /// <param name="filepath"></param>
        public void exportTo(string filepath)
        {
        }

        public void changeBaseURI(string newUri)
        {
        }
    }
}