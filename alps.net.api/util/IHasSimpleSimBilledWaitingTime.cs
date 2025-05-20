using System;
using System.Collections.Generic;
using System.Text;

namespace alps.net.api.util
{
    /// <summary>
    /// For simple simulation data values of receive states
    /// Define whether the waiting time is factored into the cost calculation 
    /// (e.g. if the subject carrier can use the time otherwise, this value is 0% and 
    /// waiting is not factored into the active time and cost for the subject exectuion. 
    /// With a value of 100% the subject carrier is considered to be waiting actively and 
    /// may not do other tasks therefore costing the time
    /// </summary>
    public interface IHasSimpleSimBilledWaitingTime
    {
        /// <summary>
        /// Define whether the waiting time is factored into the cost calculation 
        /// (e.g. if the subject carrier can use the time otherwise, this value is 0% and 
        /// waiting is not factored into the active time and cost for the subject exectuion. 
        /// With a value of 100% the subject carrier is considered to be waiting actively and 
        /// may not do other tasks therefore costing the time
        /// </summary>
        /// <returns>the percentage of time is factoring into billing for this receive state</returns>
        double getSiSiBilledWaitingTime();

        /// <summary>
        /// Define whether the waiting time is factored into the cost calculation 
        /// (e.g. if the subject carrier can use the time otherwise, this value is 0% and 
        /// waiting is not factored into the active time and cost for the subject exectuion. 
        /// With a value of 100% the subject carrier is considered to be waiting actively and 
        /// may not do other tasks therefore costing the time
        /// </summary>
        /// <param name="sisiWaitingTimeUsable">a value between 0 and 1</param>
        void setSiSiBilledWaitingTime(double sisiWaitingTimeUsable);
    }
}
