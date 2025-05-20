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
    /// Abstract Process Elements are only used on abstract layers that do not specify complete behaviors.
    /// </summary>
    public class ExtensionBehavior : SubjectBehavior, IExtensionBehavior
    {

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string CLASS_NAME = "ExtensionBehavior";
        public override string getClassName()
        {
            return CLASS_NAME;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ExtensionBehavior();
        }

        protected ExtensionBehavior() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="labelForId"></param>
        /// <param name="subject"></param>
        /// <param name="behaviorDescribingComponents"></param>
        /// <param name="initialStateOfBehavior"></param>
        /// <param name="priorityNumber"></param>
        /// <param name="comment"></param>
        /// <param name="additionalLabel"></param>
        /// <param name="additionalAttribute"></param>
        public ExtensionBehavior(IModelLayer layer, string labelForId = null, ISubject subject = null, ISet<IBehaviorDescribingComponent> behaviorDescribingComponents = null,
            IState initialStateOfBehavior = null, int priorityNumber = 0, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForId, subject, behaviorDescribingComponents, initialStateOfBehavior, priorityNumber, comment, additionalLabel, additionalAttribute)
        {
        }

        public override void setSubject(ISubject subj, int removeCascadeDepth = 0)
        {
            ISubject oldSubj = this.subj;

            // Might set it to null
            this.subj = subj;

            if (oldSubj != null)
            {
                if (oldSubj.Equals(subj)) return;
                if (oldSubj is IParseablePASSProcessModelElement parseable)
                    removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasInitialState, parseable.getUriModelComponentID()));
                if (oldSubj is ISubjectExtension oldExtension)
                {
                    oldExtension.removeExtensionBehavior(getModelComponentID());
                }
            }

            if (subj is not ISubjectExtension extension) return;

            if (extension is IParseablePASSProcessModelElement parseable2)
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasInitialState, parseable2.getUriModelComponentID()));
            extension.addExtensionBehavior(this);

        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }

    }
}
