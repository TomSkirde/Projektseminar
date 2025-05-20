namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the message exchange condition class
    /// </summary>

    public interface IMessageExchangeCondition : ITransitionCondition
    {
        /// <summary>
        /// Sets the message exchange that is required to be sent for this condition to apply
        /// </summary>
        /// <param name="messageExchange">The message exchange</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setRequiresPerformedMessageExchange(IMessageExchange messageExchange, int removeCascadeDepth = 0);

        /// <summary>
        /// Gets the message exchange that is required to be sent for this condition to apply
        /// </summary>
        /// <returns>the message exchange</returns>
        IMessageExchange getRequiresPerformedMessageExchange();

    }

}
