using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a sending failed transition 
    /// </summary>
    public class SendingFailedTransition : Transition, ISendingFailedTransition
    {
        // TODO check only keyword for source state

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SendingFailedTransition";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SendingFailedTransition();
        }

        protected SendingFailedTransition() { }
        public SendingFailedTransition(IState sourceState, IState targetState, string labelForID = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null) : base(sourceState, targetState, labelForID, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute) { }

        public SendingFailedTransition(ISubjectBehavior behavior, string label = null,
            IState sourceState = null, IState targetState = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, label, sourceState, targetState, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute) { }


        public override void setSourceState(IState source, int removeCascadeDepth = 0)
        {
            if (source is ISendState sendState)
            {
                base.setSourceState(sendState);
            }
        }

        public new ISendState getSourceState()
        {
            return (ISendState)base.getSourceState();
        }



        public override void setTransitionCondition(ITransitionCondition sendingFailedCondition, int removeCascadeDepth = 0)
        {
            if (sendingFailedCondition is ISendingFailedCondition)
            {
                base.setTransitionCondition(sendingFailedCondition);
            }
            else
            {
                base.setTransitionCondition(null);
            }
        }


        public new ISendingFailedCondition getTransitionCondition()
        {
            return (ISendingFailedCondition)base.getTransitionCondition();
        }



        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.hasTransitionCondition) && element is ITransitionCondition sendingFailed)
                {
                    setTransitionCondition(sendingFailed);
                    return true;
                }

                if (predicate.Contains(OWLTags.hasSourceState) && element is IState sendState)
                {
                    setSourceState(sendState);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }



    }
}
