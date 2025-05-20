using alps.net.api.util;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the transition class
    /// </summary>
    public interface ITransition : IBehaviorDescribingComponent, IImplementingElement<ITransition>, IAbstractElement, IHasSimple2DVisualizationLine
    {
        /// <summary>
        /// enum which describes all the possible states a transition can have
        /// </summary>
        public enum TransitionType
        {
            /// <summary>
            /// Standart transition type (if no further specification is give, all transitions are standart)
            /// </summary>
            Standard,
            /// <summary>
            /// Finalized transition type
            /// </summary>
            Finalized,
            /// <summary>
            /// Precedence transition type
            /// </summary>
            Precedence,
            /// <summary>
            /// Trigger transition type
            /// </summary>
            Trigger,
            /// <summary>
            /// Advice transition type
            /// </summary>
            Advice
        }

        /// <summary>
        /// Method that returns the action attribute of the instance
        /// </summary>
        /// <returns>The action attribute of the instance</returns>
        IAction getBelongsToAction();

        /// <summary>
        /// Method that sets the source state (where the transition is coming from)
        /// </summary>
        /// <param name="sourceState">the source state</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setSourceState(IState sourceState, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the source state (where the transition is coming from)
        /// </summary>
        /// <returns>The source state attribute of the instance</returns>
        IState getSourceState();

        /// <summary>
        /// Method that sets the target state (where the transition is going)
        /// </summary>
        /// <param name="targetState"></param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setTargetState(IState targetState, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the target state (where the transition is going)
        /// </summary>
        /// <returns>The target state attribute of the instance</returns>
        IState getTargetState();

        /// <summary>
        /// Method that sets the transition condition attribute of the instance
        /// </summary>
        /// <param name="transitionCondition">the transition condition</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setTransitionCondition(ITransitionCondition transitionCondition, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the transition condition attribute of the instance
        /// </summary>
        /// <returns>The transition condition attribute of the instance</returns>
        ITransitionCondition getTransitionCondition();

        /// <summary>
        /// Sets a type for the current instance
        /// </summary>
        /// <param name="type">The type</param>
        void setTransitionType(TransitionType type);

        /// <summary>
        /// Returns the current type of the transition
        /// </summary>
        /// <returns>the current type</returns>
        TransitionType getTransitionType();

    }

}
