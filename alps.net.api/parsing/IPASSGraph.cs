using alps.net.api.parsing.graph;
using alps.net.api.util;
using System;

namespace alps.net.api.parsing
{
    /// <summary>
    /// This is an interface for a graph used by each model to back up data in form of triples.
    /// The graph is used mainly for exporting, but could also be used for remote control of the model.
    /// It is always kept up to date when something inside the model changes.
    /// </summary>
    public interface IPASSGraph
    {


        public interface IGraphCallback
        {
            void notifyTriple(IPASSTriple triple);

            string getSubjectName();

            void notifyModelComponentIDChanged(string oldID, string newID);
        }


        public string getBaseURI();

        public bool containsNonBaseUri(string input);

        /// <summary>
        /// Adds a triple to the triple store this graph contains
        /// </summary>
        /// <param name="t">the triple</param>
        void addTriple(IPASSTriple t);
        /// <summary>
        /// Removes a triple from the triple store this graph contains
        /// </summary>
        /// <param name="t">the triple</param>
        void removeTriple(IPASSTriple t);




        /// <summary>
        /// Registers a component to the graph.
        /// When a triple is changed, the affected component will be notified and can react
        /// to the change
        /// </summary>
        /// <param name="element">the element that is registered</param>
        void register(IGraphCallback element);

        /// <summary>
        /// De-registers a component previously registered via <see cref="register(IParseablePASSProcessModelElement)"/>
        /// </summary>
        /// <param name="element">the element that is de-registered</param>
        void unregister(IGraphCallback element);

        /// <summary>
        /// Should be called when a modelComponentID is changed.
        /// The model component ids are like primary keys in a database, and many triples must be updated as result.
        /// Also, the other components inside the model will be notified about the change when they are registered.
        /// </summary>
        /// <param name="oldID">the old id</param>
        /// <param name="newID">the new id</param>
        void modelComponentIDChanged(string oldID, string newID);

        /// <summary>
        /// Exports the current graph as owl to the specified filename.
        /// </summary>
        /// <param name="filepath"></param>
        void exportTo(string filepath);

        public void changeBaseURI(string newUri);
    }
}