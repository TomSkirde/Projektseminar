using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    public class SubjectExecutionMapping : InteractionDescribingComponent, ISubjectExecutionMapping
    {
        private string _executionMapping;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SubjectExecutionMapping";

        public SubjectExecutionMappingTypes executionMappingType { get; set; }

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SubjectExecutionMapping();
        }

        protected SubjectExecutionMapping() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="messageSpecification"></param>
        /// <param name="senderSubject"></param>
        /// <param name="receiverSubject"></param>
        /// <param name="additionalAttribute"></param>
        public SubjectExecutionMapping(IModelLayer layer, string labelForID = null, string executionMapping = null, string comment = null,
            string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setExecutionMappingDefinition(executionMapping);
            executionMappingType = SubjectExecutionMappingTypes.GeneralExecutionMapping;
        }

        public string getExecutionMappingDefinition()
        {
            return this._executionMapping;
        }

        public void setExecutionMappingDefinition(string mapping)
        {
            this._executionMapping = mapping;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasExecutionMappingDefinition))
            {
                setExecutionMappingDefinition(objectContent);
                return true;
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


    }
}
