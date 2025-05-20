using System.Xml;
namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the interface subject class
    /// </summary>

    public interface IInterfaceSubject : ISubject
    {
        // Sollte eigentlich eine Methode containsBehavior haben, da aber max 0 gilt 
        // existiert diese nicht

        /// <summary>
        /// Sets the subject referenced by this interface subject
        /// </summary>
        /// <param name="fullySpecifiedSubject">the referenced subject</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setReferencedSubject(IFullySpecifiedSubject fullySpecifiedSubject, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns the subject referenced by this interface subject
        /// </summary>
        /// <returns>the referenced subject</returns>
        IFullySpecifiedSubject getReferencedSubject();

        void setSimpleSimInterfaceSubjectResponseDefinition(string simpleSimInterfaceSubjectResponseDefinitionString);
        void setSimpleSimInterfaceSubjectResponseDefinition(XmlNode simpleSimInterfaceSubjectResponseDefinition);
        XmlNode getSimpleSimInterfaceSubjectResponseDefinition(); 

    }

}
