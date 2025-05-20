using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;
using static alps.net.api.StandardPASS.ITimeTransitionCondition;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a time transition condition
    /// </summary>
    public class TimeTransitionCondition : TransitionCondition, ITimeTransitionCondition
    {
        protected string timeValue = "";
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "TimeTransitionCondition";

        private TimeTransitionConditionType timeTransitionConditionType;

        /// <summary>
        /// This dictionary is used to simplify parsing, since the specific subclasses of the TimeTransitionCondition are not modelled as classes explicitly.
        /// The choice is made because the classes do not have different functionality (only when it comes to im- and export), so it simplifies the usage of
        /// TimeTransitionCondition for users of the library.
        /// </summary>
        IDictionary<int, SpecificTimeTransitionCondition> specificConditions = new Dictionary<int, SpecificTimeTransitionCondition> {
            // CalendarBasedReminderTransitionCondition
            {(int) TimeTransitionConditionType.CalendarBasedReminderTC,
                new SpecificTimeTransitionCondition(OWLTags.CalendarBasedReminderTransitionConditionClassName,
                    OWLTags.hasCalendarBasedFrequencyOrDate,
                    OWLTags.stdHasCalendarBasedFrequencyOrDate, OWLTags.xsdDataTypeString) },

            // TimeBasedReminderTransitionCondition
            {(int) TimeTransitionConditionType.TimeBasedReminderTC,
                new SpecificTimeTransitionCondition(OWLTags.TimeBasedReminderTransitionConditionClassName,
                    OWLTags.hasTimeBasedReoccuranceFrequencyOrDate,
                    OWLTags.stdHasTimeBasedReoccuranceFrequencyOrDate, OWLTags.xsdDataTypeString) },

            // BusinessDayTimerTransitionCondition
            {(int) TimeTransitionConditionType.BusinessDayTimerTC,
                new SpecificTimeTransitionCondition(OWLTags.BusinessDayTimerTransitionConditionClassName,
                    OWLTags.hasBusinessDayDurationTimeOutTime,
                    OWLTags.stdHasBusinessDayDurationTimeOutTime,OWLTags.xsdDayTimeDuration) },

            // DayTimeTimerTransitionCondition
            {(int) TimeTransitionConditionType.DayTimeTimerTC,
                new SpecificTimeTransitionCondition(OWLTags.DayTimeTimerTransitionConditionClassName,
                    OWLTags.hasDayTimeDurationTimeOutTime,
                    OWLTags.stdHasDayTimeDurationTimeOutTime,OWLTags.xsdDayTimeDuration) },

            // YearMonthTimerTransitionCondition
            {(int) TimeTransitionConditionType.YearMonthTimerTC,
                new SpecificTimeTransitionCondition(OWLTags.YearMonthTimerTransitionConditionClassName,
                    OWLTags.hasYearMonthDurationTimeOutTime,
                    OWLTags.stdHasYearMonthDurationTimeOutTime,OWLTags.xsdYearMonthDuration) },




            {(int) TimeTransitionConditionType.TimeTC,
                new SpecificTimeTransitionCondition(OWLTags.TimeTransitionConditionClassName,
                    OWLTags.hasTimeValue,
                    OWLTags.stdHasTimeValue,OWLTags.xsdDataTypeString) },

            {(int) TimeTransitionConditionType.ReminderEventTC,
                new SpecificTimeTransitionCondition(OWLTags.ReminderEventTransitionConditionClassName,
                    OWLTags.hasReoccuranceFrequenyOrDate,
                    OWLTags.stdHasReoccuranceFrequenyOrDate,OWLTags.xsdDataTypeString) },

            {(int) TimeTransitionConditionType.TimerTC,
                new SpecificTimeTransitionCondition(OWLTags.TimerTransitionConditionClassName,
                    OWLTags.hasDurationTimeOutTime,
                    OWLTags.stdHasDurationTimeOutTime,OWLTags.xsdDuration) }

        };

        /// <summary>
        /// Needed for the <see cref="setTimeValue(string)"/> Method. If the type changed since the time value was set the last time,
        /// the old triple parsing the time value must be replaced by a triple containing a different predicate
        /// (All different classes define differnt predicates for the time value).
        /// </summary>
        protected TimeTransitionConditionType lastUsedTypeForExportFunctions;

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new TimeTransitionCondition();
        }

        protected TimeTransitionCondition()
        {
            lastUsedTypeForExportFunctions = TimeTransitionConditionType.DayTimeTimerTC;
            setTimeTransitionConditionType(TimeTransitionConditionType.DayTimeTimerTC);
        }
        public TimeTransitionCondition(ITransition transition, string labelForID = null, string toolSpecificDefintion = null, string timeValue = null,
            TimeTransitionConditionType timeTransitionConditionType = TimeTransitionConditionType.DayTimeTimerTC, string comment = null,
            string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(transition, labelForID, toolSpecificDefintion, comment, additionalLabel, additionalAttribute)
        {
            lastUsedTypeForExportFunctions = timeTransitionConditionType;
            setTimeTransitionConditionType(timeTransitionConditionType);
            setTimeValue(timeValue);
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            // Check if one of the predicates - defined by the different Condition types - is the predicate of the triple
            // For example "hasCalendarBasedFrquencyOrDate" for CalendarBased...
            foreach (SpecificTimeTransitionCondition specific in specificConditions.Values)
            {
                if (predicate.Contains(specific.getTimeValuePredicate(false)))
                {
                    setTimeValue(objectContent);
                    return true;
                }
            }

            // Parse a child of a TimeTransitionCondition correctly.
            if (predicate.Contains(OWLTags.type))
            {
                foreach (KeyValuePair<int, SpecificTimeTransitionCondition> specificPair in specificConditions)
                {
                    if (objectContent.Contains(specificPair.Value.getExportString()))
                    {
                        setTimeTransitionConditionType((TimeTransitionConditionType)specificPair.Key);
                        return true;
                    }
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public void setTimeTransitionConditionType(TimeTransitionConditionType type)
        {
            TimeTransitionConditionType oldType = this.timeTransitionConditionType;
            this.timeTransitionConditionType = type;

            if (oldType.Equals(timeTransitionConditionType)) return;

            // Removes the export tag (if it exists) which defines the element as pure TimeTransitionCondition instance
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + getClassName()));

            // Removes the export tag (if it exists) which defines the element as instance of the previously specified transition condition type 
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + specificConditions[(int)oldType].getExportString()));

            // Adds the export tag which defines the element as instance of the newly specified transition condition type
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + specificConditions[(int)timeTransitionConditionType].getExportString()));

            // Important! The time value must be exported again using the new type to get correct triples
            setTimeValue(timeValue);
        }

        public TimeTransitionConditionType getTimeTransitionType()
        {
            return timeTransitionConditionType;
        }

        public void setTimeValue(string timeValue)
        {
            // The last check is important! If the type changed, the time value triples must be changed too since the predicate differs for different TimeTransitionConditions.
            if (timeValue != null && timeValue.Equals(this.timeValue) && lastUsedTypeForExportFunctions == timeTransitionConditionType) return;

            // We remove the last timeValue triple, which may have a different predicate than the current since the TimeTransitionCondition type could have been different
            SpecificTimeTransitionCondition specificCond = specificConditions[(int)lastUsedTypeForExportFunctions];
            removeTriple(new PASSTriple(getExportXmlName(), specificCond.getTimeValuePredicate(true),
                this.timeValue, new PASSTriple.LiteralDataType(specificCond.getDataType())));


            this.timeValue = (timeValue is null || timeValue.Equals("")) ? null : timeValue;
            if (timeValue != null)
            {
                // We fetch the predicate we need with the current TimeTransitionCondition type and export the value with it
                SpecificTimeTransitionCondition newSpecificCond = specificConditions[(int)timeTransitionConditionType];
                addTriple(new PASSTriple(getExportXmlName(), newSpecificCond.getTimeValuePredicate(true), timeValue, new PASSTriple.LiteralDataType(newSpecificCond.getDataType())));
                lastUsedTypeForExportFunctions = timeTransitionConditionType;
            }

        }

        protected virtual string getTimeTag(bool withStd)
        {
            if (withStd)
                return OWLTags.stdHasTimeValue;
            return OWLTags.hasTimeValue;
        }

        protected virtual string getTimeDatatype()
        {
            return OWLTags.xsdDataTypeString;
        }


        public string getTimeValue()
        {
            return timeValue;
        }


        /// <summary>
        /// Small helper class that keeps all information regarding specific TimeTransitionCondition classes which are not modelled as classes explicitly
        /// </summary>
        class SpecificTimeTransitionCondition
        {
            private string exportString, timeValuePredicate, timeValuePredicateWithPrefix, dataType;
            public SpecificTimeTransitionCondition(string exportString, string timeValuePredicate, string timeValuePredicateWithPrefix, string dataType)
            {
                this.exportString = exportString;
                this.timeValuePredicate = timeValuePredicate;
                this.timeValuePredicateWithPrefix = timeValuePredicateWithPrefix;
                this.dataType = dataType;
            }

            /// <summary>
            /// The export string is the class name of the sepcific subclass.
            /// For example for the TimeBasedReminderTransitionCondition, it would be "TimeBasedReminderTransitionCondition".
            /// It is used for parsing triples to class data and vice versa
            /// </summary>
            public string getExportString() { return exportString; }

            /// <summary>
            /// The time value string is the triple predicate that is used by each specific subclass.
            /// For example the class CalendarBasedReminderTransitionCondition uses the predicate "hasCalendarBasedFrequencyOrDate" to describe its time string,
            /// while the class DayTimeTimerTransitionCondition uses "hasDayTimeDurationTimeOutTime"
            /// </summary>
            /// <param name="withPrefix">if this is true, the predicate also contains the owl prefix, usually "standard-pass-ont:"</param>
            public string getTimeValuePredicate(bool withPrefix) { return (withPrefix) ? timeValuePredicateWithPrefix : timeValuePredicate; }

            /// <summary>
            /// The datatype is the type of the time value for each specific subclass.
            /// For example for the DayTimeTimerTransitionCondition it is "xsd:DayTimeDuration",
            /// while for YearMonthTimerTransitionCondition it is "xsd:YearMonthDuration"
            /// </summary>
            public string getDataType() { return dataType; }
        }
    }
}