using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a receive function class
    /// </summary>
    public class ReceiveFunction : CommunicationAct, IReceiveFunction
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "ReceiveFunction";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ReceiveFunction();
        }

        protected ReceiveFunction() { }
        public ReceiveFunction(ISubjectBehavior behavior, string labelForID = null, string toolSpecificDefinition = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForID, toolSpecificDefinition, comment, additionalLabel, additionalAttribute) { }


    }
}