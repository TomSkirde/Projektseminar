using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// interface of the receive transition class
    /// </summary>
    public interface IReceiveTransition : ICommunicationTransition, IPrioritizableElement
    {
        /// <summary>
        /// Adds a data mapping function (maps data contained by the message specification to the local data) to the set of mapping functions
        /// </summary>
        /// <param name="dataMappingIncomingToLocal">the new mapping function</param>
        public void addDataMappingFunction(IDataMappingIncomingToLocal dataMappingIncomingToLocal);
        /// <summary>
        /// Removes a data mapping function from the set of mapping functions
        /// </summary>
        /// <param name="mappingID">the id of the mapping function</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void removeDataMappingFunction(string mappingID, int removeCascadeDepth = 0);
        /// <summary>
        /// Overrides the data mapping functions (maps data contained by the message specification to the local data)
        /// </summary>
        /// <param name="dataMappingsIncomingToLocal">the new data mapping functions</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void setDataMappingFunctionsIncomingToLocal(ISet<IDataMappingIncomingToLocal> dataMappingsIncomingToLocal, int removeCascadeDepth = 0);
        /// <summary>
        /// Gets all data mapping functions (maps data contained by the message specification to the local data) for this instance
        /// </summary>
        /// <returns>all data mapping functions</returns>
        public IDictionary<string, IDataMappingIncomingToLocal> getDataMappingFunctions();
        /// <summary>
        /// Method that returns the transition condition attribute of the instance
        /// </summary>
        /// <returns>The transition condition attribute of the instance</returns>
        public new IReceiveTransitionCondition getTransitionCondition();
        /// <summary>
        /// Method that sets the transition condition attribute of the instance
        /// </summary>
        /// <param name="transitionCondition">the transition condition</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public new void setTransitionCondition(ITransitionCondition condition, int removeCascadeDepth = 0);

    }
}
