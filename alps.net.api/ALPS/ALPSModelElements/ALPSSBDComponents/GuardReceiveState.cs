using System.Collections.Generic;
using alps.net.api.util;
using alps.net.api.StandardPASS;
using alps.net.api.parsing;
using alps.net.api.src;

namespace alps.net.api.ALPS
{

    public class GuardReceiveState : ReceiveState, IGuardReceiveState
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string CLASS_NAME = "GuardReceiveState";

        public override string getClassName()
        {
            return CLASS_NAME;
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new GuardReceiveState();
        }

        protected GuardReceiveState() { }

        /// <summary>
        /// Constructor that creates a new fully specified instance of the guard receive state class
        /// </summary>
        /// <param name="behavior"></param>
        /// <param name="labelForId">a string describing this element which is used to generate the unique model component id</param>
        /// <param name="guardBehavior"></param>
        /// <param name="functionSpecification"></param>
        /// <param name="incomingTransition"></param>
        /// <param name="outgoingTransition"></param>
        /// <param name="comment"></param>
        /// <param name="additionalLabel"></param>
        /// <param name="additionalAttribute"></param>
        public GuardReceiveState(ISubjectBehavior behavior, string labelForId = null, IGuardBehavior guardBehavior = null,
            IReceiveFunction functionSpecification = null,
            ISet<ITransition> incomingTransition = null, ISet<IReceiveTransition> outgoingTransition = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForId, guardBehavior, functionSpecification, incomingTransition, outgoingTransition, comment, additionalLabel, additionalAttribute)
        { }

    }
}
