using alps.net.api.parsing;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class FlowRestrictor : Transition, IFlowRestrictor
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string CLASS_NAME = "FlowRestrictor";


        public override string getClassName()
        {
            return CLASS_NAME;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new FlowRestrictor();
        }

        protected FlowRestrictor() { }

        /// <summary>
        /// The constructor for a FlowRestrictor that is created by passing the referenced states (source and target)
        /// </summary>
        /// <param name="sourceState"></param>
        /// <param name="targetState"></param>
        /// <param name="labelForId">a string describing this element which is used to generate the unique model component id</param>
        /// <param name="transitionCondition"></param>
        /// <param name="transitionType"></param>
        /// <param name="comment"></param>
        /// <param name="additionalLabel"></param>
        /// <param name="additionalAttribute"></param>
        public FlowRestrictor(IState sourceState, IState targetState, string labelForId = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(sourceState, targetState, labelForId, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute)
        {
        }

        /// <summary>
        /// The constructor for a FlowRestrictor that is created by passing the parent behavior (source and target state are optional and can be specified later)
        /// </summary>
        /// <param name="behavior">The behavior on which the FlowRestrictor will be created</param>
        /// <param name="labelForId">a string describing this element which is used to generate the unique model component id</param>
        /// <param name="sourceState"></param>
        /// <param name="targetState"></param>
        /// <param name="transitionCondition"></param>
        /// <param name="transitionType"></param>
        /// <param name="comment"></param>
        /// <param name="additionalLabel"></param>
        /// <param name="additionalAttribute"></param>
        public FlowRestrictor(ISubjectBehavior behavior, string labelForId = null, IState sourceState = null, IState targetState = null,
            ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForId, sourceState, targetState, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute)
        {
        }
    }
}
