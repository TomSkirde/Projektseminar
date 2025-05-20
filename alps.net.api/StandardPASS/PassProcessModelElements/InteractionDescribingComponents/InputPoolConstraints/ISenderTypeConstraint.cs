namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the sender type constraint class
    /// </summary>
    public interface ISenderTypeConstraint : IInputPoolConstraint
    {
        /// <summary>
        /// Method that sets the subject attribute of the instance
        /// </summary>
        /// <param name="subject">the subject</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setReferencesSubject(ISubject subject, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the subject attribute of the instance
        /// </summary>
        /// <returns>The subject attribute of the instance</returns>
        ISubject getReferenceSubject();
    }

}
