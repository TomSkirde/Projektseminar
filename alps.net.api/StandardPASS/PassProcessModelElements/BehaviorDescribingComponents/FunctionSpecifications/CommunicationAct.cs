using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a communication act
    /// </summary>
    public class CommunicationAct : FunctionSpecification, ICommunicationAct
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "CommunicationAct";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new CommunicationAct();
        }

        protected CommunicationAct() { }
        public CommunicationAct(ISubjectBehavior behavior, string labelForID = null, string toolSpecificDefinition = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForID, toolSpecificDefinition, comment, additionalLabel, additionalAttribute) { }

    }
}