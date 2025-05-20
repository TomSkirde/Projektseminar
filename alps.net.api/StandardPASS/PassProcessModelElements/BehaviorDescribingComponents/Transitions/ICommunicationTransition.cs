namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the communication transition class
    /// </summary>
    public interface ICommunicationTransition : ITransition
    {
        /// <summary>
        /// Method that returns the transition condition attribute of the instance
        /// </summary>
        /// <returns>The transition condition attribute of the instance</returns>
        new IMessageExchangeCondition getTransitionCondition();

        /// <summary>
        /// Method that sets the transition condition attribute of the instance
        /// </summary>
        /// <param name="transitionCondition">the transition condition</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        new void setTransitionCondition(ITransitionCondition condition, int removeCascadeDepth);
    }
}
