using System.Collections.Generic;
using alps.net.api.util;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the message exchange list class
    /// </summary>

    public interface IMessageExchangeList : IInteractionDescribingComponent, IHasSimple2DVisualizationLine
    {
        /// <summary>
        /// Adds a message exchange to the exchange list
        /// </summary>
        /// <param name="messageExchange">the new message exchange</param>
        void addContainsMessageExchange(IMessageExchange messageExchange);

        /// <summary>
        /// Overrides the set of contained message exchanges
        /// </summary>
        /// <param name="messageExchanges">a set of new message exchanges</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setContainsMessageExchanges(ISet<IMessageExchange> messageExchanges, int removeCascadeDepth = 0);
        /// <summary>
        /// Returns all message exchanges contained by the list, mapped with their ids
        /// </summary>
        /// <returns>A dictionary containing all message exchanges</returns>
        IDictionary<string, IMessageExchange> getMessageExchanges();
        /// <summary>
        /// Removes a message exchange from the list
        /// </summary>
        /// <param name="id">the id of the message exchange</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void removeMessageExchange(string id, int removeCascadeDepth = 0);

    }
}
