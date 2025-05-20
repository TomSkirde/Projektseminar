namespace alps.net.api.StandardPASS.PassProcessModelElements.DataDescribingComponents
{
    /// <summary>
    /// Interface to the data mapping function class
    /// </summary>
    public interface IDataMappingFunction : IDataDescribingComponent
    {
        /// <summary>
        /// Sets the data mapping string for the data mapping function
        /// </summary>
        /// <param name="dataMappingString">the data mapping string</param>
        void setDataMappingString(string dataMappingString);

        /// <summary>
        /// Returns the data mapping string
        /// </summary>
        /// <returns>the data mapping string</returns>
        string getDataMappingString();

        /// <summary>
        /// Sets the feel expression string for the data mapping function
        /// </summary>
        /// <param name="FeelExpression">the feel expression</param>
        void setFeelExpressionAsDataMapping(string FeelExpression);

        /// <summary>
        /// Returns the feel expression string for the data mapping function
        /// </summary>
        /// <returns>the feel expression</returns>
        string getFeelExpressionAsDataMapping();

        /// <summary>
        /// Sets a tool specific definition for the data mapping function
        /// </summary>
        /// <param name="toolSpecificDefinition">a tool specific definition</param>
        void setToolSpecificDefinition(string toolSpecificDefinition);

        /// <summary>
        /// Returns the tool specific definition for the data mapping function
        /// </summary>
        /// <returns>a tool specific definition</returns>
        string getToolSpecificDefinition();
        /// <summary>
        /// Checks whether the instance has a toolspecific definition
        /// </summary>
        /// <returns>true if it contains a definition</returns>
        bool hasToolSpecificDefinition();
        /// <summary>
        /// Checks whether the instance has a toolspecific definition
        /// </summary>
        /// <returns>true if it contains a definition</returns>
        public bool hasFeelExpression();
    }

}
