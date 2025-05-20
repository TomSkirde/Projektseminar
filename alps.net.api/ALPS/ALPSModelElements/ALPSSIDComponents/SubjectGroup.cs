using alps.net.api.ALPS.ALPSModelElements.ALPSSIDComponents;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{

    /// <summary>
    /// This class represents the SystemInterfaceSubject owl class defined in the abstract pass ont.
    /// A SystemInterfaceSubject is an InterfaceSubject which can contain other InterfaceSubjects.
    /// </summary>
    public class SubjectGroup : Subject, ISubjectGroup
    {
        private readonly ICompDict<string, ISubject> containedSubjects
            = new CompDict<string, ISubject>();

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string CLASS_NAME = "SubjectGroup";

        public override string getClassName()
        {
            return CLASS_NAME;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SubjectGroup();
        }

        protected SubjectGroup() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer">The layer this subject should be placed onto</param>
        /// <param name="labelForId">the </param>
        /// <param name="referencedSubject">If the InterfaceSubject is referencing another FullySpecifiedSubject, this can be passed here</param>
        /// <param name="comment"></param>
        /// <param name="incomingMessageExchange"></param>
        /// <param name="containedSubjects"></param>
        /// <param name="outgoingMessageExchange"></param>
        /// <param name="maxSubjectInstanceRestriction"></param>
        /// <param name="additionalLabel"></param>
        /// <param name="additionalAttribute"></param>
        public SubjectGroup(IModelLayer layer, string labelForId = null, ISet<IMessageExchange> incomingMessageExchange = null,
            ISet<ISubject> containedSubjects = null, ISet<IMessageExchange> outgoingMessageExchange = null, int maxSubjectInstanceRestriction = 1,
            IFullySpecifiedSubject referencedSubject = null, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForId, incomingMessageExchange, outgoingMessageExchange, maxSubjectInstanceRestriction,
                comment, additionalLabel, additionalAttribute)
        {
            setSubjects(containedSubjects);
        }


        public bool addSubject(ISubject subject)
        {
            if (subject is null) { return false; }

            if (!containedSubjects.TryAdd(subject.getModelComponentID(), subject)) return false;

            publishElementAdded(subject);
            subject.register(this);
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, subject.getUriModelComponentID()));
            return true;
        }



        public void setSubjects(ISet<ISubject> subjects, int removeCascadeDepth = 0)
        {
            foreach (ISubject mySubject in this.getContainedSubjects().Values)
            {
                removeSubject(mySubject.getModelComponentID(), removeCascadeDepth);
            }
            if (subjects is null) return;
            foreach (ISubject subject in subjects)
            {
                addSubject(subject);
            }
        }

        public bool removeSubject(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return false;
            if (!containedSubjects.TryGetValue(id, out ISubject subject)) return false;

            containedSubjects.Remove(id);
            subject.unregister(this, removeCascadeDepth);
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, subject.getUriModelComponentID()));
            return true;
        }


        public IDictionary<string, ISubject> getContainedSubjects()
        {
            return new Dictionary<string, ISubject>(containedSubjects);
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {

            //Console.WriteLine("parsging in an Subject Group: " + this.getModelComponentID() );
            if (element is ISubject subj && predicate.Contains(OWLTags.contains))
            {
                addSubject(subj);
                return true;
            }

            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }



    }
}
