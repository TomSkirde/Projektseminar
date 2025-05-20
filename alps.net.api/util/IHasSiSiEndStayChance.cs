using System;
using System.Collections.Generic;
using System.Text;

namespace alps.net.api.util
{
    /// <summary>
    /// For SimpleSimulation: to define
    /// </summary>
    public interface IHasSiSiEndStayChance
    {
        // <summary>
        /// For do-end states to define what the likelihood of remining in the state is 
        /// If there should be a do transition to leave the state
        /// SHOULD be a value between 0 and 1
        /// </summary>
        public double getSisiEndStayChance();

        // <summary>
        /// For do-end states to define what the likelihood of remining in the state is 
        /// If there should be a do transition to leave the state
        /// SHOULD be a value between 0 and 1
        /// </summary>
        public void setSisiEndStayChance(double value);

    }
}
