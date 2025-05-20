using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using Serilog;
using System.Collections.Generic;
using static alps.net.api.StandardPASS.ITimeTransition;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a time transition 
    /// </summary>
    public class TimeTransition : Transition, ITimeTransition
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "TimeTransition";


        public override string getClassName()
        {
            return className;
        }

        IDictionary<int, string> timeTransitionTypesExportNames = new Dictionary<int, string>  {

            { (int) TimeTransitionType.CalendarBasedReminder, OWLTags.CalendarBasedReminderTransitionClassName },
            { (int) TimeTransitionType.TimeBasedReminder, OWLTags.TimeBasedReminderTransitionClassName },
            { (int) TimeTransitionType.BusinessDayTimer, OWLTags.BusinessDayTimerTransitionClassName },
            { (int) TimeTransitionType.DayTimeTimer, OWLTags.DayTimeTimerTransitionClassName },
            { (int) TimeTransitionType.YearMonthTimer, OWLTags.YearMonthTimerTransitionClassName }
        };


        private TimeTransitionType timeTransitionType = TimeTransitionType.BusinessDayTimer;

        public double _sisiChoiceChance;
        public double getSisiChoiceChance()
        {
            return this._sisiChoiceChance;
        }

        public void setSisiChoiceChance(double value)
        {
            if (value >= 0.0) { _sisiChoiceChance = value; }
            else { throw new System.ArgumentOutOfRangeException("_sisiChoiceChance", "Value must be between 0.0 and 1.0."); }
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new TimeTransition();
        }

        public new ITimeTransitionCondition getTransitionCondition()
        {
            return (ITimeTransitionCondition)transitionCondition;
        }

        public override void setTransitionCondition(ITransitionCondition transitionCondition, int removeCascadeDepth = 0)
        {
            if (transitionCondition is ITimeTransitionCondition) base.setTransitionCondition(transitionCondition, removeCascadeDepth);
        }

        public void setTimeTransitionType(TimeTransitionType type)
        {
            TimeTransitionType oldType = this.timeTransitionType;
            this.timeTransitionType = type;

            if (oldType.Equals(timeTransitionType)) return;

            // Removes the export tag (if it exists) which defines the element as pure TimeTransition instance
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + getClassName()));

            // Removes the export tag (if it exists) which defines the element as instance of the previously specified transition type
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + timeTransitionTypesExportNames[(int)oldType]));

            // Adds the export tag which defines the element as instance of the newly specified transition type
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + timeTransitionTypesExportNames[(int)timeTransitionType]));
        }

        public TimeTransitionType getTimeTransitionType()
        {
            return timeTransitionType;
        }

        protected TimeTransition() { setTimeTransitionType(TimeTransitionType.DayTimeTimer); }
        /// <summary>
        /// Constructor that creates a new fully specified instance of the timer transition class
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="action"></param>
        /// <param name="sourceState"></param>
        /// <param name="targetState"></param>
        /// <param name="transitionCondition"></param>
        /// <param name="TransitionType"></param>
        /// <param name="additionalAttribute"></param>
        public TimeTransition(IState sourceState, IState targetState, string labelForID = null, ITimeTransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, TimeTransitionType timeTransitionType = TimeTransitionType.DayTimeTimer,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(sourceState, targetState, labelForID, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute)
        {
            setTimeTransitionType(timeTransitionType);
        }

        public TimeTransition(ISubjectBehavior behavior, string labelForID = null,
            IState sourceState = null, IState targetState = null, ITimeTransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, TimeTransitionType timeTransitionType = TimeTransitionType.DayTimeTimer,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForID, sourceState, targetState, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute)
        {
            setTimeTransitionType(timeTransitionType);
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {

            // Parse a child of a TimeTransition correctly.
            if (predicate.Contains(OWLTags.type))
            {
                foreach (KeyValuePair<int, string> specificPair in timeTransitionTypesExportNames)
                {
                    if (objectContent.Contains(specificPair.Value))
                    {
                        setTimeTransitionType((TimeTransitionType)specificPair.Key);
                        return true;
                    }
                }
            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimTransitionChoiceChance))
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