using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a send function
    /// </summary>
    public class SendFunction : CommunicationAct, ISendFunction
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SendFunction";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SendFunction();
        }

        protected SendFunction() { }
        public SendFunction(ISubjectBehavior behavior, string labelForID = null, string toolSpecificDefinition = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForID, toolSpecificDefinition, comment, additionalLabel, additionalAttribute) { }


    }
}