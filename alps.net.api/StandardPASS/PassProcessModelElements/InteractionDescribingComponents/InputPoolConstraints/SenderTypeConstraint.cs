using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a sender type constraint
    /// </summary>
    public class SenderTypeConstraint : InputPoolConstraint, ISenderTypeConstraint
    {
        protected ISubject subject;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SenderTypeConstraint";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SenderTypeConstraint();
        }

        protected SenderTypeConstraint() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="inputPoolConstraintHandlingStrategy"></param>
        /// <param name="limit"></param>
        /// <param name="subject"></param>
        /// <param name="additionalAttribute"></param>
        public SenderTypeConstraint(IModelLayer layer, string labelForID = null, IInputPoolConstraintHandlingStrategy inputPoolConstraintHandlingStrategy = null,
            int limit = 0, ISubject subject = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, inputPoolConstraintHandlingStrategy, limit, comment, additionalLabel, additionalAttribute)
        {

            setReferencesSubject(subject);
        }


        public void setReferencesSubject(ISubject subject, int removeCascadeDepth = 0)
        {
            ISubject oldSubj = subject;
            // Might set it to null
            this.subject = subject;

            if (oldSubj != null)
            {
                oldSubj.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdReferences, oldSubj.getUriModelComponentID()));
            }

            if (!(subject is null))
            {
                publishElementAdded(subject);
                subject.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdReferences, subject.getUriModelComponentID()));
            }
        }


        public ISubject getReferenceSubject()
        {
            return subject;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.references) && element is ISubject subject)
                {
                    setReferencesSubject(subject);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getReferenceSubject() != null)
                baseElements.Add(getReferenceSubject());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is ISubject subject && subject.Equals(getReferenceSubject()))
                    setReferencesSubject(null, removeCascadeDepth);
            }
        }
    }
}