namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interace to the data object definition class
    /// A data object belongs to exactly one data type
    /// I.e. a complex datatype might be "student" containing 3 String fields: "name", "sirname" and "university".
    /// A data object definitions would be "John", "Doe", "KIT", with the datatype "student".
    /// The datatype is defining the structure, the data object is the instance
    /// </summary>
    public interface IDataObjectDefinition : IDataDescribingComponent
    {
        /// <summary>
        /// Sets the datatype definition for the data object definition
        /// </summary>
        /// <param name="dataTypeDefintion">the datatype definition</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setDataTypeDefinition(IDataTypeDefinition dataTypeDefintion, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns the datatype definition for the data object definition
        /// </summary>
        /// <returns>the datatype definition</returns>
        IDataTypeDefinition getDataTypeDefinition();
    }
}
