using alps.net.api.StandardPASS;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// Interface for a subject extension
    /// </summary>
    public interface ISubjectExtension : IALPSSIDComponent, ISubject
    {
        /// <summary>
        /// Adds an extension behavior to the extension subject
        /// </summary>
        /// <param name="behavior">the new behavior</param>
        public void addExtensionBehavior(ISubjectBehavior behavior);

        /// <returns>A set of extension behaviors that belong to this subject extension</returns>
        public IDictionary<string, ISubjectBehavior> getExtensionBehaviors();

        /// <summary>
        /// Overrides the set of behaviors that belong to this extension
        /// </summary>
        /// <param name="behaviors">the new behaviors</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void setExtensionBehaviors(ISet<ISubjectBehavior> behaviors, int removeCascadeDepth = 0);

        /// <summary>
        /// Removes a behavior from the set of behaviors belonging to this subject extension
        /// </summary>
        /// <param name="id">the id of the behavior</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void removeExtensionBehavior(string id, int removeCascadeDepth = 0);

        /// <summary>
        /// Sets the subject that is extended by this extension
        /// </summary>
        /// <param name="subject">the extended subject</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void setExtendedSubject(ISubject subject, int removeCascadeDepth = 0);

        /// <returns>The subject that is extended by this extension</returns>
        public ISubject getExtendedSubject();
    }
}
