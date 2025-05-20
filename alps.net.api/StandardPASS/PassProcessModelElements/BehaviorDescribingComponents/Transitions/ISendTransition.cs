using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the send transition
    /// </summary>
    public interface ISendTransition : ICommunicationTransition
    {
        /// <summary>
        /// Adds a data mapping function (maps local data to the data contained by the message specification) to the set of mapping functions
        /// </summary>
        /// <param name="dataMappingLocalToOutgoing">the new data mapping function</param>
        void addDataMappingFunction(IDataMappingLocalToOutgoing dataMappingLocalToOutgoing);
        /// <summary>
        /// Removes a data mapping function from the set of mapping functions
        /// </summary>
        /// <param name="mappingID">the id of the mapping function</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeDataMappingFunction(string mappingID, int removeCascadeDepth = 0);
        /// <summary>
        /// Overrides the data mapping functions (maps local data to the data contained by the message specification)
        /// </summary>
        /// <param name="dataMappingsLocalToOutgoing">the new data mapping functions </param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setDataMappingFunctionsLocalToOutgoing(ISet<IDataMappingLocalToOutgoing> dataMappingsLocalToOutgoing, int removeCascadeDepth = 0);
        /// <summary>
        /// Gets all data mapping functions (maps local data to the data contained by the message specification) for this instance
        /// </summary>
        /// <returns>all data mapping functions</returns>
        IDictionary<string, IDataMappingLocalToOutgoing> getDataMappingFunctions();

        /// <summary>
        /// Method that returns the transition condition attribute of the instance
        /// </summary>
        /// <returns>The transition condition attribute of the instance</returns>
        new ISendTransitionCondition getTransitionCondition();
        /// <summary>
        /// Method that sets the transition condition attribute of the instance
        /// </summary>
        /// <param name="transitionCondition">the transition condition</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        new void setTransitionCondition(ITransitionCondition condition, int removeCascadeDepth = 0);
    }
}
