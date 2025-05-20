namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the send transition condition class
    /// </summary>
    public interface ISendTransitionCondition : IMessageExchangeCondition
    {
        /// <summary>
        /// The different send types as an enum.
        /// </summary>
        public enum SendTypes
        {
            STANDARD,
            SEND_TO_NEW,
            SEND_TO_KNOWN,
            SEND_TO_ALL
        }


        /// <summary>
        /// Method that sets the lower bound attribute of the instance
        /// </summary>
        /// <param name="lowerBound">the lower bound</param>
        void setMultipleSendLowerBound(int lowerBound);

        /// <summary>
        /// Method that returns the lower bound attribute of the instance
        /// </summary>
        /// <returns>The lower bound attribute of the instance</returns>
        int getMultipleLowerBound();

        /// <summary>
        /// Method that sets the upper bound attribute of the instance
        /// </summary>
        /// <param name="upperBound">the upper bound</param>
        void setMultipleSendUpperBound(int upperBound);

        /// <summary>
        /// Method that returns the upper bound attribute of the instance
        /// </summary>
        /// <returns>The upper bound attribute of the instance</returns>
        int getMultipleUpperBound();

        /// <summary>
        /// Method that sets the send type attribute of the instance
        /// </summary>
        /// <param name="sendType">the send type</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setSendType(SendTypes sendType);

        /// <summary>
        /// Method that returns the send type attribute of the instance
        /// </summary>
        /// <returns>The send type attribute of the instance</returns>
        SendTypes getSendType();

        /// <summary>
        /// Sets the subject that must be the receiver of the <see cref="MessageSpecification"/> for this Condition to apply.
        /// </summary>
        /// <param name="subject">The corresponding receiving subject</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setRequiresMessageSentTo(ISubject subject, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns the subject that must be the receiver of the <see cref="MessageSpecification"/> (specified by <see cref="setRequiresSendingOfMessage"/>) for this Condition to apply.
        /// </summary>
        /// <returns>The corresponding receiving subject</returns>
        ISubject getRequiresMessageSentTo();

        /// <summary>
        /// Sets the messageSpecification that must be send for this Condition to apply.
        /// </summary>
        /// <param name="messageSpecification">The corresponding message specification</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setRequiresSendingOfMessage(IMessageSpecification messageSpecification, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns the messageSpecification that must be send for this Condition to apply.
        /// </summary>
        /// <returns>The corresponding message specification</returns>
        IMessageSpecification getRequiresSendingOfMessage();
    }

}
