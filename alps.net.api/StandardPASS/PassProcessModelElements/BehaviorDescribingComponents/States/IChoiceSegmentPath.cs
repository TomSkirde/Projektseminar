using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the choice segment path class
    /// </summary>
    public interface IChoiceSegmentPath : IState, IContainableElement<IChoiceSegment>
    {


        /// <summary>
        /// Sets an ending state for the choice segment path
        /// </summary>
        /// <param name="state">an ending state</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setEndState(IState state, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns the ending state for the choice segment path
        /// </summary>
        /// <returns>the ending state</returns>
        IState getEndState();

        /// <summary>
        /// Sets an initial state for the choice segment path
        /// </summary>
        /// <param name="state">an initial state</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setInitialState(IState state, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns the initial state for the choice segment path
        /// </summary>
        /// <returns>the initial state</returns>
        IState getInitialState();

        /// <summary>
        /// Sets whether the path is optional to end or not
        /// </summary>
        /// <param name="endChoice">whether the path is optional to end or not</param>
        void setIsOptionalToEndChoiceSegmentPath(bool endChoice);

        /// <summary>
        /// Returns whether the path is optional to end or not
        /// </summary>
        /// <returns>whether the path is optional to end or not</returns>
        bool getIsOptionalToEndChoiceSegmentPath();

        /// <summary>
        /// Sets whether the path is optional to start or not
        /// </summary>
        /// <param name="endChoice">whether the path is optional to start or not</param>
        void setIsOptionalToStartChoiceSegmentPath(bool endChoice);

        /// <summary>
        /// Returns whether the path is optional to start or not
        /// </summary>
        /// <returns>whether the path is optional to start or not</returns>
        bool getIsOptionalToStartChoiceSegmentPath();

        /// <summary>
        /// Overrides the set of states contained in the choice segment path
        /// </summary>
        /// <param name="containedStates">the new states</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setContainedStates(ISet<IState> containedStates, int removeCascadeDepth = 0);
        /// <summary>
        /// Adds a state to the choice segment path
        /// </summary>
        /// <param name="containedState">the new state</param>
        void addContainedState(IState containedState);
        /// <summary>
        /// Gets all contained states
        /// </summary>
        /// <returns>all states</returns>
        IDictionary<string, IState> getContainedStates();
        /// <summary>
        /// Removes a state from the path
        /// </summary>
        /// <param name="id">the id of the state</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeContainedState(string id, int removeCascadeDepth = 0);


    }

}
