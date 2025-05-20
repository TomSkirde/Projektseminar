using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the GuardBehavior class
    /// </summary>

    public interface IGuardBehavior : ISubjectBehavior
    {
        /// <summary>
        /// Overrides the behaviors that are guarded by this GuardBehavior
        /// </summary>
        /// <param name="behaviors">the new set of guarded behaviors</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setGuardedBehaviors(ISet<ISubjectBehavior> behaviors, int removeCascadeDepth = 0);

        /// <summary>
        /// Adds a behavior to the set of guarded behaviors
        /// </summary>
        /// <param name="behavior">the new guarded behavior</param>
        public void addGuardedBehavior(ISubjectBehavior behavior);

        /// <summary>
        /// Returns all behaviors that are guarded by this instance
        /// </summary>
        /// <returns>A set of behaviors</returns>
        public IDictionary<string, ISubjectBehavior> getGuardedBehaviors();

        /// <summary>
        /// Removes a behavior from the set of guarded behaviors
        /// </summary>
        /// <param name="id"></param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void removeGuardedBehavior(string id, int removeCascadeDepth = 0);

        /// <summary>
        ///  Overrides the states that are guarded by this GuardBehavior
        /// </summary>
        /// <param name="guardedStates">the new set of guarded states</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void setGuardedStates(ISet<IState> guardedStates, int removeCascadeDepth = 0);

        /// <summary>
        /// Adds a state to the set of guarded states
        /// </summary>
        /// <param name="state">the new guarded state</param>
        public void addGuardedState(IState state);

        /// <summary>
        /// Returns all states that are guarded by this instance
        /// </summary>
        /// <returns>A set of states</returns>
        public IDictionary<string, IState> getGuardedStates();

        /// <summary>
        /// Removes a state from the set of guarded states
        /// </summary>
        /// <param name="id">the id of the state that is guarded</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void removeGuardedState(string id, int removeCascadeDepth = 0);

    }

}
