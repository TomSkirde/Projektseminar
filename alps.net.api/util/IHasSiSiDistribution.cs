using System;
using System.Collections.Generic;
using System.Text;

namespace alps.net.api.util
{
    public interface IHasSiSiTimeCategory
    {
        /// <summary>
        /// For Simple Simulations for state Set the type of time Spend 
        /// Standard;Main Processing Time;Secondary Processing Time;Waiting Time
        /// </summary>
        SimpleSimTimeCategory getSisiVSMTimeCategory();

        /// <summary>
        /// For Simple Simulations for state Set the type of time Spend 
        /// Standard;Main Processing Time;Secondary Processing Time;Waiting Time
        /// </summary>
        void setSisiVSMTimeCategory(SimpleSimTimeCategory simpleSimTimeCategory);
    }
    public interface IHasDuration
    {
        /// <summary>
        /// For simple simulation of processes: The (expected) transmission time of this kind of message. Necessary only for simulation purposes
        /// </summary>
        ISiSiTimeDistribution getSisiExecutionDuration();


        /// <summary>
        /// For simple simulation of processes: The (expected) transmission time of this kind of message. Necessary only for simulation purposes
        /// </summary>
        void setSisiExecutionDuration (ISiSiTimeDistribution sisiExecutionDuration);

    }


    /// <summary>
    /// For Simple Simulations for state Set the type of time Spend 
    /// Standard;Main Processing Time;Secondary Processing Time;Waiting Time
    /// </summary>
    public enum SimpleSimTimeCategory
    {
        Standard,
        Main,
        Secondary,
        Waiting
    }

}
