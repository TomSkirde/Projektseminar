using System;
using System.Collections.Generic;
using System.Text;

namespace alps.net.api.util
{
    /// <summary>
    /// For SimpleSimulation: expected cost of a state per Execution
    /// </summary>
    public interface IHasSiSiCostPerExecution
    {
        /// <summary>
        /// The expected cost per Exectuion for a state
        /// </summary>
        double getSisiCostPerExecution();

        /// <summary>
        /// The expected cost per Exectuion for a state
        /// </summary>
        /// <param name="sisiCostPerExecution">a positive value</param>
        void setSisiCostPerExecution(double sisiCostPerExecution);  
    }
}
