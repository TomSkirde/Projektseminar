using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Threading;
using System;
using alps.net.api.util;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface of the receive state class
    /// </summary>
    public interface IReceiveState : IStandardPASSState, ICanBeEndState
    {
        /// <summary>
        /// Method that sets the receive function attribute of the instance
        /// </summary>
        /// <param name="specification">the function specification</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        new void setFunctionSpecification(IFunctionSpecification specification, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the receive function attribute of the instance
        /// </summary>
        /// <returns>The receive function attribute of the instance</returns>
        new IReceiveFunction getFunctionSpecification();

        /// <summary>
        /// To Define whether the waiting time here is factored into the cost calculation 
        /// (e.g. if the subject carrier can use the time otherwise, 
        /// this value is 0% and waiting is not factored into the active time and cost 
        /// for the subject exectuion.With a value of 100% the subject carrier is considered 
        /// to be waiting actively and may not do other tasks therefore costing the time
        /// </summary>
        double getSisiBilledWaitingTime();

        /// <summary>
        /// To Define whether the waiting time here is factored into the cost calculation 
        /// (e.g. if the subject carrier can use the time otherwise, 
        /// this value is 0% and waiting is not factored into the active time and cost 
        /// for the subject exectuion.With a value of 100% the subject carrier is considered 
        /// to be waiting actively and may not do other tasks therefore costing the time
        /// </summary>
       /// <param name="billedWaitingTime"> value must be between 0 and 1</param>
        void setSiSiBilledWaitingTime(double billedWaitingTime);


        /// <summary>
        /// Support Function that allows to easily set this Do State to be an End State
        /// Removes or adds the StateType.EndState from this states end States
        /// </summary>
        /// <param name="isEndState">true= make this State an end state, false = remove end-State status</param>
        new void setEndState(Boolean isEndState);

    }

}
