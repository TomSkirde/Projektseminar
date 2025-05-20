using alps.net.api.util;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the message exchange class
    /// Note that message exchanges are just a combination of a receiver, a sender, and a message (spec)
    /// In a visual modeling approach often message exchanges are grouped individually 
    /// You can find these in MessageExchangeList-Objects. Those also contain the rudamentary information in regards to 
    /// 2d routing of the accordings
    /// </summary>

    public interface IMessageExchange : IInteractionDescribingComponent, IAbstractElement
    {
        /// <summary>
        /// enum which describes what type an Message Exchange has
        /// </summary>
        public enum MessageExchangeType
        {
            StandardMessageExchange,
            AbstractMessageExchange,
            FinalizedMessageExchange
        }
        /// <summary>
        /// Method that sets the message specification attribute of the instance
        /// </summary>
        /// <param name="messageSpecification">the type of message</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setMessageType(IMessageSpecification messageSpecification, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the message specification attribute of the instance
        /// </summary>
        /// <returns>The message specification attribute of the instance</returns>
        IMessageSpecification getMessageType();

        /// <summary>
        /// Method that sets the receiver attribute of the instance
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setReceiver(ISubject receiver, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the receiver attribute of the instance
        /// </summary>
        /// <returns>The receiver attribute of the instance</returns>
        ISubject getReceiver();

        /// <summary>
        /// Method that sets the sender attribute of the instance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setSender(ISubject sender, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the sender attribute of the instance
        /// </summary>
        /// <returns>The sender attribute of the instance</returns>
        ISubject getSender();

        /// <summary>
        /// Sets a type for the current instance
        /// </summary>
        /// <param name="type">The type</param>
        void setMessageExchangeType(MessageExchangeType type);

        /// <summary>
        /// Returns the current type of the exchange
        /// </summary>
        /// <returns>the current type</returns>
        MessageExchangeType getMessageExchangeType();

    }

   

}
