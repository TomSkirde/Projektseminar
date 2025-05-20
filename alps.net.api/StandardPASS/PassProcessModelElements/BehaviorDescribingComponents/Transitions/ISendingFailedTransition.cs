namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the sending failed transition 
    /// </summary>
    public interface ISendingFailedTransition : ITransition
    {
        // No new xml tags, only part of the signature changed

        /// <summary>
        /// Method that sets the source state (where the transition is coming from)
        /// </summary>
        /// <param name="sourceState">the source state</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public new void setSourceState(IState source, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the source state (where the transition is coming from)
        /// </summary>
        /// <returns>The source state attribute of the instance</returns>
        public new ISendState getSourceState();
        /// <summary>
        /// Method that sets the transition condition attribute of the instance
        /// </summary>
        /// <param name="transitionCondition">the transition condition</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public new void setTransitionCondition(ITransitionCondition sendingFailedCondition, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the transition condition attribute of the instance
        /// </summary>
        /// <returns>The transition condition attribute of the instance</returns>
        public new ISendingFailedCondition getTransitionCondition();

    }

}
