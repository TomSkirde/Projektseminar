namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface of the transition condition class
    /// </summary>
    public interface ITransitionCondition : IBehaviorDescribingComponent
    {
        /// <summary>
        /// Method that sets the tool specific definition attribute of the instance
        /// </summary>
        /// <param name="toolSpecificDefintion">The tool specific definition</param>
        void setToolSpecificDefinition(string toolSpecificDefintion);

        /// <summary>
        /// Method that returns the tool specific definition attribute of the instance
        /// </summary>
        /// <returns>The tool specific definition attribute of the instance</returns>
        string getToolSpecificDefinition();
    }

}
