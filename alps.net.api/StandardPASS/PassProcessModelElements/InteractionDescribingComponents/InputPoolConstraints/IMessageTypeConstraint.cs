namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the message type constraint class
    /// </summary>
    public interface IMessageTypeConstraint : IInputPoolConstraint
    {
        /// <summary>
        /// Sets the referenced message specification
        /// </summary>
        /// <param name="messageSpecification">the new message specification</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setReferencedMessageSpecification(IMessageSpecification messageSpecification, int removeCascadeDepth = 0);

        /// <summary>
        /// Gets the referenced message specification
        /// </summary>
        /// <returns>the message specification</returns>
        IMessageSpecification getReferencedMessageSpecification();

    }

}
