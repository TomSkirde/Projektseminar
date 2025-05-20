using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a BehaviorDescriptionComponent
    /// </summary>

    public class BehaviorDescribingComponent : PASSProcessModelElement, IBehaviorDescribingComponent
    {

        protected ISubjectBehavior subjectBehavior;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "BehaviorDescribingComponent";

        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new BehaviorDescribingComponent();
        }

        protected BehaviorDescribingComponent() { }
        /// <summary>
        /// Constructor that creates an instance of the behavior description component
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="additionalAttribute"></param>
        public BehaviorDescribingComponent(ISubjectBehavior subjectBehavior, string labelForID = null, string comment = null,
            string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { setContainedBy(subjectBehavior); }


        public void setContainedBy(ISubjectBehavior subjectBehavior)
        {
            if (this.subjectBehavior != null)
            {
                if (this.subjectBehavior.Equals(subjectBehavior)) return;
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdBelongsTo, this.subjectBehavior.getUriModelComponentID()));
            }

            // Might set it to null
            this.subjectBehavior = subjectBehavior;
            if (!(subjectBehavior is null))
            {
                subjectBehavior.addBehaviorDescribingComponent(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdBelongsTo, subjectBehavior.getUriModelComponentID()));
            }
        }


        public bool getContainedBy(out ISubjectBehavior behavior)
        {
            behavior = subjectBehavior;
            return subjectBehavior != null;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.belongsTo) && element is ISubjectBehavior behavior)
                {
                    setContainedBy(behavior);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }



        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (specification == ConnectedElementsSetSpecification.ALL)
                if (getContainedBy(out ISubjectBehavior behavior)) baseElements.Add(behavior);
            return baseElements;
        }


        protected override IDictionary<string, IParseablePASSProcessModelElement> getDictionaryOfAllAvailableElements()
        {
            if (!getContainedBy(out ISubjectBehavior behavior)) return null;
            if (!behavior.getContainedBy(out IModelLayer layer)) return null;
            if (!layer.getContainedBy(out IPASSProcessModel model)) return null;
            IDictionary<string, IPASSProcessModelElement> allElements = model.getAllElements();
            IDictionary<string, IParseablePASSProcessModelElement> allParseableElements = new Dictionary<string, IParseablePASSProcessModelElement>();
            foreach (KeyValuePair<string, IPASSProcessModelElement> pair in allElements)
                if (pair.Value is IParseablePASSProcessModelElement parseable) allParseableElements.Add(pair.Key, parseable);
            return allParseableElements;
        }

        public void removeFromContainer()
        {
            if (subjectBehavior != null)
                subjectBehavior.removeBehaviorDescribingComponent(getModelComponentID());
            subjectBehavior = null;
        }
    }
}