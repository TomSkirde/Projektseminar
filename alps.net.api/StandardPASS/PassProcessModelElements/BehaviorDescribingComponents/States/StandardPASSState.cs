using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a standart PASS state
    /// </summary>
    public class StandardPASSState : State, IStandardPASSState
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "StandardPASSState";



        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new StandardPASSState();
        }

        protected StandardPASSState() { }

        public StandardPASSState(ISubjectBehavior behavior, string labelForID = null, IGuardBehavior guardBehavior = null,
            IFunctionSpecification functionSpecification = null,
            ISet<ITransition> incomingTransition = null, ISet<ITransition> outgoingTransition = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForID, guardBehavior, functionSpecification, incomingTransition, outgoingTransition, comment, additionalLabel, additionalAttribute)
        { }

    }
}
