namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the FunctionSpecification class
    /// </summary>

    public interface IFunctionSpecification : IBehaviorDescribingComponent
    {
        /// <summary>
        /// Sets a tool-specific Definition
        /// </summary>
        /// <param name="toolSpecificDefinition">a tool-specific Definition</param>
        void setToolSpecificDefinition(string toolSpecificDefinition);

        /// <summary>
        /// Returns the tool-specific Definition
        /// </summary>
        /// <returns>the tool-specific Definition</returns>
        string getToolSpecificDefinition();

    }

}
