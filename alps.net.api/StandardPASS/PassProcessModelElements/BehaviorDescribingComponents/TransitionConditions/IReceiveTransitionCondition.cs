namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the receive transition condition class
    /// </summary>
    public interface IReceiveTransitionCondition : IMessageExchangeCondition
    {

        /// <summary>
        /// The different receive types as an enum.
        /// </summary>
        public enum ReceiveTypes
        {
            STANDARD,
            RECEIVE_FROM_KNOWN,
            RECEIVE_FROM_ALL
        }

        /// <summary>
        /// Method that sets the lower bound attribute of the instance
        /// </summary>
        /// <param name="lowerBound">the lower bound</param>
        void setMultipleReceiveLowerBound(int lowerBound);

        /// <summary>
        /// Method that returns the lower bound attribute of the instance
        /// </summary>
        /// <returns>The lower bound attribute of the instance</returns>
        int getMultipleLowerBound();

        /// <summary>
        /// Method that sets the upper bound attribute of the instance
        /// </summary>
        /// <param name="upperBound">the upper bound</param>
        void setMultipleReceiveUpperBound(int upperBound);

        /// <summary>
        /// Method that returns the upper bound attribute of the instance
        /// </summary>
        /// <returns>The upper bound attribute of the instance</returns>
        int getMultipleUpperBound();

        /// <summary>
        /// Method that sets the receive type attribute of the instance
        /// </summary>
        /// <param name="receiveType">the receive type</param>
        void setReceiveType(ReceiveTypes receiveType);

        /// <summary>
        /// Method that returns the receive type attribute of the instance
        /// </summary>
        /// <returns>The receive type attribute of the instance</returns>
        ReceiveTypes getReceiveType();

        /// <summary>
        /// Sets the subject that must be the sender of the <see cref="MessageSpecification"/> for this Condition to apply.
        /// </summary>
        /// <param name="subject">the subject the message is sent from</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setMessageSentFrom(ISubject subject, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns the subject that must be the sender of the <see cref="MessageSpecification"/> (specified by <see cref="setRequiresSendingOfMessage"/>) for this Condition to apply.
        /// </summary>
        /// <returns>The subject attribute of the instance</returns>
        ISubject getMessageSentFrom();

        /// <summary>
        /// Method that sets the message specification attribute of the instance
        /// </summary>
        /// <param name="messageSpecification">the specification of the message</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setReceptionOfMessage(IMessageSpecification messageSpecification, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the message specification attribute of the instance
        /// </summary>
        /// <returns>The message specification attribute of the instance</returns>
        IMessageSpecification getReceptionOfMessage();
    }
}
