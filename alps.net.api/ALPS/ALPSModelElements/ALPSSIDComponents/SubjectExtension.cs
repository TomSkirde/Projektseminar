using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// From abstract pass ont:<br></br>
    /// An actor extension is a standard or abstract subject that can/should only exist on layers that extend other layers.
    /// The idea of this subject is, that it extends another subject on an underlying layer
    /// </summary>
    public class SubjectExtension : Subject, ISubjectExtension
    {
        protected readonly ICompDict<string, ISubjectBehavior> extensionBehavior = new CompDict<string, ISubjectBehavior>();
        protected ISubject extendedSubj;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SubjectExtension";

        public override string getClassName()
        {
            return className;
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SubjectExtension();
        }

        protected SubjectExtension() { }

        public SubjectExtension(IModelLayer layer, string labelForID = null, ISubject extendedSubject = null, ISet<ISubjectBehavior> extensionBehavior = null,
             ISet<IMessageExchange> incomingMessageExchange = null, ISet<IMessageExchange> outgoingMessageExchange = null,
            int maxSubjectInstanceRestriction = 1, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, incomingMessageExchange, outgoingMessageExchange, maxSubjectInstanceRestriction, comment, additionalLabel, additionalAttribute)
        {
            setExtendedSubject(extendedSubject);
            setExtensionBehaviors(extensionBehavior);
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if ((predicate.Contains(OWLTags.contains) || predicate.Contains(OWLTags.containsBehavior)) && element is ISubjectBehavior behavior)
                {
                    addExtensionBehavior(behavior);
                    return true;
                }
                if (predicate.Contains(OWLTags.extends) && element is ISubject subject)
                {
                    setExtendedSubject(subject);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }



        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (ISubjectBehavior behavior in getExtensionBehaviors().Values)
                baseElements.Add(behavior);
            if (getExtendedSubject() != null)
                baseElements.Add(getExtendedSubject());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IExtensionBehavior extBehavior)
                    removeExtensionBehavior(extBehavior.getModelComponentID(), removeCascadeDepth);
                if (update is ISubject subj && subj.Equals(getExtendedSubject()))
                    setExtendedSubject(null, removeCascadeDepth);
            }
        }

        public void addExtensionBehavior(ISubjectBehavior behavior)
        {
            if (behavior is null) { return; }
            if (extensionBehavior.TryAdd(behavior.getModelComponentID(), behavior))
            {
                publishElementAdded(behavior);
                behavior.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContainsBehavior, behavior.getUriModelComponentID()));
                if (extendedSubj != null && extendedSubj is IFullySpecifiedSubject subj)
                    subj.addBehavior(behavior);
            }
        }

        public IDictionary<string, ISubjectBehavior> getExtensionBehaviors()
        {
            return new Dictionary<string, ISubjectBehavior>(extensionBehavior);
        }

        public void setExtensionBehaviors(ISet<ISubjectBehavior> behaviors, int removeCascadeDepth = 0)
        {
            foreach (ISubjectBehavior behavior in getExtensionBehaviors().Values)
            {
                removeExtensionBehavior(behavior.getModelComponentID(), removeCascadeDepth);
            }
            if (behaviors is null) return;
            foreach (ISubjectBehavior behavior in behaviors)
            {
                addExtensionBehavior(behavior);
            }
        }

        public void removeExtensionBehavior(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (extensionBehavior.TryGetValue(id, out ISubjectBehavior behavior))
            {
                extensionBehavior.Remove(id);
                behavior.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContainsBehavior, behavior.getUriModelComponentID()));
            }
        }

        public void setExtendedSubject(ISubject subject, int removeCascadeDepth = 0)
        {
            ISubject oldExtended = this.extendedSubj;
            // Might set it to null
            this.extendedSubj = subject;

            if (oldExtended != null)
            {
                if (oldExtended.Equals(subject)) return;
                oldExtended.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstrExtends, oldExtended.getUriModelComponentID()));
                if (oldExtended is IFullySpecifiedSubject subj)
                    foreach (ISubjectBehavior behavior in getExtensionBehaviors().Values)
                    {
                        subj.removeBehavior(behavior.getModelComponentID());
                    }
            }

            if (!(subject is null))
            {
                publishElementAdded(subject);
                subject.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstrExtends, subject.getUriModelComponentID()));
                if (extendedSubj is IFullySpecifiedSubject subj)
                    foreach (ISubjectBehavior behavior in getExtensionBehaviors().Values)
                    {
                        subj.addBehavior(behavior);
                    }
            }
        }



        public ISubject getExtendedSubject()
        {
            return extendedSubj;
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (extensionBehavior.ContainsKey(oldID))
            {
                ISubjectBehavior element = extensionBehavior[oldID];
                extensionBehavior.Remove(oldID);
                extensionBehavior.Add(element.getModelComponentID(), element);
            }
            base.notifyModelComponentIDChanged(oldID, newID);
        }

    }
}
