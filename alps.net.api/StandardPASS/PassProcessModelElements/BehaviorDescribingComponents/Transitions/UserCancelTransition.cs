using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using Serilog;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a user cancel transition 
    /// </summary>
    public class UserCancelTransition : Transition, IUserCancelTransition
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "UserCancelTransition";

        private double _sisiChoiceChance;
        public double getSisiChoiceChance()
        {
            return this._sisiChoiceChance;
        }

        public void setSisiChoiceChance(double value)
        {
            if (value >= 0.0) { _sisiChoiceChance = value; }
            else { throw new System.ArgumentOutOfRangeException("_sisiChoiceChance", "Value must be between 0.0 and 1.0."); }
        }

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new UserCancelTransition();
        }

        protected UserCancelTransition() { }
        public UserCancelTransition(IState sourceState, IState targetState, string labelForID = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null) : base(sourceState, targetState, labelForID, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute) { }

        public UserCancelTransition(ISubjectBehavior behavior, string labelForID = null,
            IState sourceState = null, IState targetState = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForID, sourceState, targetState, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute) { }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {

            if (predicate.Contains(OWLTags.abstrHasSimpleSimTransitionChoiceChance))
            {
                try
                {
                    this.setSisiChoiceChance(double.Parse(objectContent));
                }
                catch (System.Exception e)
                {
                    Log.Warning("could not parse the value " + objectContent + " as valid double");
                }
                return true;
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);


        }
    }
}
