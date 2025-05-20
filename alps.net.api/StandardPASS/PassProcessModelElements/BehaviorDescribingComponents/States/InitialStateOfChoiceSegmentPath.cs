using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents InitialStateOfChoiceSegmentPath
    /// </summary>

    public class InitialStateOfChoiceSegmentPath : State, IInitialStateOfChoiceSegmentPath
    {
        protected IChoiceSegmentPath choiceSegmentPath;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "InitialStateOfChoiceSegmentPath";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new InitialStateOfChoiceSegmentPath();
        }

        protected InitialStateOfChoiceSegmentPath() { }
        public InitialStateOfChoiceSegmentPath(ISubjectBehavior behavior, string labelForID = null, IGuardBehavior guardBehavior = null,
            IFunctionSpecification functionSpecification = null,
            ISet<ITransition> incomingTransition = null, ISet<ITransition> outgoingTransition = null,
            IChoiceSegmentPath choiceSegmentPath = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForID, guardBehavior, functionSpecification, incomingTransition, outgoingTransition, comment, additionalLabel, additionalAttribute)
        {
            setBelongsToChoiceSegmentPath(choiceSegmentPath);
        }

        public void setBelongsToChoiceSegmentPath(IChoiceSegmentPath choiceSegmentPath)
        {
            IChoiceSegmentPath oldPath = this.choiceSegmentPath;
            // Might set it to null
            this.choiceSegmentPath = choiceSegmentPath;

            if (oldPath != null)
            {
                if (oldPath.Equals(choiceSegmentPath)) return;
                oldPath.unregister(this);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdBelongsTo, oldPath.getModelComponentID()));
            }

            if (!(choiceSegmentPath is null))
            {
                publishElementAdded(choiceSegmentPath);
                choiceSegmentPath.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdBelongsTo, choiceSegmentPath.getModelComponentID()));
            }
        }


        public IChoiceSegmentPath getChoiceSegmentPath()
        {
            return choiceSegmentPath;
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.belongsTo) && element is IChoiceSegmentPath path)
                {
                    setBelongsToChoiceSegmentPath(path);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getChoiceSegmentPath() != null)
                baseElements.Add(getChoiceSegmentPath());
            return baseElements;
        }

    }
}