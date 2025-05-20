using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the subject class
    /// </summary>
    public interface ISubject : IHasSimple2DVisualizationBox, IInteractionDescribingComponent, IImplementingElement<ISubject>, IExtendingElement<ISubject>, IAbstractElement
    {

        /// <summary>
        /// Adds a MessageExchange to the list of incoming message exchanges
        /// </summary>
        /// <param name="exchange">The new incoming exchange</param>
        public void addIncomingMessageExchange(IMessageExchange exchange);

        /// <summary>
        /// Adds a MessageExchange to the list of outgoing message exchanges
        /// </summary>
        /// <param name="exchange">The new outgoing exchange</param>
        public void addOutgoingMessageExchange(IMessageExchange exchange);

        /// <summary>
        /// Provides all incoming MessageExchanges mapped with their model component ids
        /// </summary>
        /// <returns>A dictionary of incoming message exchanges</returns>
        public IDictionary<string, IMessageExchange> getIncomingMessageExchanges();

        /// <summary>
        /// Provides all outgoing MessageExchanges mapped with their model component ids
        /// </summary>
        /// <returns>A dictionary of outgoing message exchanges</returns>
        public IDictionary<string, IMessageExchange> getOutgoingMessageExchanges();

        /// <summary>
        /// Overrides the set of incoming exchanges
        /// </summary>
        /// <param name="exchanges"></param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void setIncomingMessageExchanges(ISet<IMessageExchange> exchanges, int removeCascadeDepth = 0);

        /// <summary>
        /// Overrides the set of outgoing exchanges
        /// </summary>
        /// <param name="exchanges">the set of new outgoing exchanges</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void setOutgoingMessageExchanges(ISet<IMessageExchange> exchanges, int removeCascadeDepth = 0);

        /// <summary>
        /// Removes an incoming exchange
        /// </summary>
        /// <param name="id">the id of the exchange</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void removeIncomingMessageExchange(string id, int removeCascadeDepth = 0);

        /// <summary>
        /// Removes an outgoing exchange
        /// </summary>
        /// <param name="id">the id of the exchange</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void removeOutgoingMessageExchange(string id, int removeCascadeDepth = 0);

        /// <summary>
        /// Getter for the instance restriction, defines how often the subject might be instanciated
        /// </summary>
        /// <returns>the instance restriction</returns>
        int getInstanceRestriction();

        /// <summary>
        /// Setter for the instance restriction, defines how often the subject might be instanciated
        /// </summary>
        /// <param name="restriction">the instance restriction</param>
        void setInstanceRestriction(int restriction);

        /// <summary>
        /// Represents roles that can be assigned to a subject
        /// </summary>
        public enum Role
        {
            StartSubject
        }

        /// <summary>
        /// Assigns a role to the current subject
        /// </summary>
        /// <param name="role">the role that will be assigned</param>
        void assignRole(Role role);

        /// <summary>
        /// Checks whether the specified role was assigned to this subject
        /// </summary>
        /// <param name="role">the role that is checked</param>
        /// <returns>true if the subject is assigned to the role, false if not</returns>
        bool isRole(Role role);

        /// <summary>
        /// Unassignes the specified role from the subject
        /// </summary>
        /// <param name="role">the role to be removed</param>
        void removeRole(Role role);

    }
}
