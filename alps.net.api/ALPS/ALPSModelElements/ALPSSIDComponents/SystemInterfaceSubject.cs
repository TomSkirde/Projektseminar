using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace alps.net.api.ALPS
{

    /// <summary>
    /// This class represents the SystemInterfaceSubject owl class defined in the abstract pass ont.
    /// A SystemInterfaceSubject is an InterfaceSubject which can contain other InterfaceSubjects.
    /// </summary>
    public class SystemInterfaceSubject : InterfaceSubject, ISystemInterfaceSubject
    {
        private readonly ICompDict<string, IInterfaceSubject> containedInterfaceSubjects
            = new CompDict<string, IInterfaceSubject>();

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string CLASS_NAME = "SystemInterfaceSubject";

        public override string getClassName()
        {
            return CLASS_NAME;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SystemInterfaceSubject();
        }

        protected SystemInterfaceSubject() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer">The layer this subject should be placed onto</param>
        /// <param name="labelForId">the </param>
        /// <param name="referencedSubject">If the InterfaceSubject is referencing another FullySpecifiedSubject, this can be passed here</param>
        /// <param name="comment"></param>
        /// <param name="incomingMessageExchange"></param>
        /// <param name="containedInterfaceSubjects"></param>
        /// <param name="outgoingMessageExchange"></param>
        /// <param name="maxSubjectInstanceRestriction"></param>
        /// <param name="additionalLabel"></param>
        /// <param name="additionalAttribute"></param>
        public SystemInterfaceSubject(IModelLayer layer, string labelForId = null, ISet<IMessageExchange> incomingMessageExchange = null,
            ISet<IInterfaceSubject> containedInterfaceSubjects = null, ISet<IMessageExchange> outgoingMessageExchange = null, int maxSubjectInstanceRestriction = 1,
            IFullySpecifiedSubject referencedSubject = null, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForId, incomingMessageExchange, outgoingMessageExchange, maxSubjectInstanceRestriction, referencedSubject,
                comment, additionalLabel, additionalAttribute)
        {
            setInterfaceSubjects(containedInterfaceSubjects);
        }


        public bool addInterfaceSubject(IInterfaceSubject subject)
        {
            if (subject is null) { return false; }

            if (!containedInterfaceSubjects.TryAdd(subject.getModelComponentID(), subject)) return false;

            publishElementAdded(subject);
            subject.register(this);
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, subject.getUriModelComponentID()));
            return true;
        }



        public void setInterfaceSubjects(ISet<IInterfaceSubject> subjects, int removeCascadeDepth = 0)
        {
            foreach (IInterfaceSubject interfaceSubject in this.getContainedInterfaceSubjects().Values)
            {
                removeInterfaceSubject(interfaceSubject.getModelComponentID(), removeCascadeDepth);
            }
            if (subjects is null) return;
            foreach (IInterfaceSubject subject in subjects)
            {
                addInterfaceSubject(subject);
            }
        }

        public bool removeInterfaceSubject(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return false;
            if (!containedInterfaceSubjects.TryGetValue(id, out IInterfaceSubject subject)) return false;

            containedInterfaceSubjects.Remove(id);
            subject.unregister(this, removeCascadeDepth);
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, subject.getUriModelComponentID()));
            return true;
        }


        public IDictionary<string, IInterfaceSubject> getContainedInterfaceSubjects()
        {
            return new Dictionary<string, IInterfaceSubject>(containedInterfaceSubjects);
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            //Console.WriteLine("parsging in an System Interface Subject" + this.getModelComponentID());
            if (element is IInterfaceSubject interfaceSubj && predicate.Contains(OWLTags.contains))
            {
                addInterfaceSubject(interfaceSubj);
                return true;
            }

            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }



    }
}
