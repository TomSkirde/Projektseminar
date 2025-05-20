namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the MessageSenderTypeConstraint Class
    /// </summary>

    public interface IMessageSenderTypeConstraint : IInputPoolConstraint
    {
        /// <summary>
        /// Sets the referenced Message specification
        /// </summary>
        /// <param name="messageSpecification">the referenced Message specification</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setReferencedMessageSpecification(IMessageSpecification messageSpecification, int removeCascadeDepth = 0);

        /// <summary>
        /// Gets the referenced Message specification
        /// </summary>
        /// <returns>the referenced Message specification</returns>
        IMessageSpecification getReferencedMessageSpecification();

        /// <summary>
        /// Sets the referenced subject
        /// </summary>
        /// <param name="subject">the referenced subject</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setReferencedSubject(ISubject subject, int removeCascadeDepth = 0);

        /// <summary>
        /// Gets the referenced Message subject
        /// </summary>
        /// <returns>the referenced Message subject</returns>
        ISubject getReferencedSubject();

    }

}
