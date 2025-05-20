using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an FunctionSpecification
    /// </summary>
    public class FunctionSpecification : BehaviorDescribingComponent, IFunctionSpecification
    {
        protected string toolSpecificDefinition;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "FunctionSpecification";


        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new FunctionSpecification();
        }

        protected FunctionSpecification() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="toolSpecificDefinition"></param>
        /// <param name="additionalAttribute"></param>
        public FunctionSpecification(ISubjectBehavior behavior, string labelForID = null,
            string toolSpecificDefinition = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setToolSpecificDefinition(toolSpecificDefinition);
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