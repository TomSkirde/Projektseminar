using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the subject base behavior class
    /// </summary>
    public interface ISubjectBaseBehavior : ISubjectBehavior
    {
        /// <summary>
        /// Get all the end states this behavior contains.
        /// All these are as well listed in the overall amount of BehaviorDescribingComponents this behavior holds.
        /// </summary>
        /// <returns>A dictionary of states with their ids as keys</returns>
        IDictionary<string, IState> getEndStates();

        /// <summary>
        /// Sets the set of end states for the instance
        /// </summary>
        /// <param name="endStates">The set of end states</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setEndStates(ISet<IState> endStates, int removeCascadeDepth = 0);

        /// <summary>
        /// Makes a state an end state (if it was not already).
        /// Adds the state to the list of behavior describing components (if it was not contained already).
        /// </summary>
        /// <param name="endState">the new end state</param>
        void registerEndState(IState endState);

        /// <summary>
        /// Removes the EndState type from a specified end state.
        /// Does not delete the state from the behavior, use <see cref="removeBehaviorDescribingComponent()"/> for this
        /// </summary>
        /// <param name="id">the id of the end state that should be removed</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void unregisterEndState(string id, int removeCascadeDepth = 0);
    }

}
