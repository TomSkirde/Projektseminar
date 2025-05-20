using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a transition condition
    /// </summary>
    public class TransitionCondition : BehaviorDescribingComponent, ITransitionCondition
    {
        protected string toolSpecificDefinition = "";

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "TransitionCondition";


        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new TransitionCondition();
        }

        protected TransitionCondition() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="toolSpecificDefintion"></param>
        /// <param name="additionalAttribute"></param>
        public TransitionCondition(ITransition transition, string label = null, string toolSpecificDefintion = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(null, label, comment, additionalLabel, additionalAttribute)
        {
            if (transition != null)
            {
                if (transition.getContainedBy(out ISubjectBehavior behavior))
                    setContainedBy(behavior);
                transition.setTransitionCondition(this);
            }

            setToolSpecificDefinition(toolSpecificDefintion);
        }


        public void setToolSpecificDefinition(string toolSpecificDefinition)
        {
            if (toolSpecificDefinition != null && toolSpecificDefinition.Equals(this.toolSpecificDefinition)) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasToolSpecificDefinition,
                this.toolSpecificDefinition, new PASSTriple.LiteralDataType(OWLTags.xsdDataTypeString)));
            this.toolSpecificDefinition = (toolSpecificDefinition is null || toolSpecificDefinition.Equals(""))
                ? null
                : toolSpecificDefinition;
            if (toolSpecificDefinition != null)
            {
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasToolSpecificDefinition,
                    toolSpecificDefinition, new PASSTriple.LiteralDataType(OWLTags.xsdDataTypeString)));
            }
        }


        public string getToolSpecificDefinition()
        {
            return toolSpecificDefinition;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType,
            IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasToolSpecificDefinition))
            {
                setToolSpecificDefinition(objectContent);
                return true;
            }

            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }
    }
}