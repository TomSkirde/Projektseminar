using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the state class
    /// </summary>
    public interface IState : IBehaviorDescribingComponent, IImplementingElement<IState>, IHasSimple2DVisualizationBox
    {
        /// <summary>
        /// Method that sets the incoming transition attribute of the instance
        /// </summary>
        /// <param name="transition">incoming transition attribute</param>
        void addIncomingTransition(ITransition transition);

        /// <summary>
        /// Method that returns the incoming transition attribute of the instance
        /// </summary>
        /// <returns>The incoming transition attribute of the instance</returns>
        IDictionary<string, ITransition> getIncomingTransitions();

        /// <summary>
        /// Method that sets the outgoing transition attribute of the instance
        /// </summary>
        /// <param name="transition">outgoing transition attribute</param>
        void addOutgoingTransition(ITransition transition);

        /// <summary>
        /// Method that returns the outgoing transition attribute of the instance
        /// </summary>
        /// <returns>The outgoing transition attribute of the instance</returns>
        IDictionary<string, ITransition> getOutgoingTransitions();

        /// <summary>
        /// Method that sets the function specification attribute of the instance
        /// </summary>
        /// <param name="functionSpecification">function specification attribute</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setFunctionSpecification(IFunctionSpecification functionSpecification, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the function specification attribute of the instance
        /// </summary>
        /// <returns>The function specification attribute of the instance</returns>
        IFunctionSpecification getFunctionSpecification();

        /// <summary>
        /// Method that sets the guard behavior attribute of the instance
        /// </summary>
        /// <param name="guardBehavior">guard behavior attribute</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setGuardBehavior(IGuardBehavior guardBehavior, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the guard behavior attribute of the instance
        /// </summary>
        /// <returns>The guard behavior attribute of the instance</returns>
        IGuardBehavior getGuardBehavior();

        /// <summary>
        /// Method that returns the action attribute (describing the bundle of state and outgoing transitions).
        /// No setter exists, because the action is atomatically created and should not be modified.
        /// </summary>
        /// <returns>The action attribute of the instance</returns>
        IAction getAction();

        /// <summary>
        /// Checks if the state is of the given type
        /// </summary>
        /// <param name="stateType">the specified type</param>
        /// <returns>true if the state is of this type</returns>
        bool isStateType(StateType stateType);

        /// <summary>
        /// Sets a new type for this state.
        /// This must not override the old type, a state can have multiple types at once.
        /// Used to make state i.e. an EndState, declared finalized, abstract...
        /// </summary>
        /// <param name="stateType">the new state type</param>
        void setIsStateType(StateType stateType);

        /// <summary>
        /// Removes a type from the list of types this state currently is of.
        /// 
        /// </summary>
        /// <param name="stateType">the type that is removed</param>
        void removeStateType(StateType stateType);

        /// <summary>
        /// Represent different types the state can be of.
        /// A state can have several types at once
        /// </summary>
        public enum StateType
        {
            EndState,
            InitialStateOfBehavior,
            InitialStateOfChoiceSegmentPath,
            Abstract,
            Finalized
        }
        /// <summary>
        /// Overrides the outgoing transitions for the state
        /// </summary>
        /// <param name="transitions">The new outgoing transitions</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void setOutgoingTransitions(ISet<ITransition> transitions, int removeCascadeDepth = 0);
        /// <summary>
        /// Overrides the incoming transitions for the state
        /// </summary>
        /// <param name="transitions">The new incoming transitions</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void setIncomingTransitions(ISet<ITransition> transitions, int removeCascadeDepth = 0);
        /// <summary>
        /// Deletes a transition from the outgoing transitions
        /// </summary>
        /// <param name="modelCompID">The id of the outgoing transition</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void removeOutgoingTransition(string modelCompID, int removeCascadeDepth = 0);
        /// <summary>
        /// Deletes a transition from the incoming transitions
        /// </summary>
        /// <param name="modelCompID">The id of the incoming transition</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void removeIncomingTransition(string modelCompID, int removeCascadeDepth = 0);
    }

}
