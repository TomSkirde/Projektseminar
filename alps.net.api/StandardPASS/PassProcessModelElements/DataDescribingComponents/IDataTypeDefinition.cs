using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the data type definition class
    /// A datatype contains some data object definition.
    /// I.e. a complex datatype might be "student" containing 3 String fields: "name", "sirname" and "university".
    /// A data object definitions would be "John", "Doe", "KIT", with the datatype "student".
    /// The datatype is defining the structure, the data object is the instance
    /// </summary>
    public interface IDataTypeDefinition : IDataDescribingComponent
    {
        /// <summary>
        /// Overrides the current data object definitions
        /// </summary>
        /// <param name="dataObjectDefinitons">the new definitions</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void setContainsDataObjectDefintions(ISet<IDataObjectDefinition> dataObjectDefinitons, int removeCascadeDepth = 0);
        /// <summary>
        /// Adds a data object definition to the set of definitions
        /// </summary>
        /// <param name="dataObjectDefiniton">the new data object definition</param>
        public void addContainsDataObjectDefintion(IDataObjectDefinition dataObjectDefiniton);
        /// <summary>
        /// Returns all data object definitions 
        /// </summary>
        /// <returns>all definitions</returns>
        public IDictionary<string, IDataObjectDefinition> getDataObjectDefinitons();
        /// <summary>
        /// Removes a data object definition from the set of definitions
        /// </summary>
        /// <param name="modelComponentID">the id of the definition</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void removeDataObjectDefiniton(string modelComponentID, int removeCascadeDepth = 0);
    }
}
