using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the send type class
    /// </summary>
    public interface ISendState : IStandardPASSState, IHasDuration, IHasSiSiCostPerExecution
    {

        /// <summary>
        /// Method that sets the send transition attribute of the instance
        /// </summary>
        /// <param name="sendTransition">the send transition</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setSendTransition(ISendTransition sendTransition, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the send transition attribute of the instance
        /// </summary>
        /// <returns>The send transition attribute of the instance</returns>
        ISendTransition getSendTransition();

        /// <summary>
        /// Method that adds a sending failed transition to the set of sending failed transitions
        /// </summary>
        /// <param name="sendingFailedTransition">the transition that is executed when the sending of a message fails</param>
        void addSendingFailedTransition(ISendingFailedTransition sendingFailedTransition);

        /// <summary>
        /// Removes a sending failed transition of the state
        /// </summary>
        /// <param name="id">the id of the transition</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeSendingFailedTransition(string id, int removeCascadingDepth = 0);

        /// <summary>
        /// Overrides all sending failed transitions for the state
        /// </summary>
        /// <param name="transitions">the transition that are executed when the sending of a message fails</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setSendingFailedTransitions(ISet<ISendingFailedTransition> transitions, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that sets the sending failed transition attribute of the instance
        /// </summary>
        /// <returns>The sending failed transition attribute of the instance</returns>
        IDictionary<string,ISendingFailedTransition> getSendingFailedTransitions();

        /// <summary>
        /// Gets the function specification for the current state
        /// </summary>
        /// <returns>the function specification</returns>
        new ISendFunction getFunctionSpecification();

        /// <summary>
        /// Sets the function specification  for the current state
        /// </summary>
        /// <param name="specification">the function specification</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        new void setFunctionSpecification(IFunctionSpecification specification, int removeCascadeDepth = 0);
    }

}
