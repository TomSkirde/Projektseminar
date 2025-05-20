namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the time transition condition class
    /// </summary>
    public interface ITimeTransitionCondition : ITransitionCondition
    {

        public enum TimeTransitionConditionType
        {
            TimeTC,
            ReminderEventTC,
            TimerTC,
            TimeBasedReminderTC,
            BusinessDayTimerTC,
            CalendarBasedReminderTC,
            DayTimeTimerTC,
            YearMonthTimerTC
        }


        /// <summary>
        /// Method that sets the time value attribute of the instance
        /// </summary>
        /// <param name="timeValue">the time value</param>
        void setTimeValue(string timeValue);

        /// <summary>
        /// Method that returns the time value attribute of the instance
        /// </summary>
        /// <returns>The time value attribute of the instance</returns>
        string getTimeValue();

        public void setTimeTransitionConditionType(TimeTransitionConditionType type);

        public TimeTransitionConditionType getTimeTransitionType();
    }

}
