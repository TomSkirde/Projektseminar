using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a communication transition
    /// </summary>
    public class CommunicationTransition : Transition, ICommunicationTransition
    {
        /// <summary>
        /// The name of the class
        /// </summary>
        private const string className = "CommunicationTransition";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new CommunicationTransition();
        }

        protected CommunicationTransition() { }
        public CommunicationTransition(IState sourceState, IState targetState, string labelForID = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null) : base(sourceState, targetState, labelForID, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute) { }

        public CommunicationTransition(ISubjectBehavior behavior, string label = null,
            IState sourceState = null, IState targetState = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, label, sourceState, targetState, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute) { }


        public new IMessageExchangeCondition getTransitionCondition()
        {
            return (IMessageExchangeCondition)transitionCondition;
        }

        public override void setTransitionCondition(ITransitionCondition condition, int removeCascadeDepth = 0)
        {
            if (condition is IMessageExchangeCondition messageCondition) base.setTransitionCondition(messageCondition);
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.hasTransitionCondition) && element is ITransitionCondition condition)
                {
                    setTransitionCondition(condition);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

    }
}