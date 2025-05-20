using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an DoTransitionCondtition
    /// </summary>

    public class DoTransitionCondition : TransitionCondition, IDoTransitionCondition
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "DoTransitionCondition";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new DoTransitionCondition();
        }

        protected DoTransitionCondition() { }
        public DoTransitionCondition(ITransition transition, string labelForID = null, string toolSpecificDefintion = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(transition, labelForID, toolSpecificDefintion, comment, additionalLabel, additionalAttribute)
        { }

    }
}