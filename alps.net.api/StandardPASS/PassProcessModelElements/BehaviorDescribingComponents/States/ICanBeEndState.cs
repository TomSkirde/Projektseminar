using System;

namespace alps.net.api.StandardPASS
{
    public interface ICanBeEndState
    {

        /// <summary>
        /// Support function that allows to easily set Do/receive state to be an End State
        /// Removes or adds the StateType.EndState from this state
        /// Equal to remove/set Statetype(StateType.EndState) method
        /// </summary>
        /// <param name="isEndState">true= make this State an end state, false = remove end-State status</param>
        new void setEndState(Boolean isEndState);

        /// <summary>
        /// direct way to determin the whether this state is of the EndState type.
        /// </summary>
        /// <returns>True if this state has the StateType.EndState attribute</returns>
        new Boolean isEndState();

    }
}