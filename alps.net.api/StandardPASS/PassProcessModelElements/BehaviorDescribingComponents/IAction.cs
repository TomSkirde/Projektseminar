using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface that represents an action. This is a construct used in the ontology, but is only present here to guarantee a correct standard.
    /// A user should not create own actions, they will be created automatically when creating a state.
    /// They are only used for export, so there are no writing methods provided to the user.
    /// However, when imported, the correct actions should be loaded and parsed correctly.
    /// </summary>

    public interface IAction : IBehaviorDescribingComponent
    {

        /// <summary>
        /// Returns the state attribute of the action class
        /// </summary>
        /// <returns>The state attribute of the action class</returns>
        IState getState();

        /// <summary>
        /// Returns the outgoing transitions that are connected to the state
        /// </summary>
        /// <returns>The outgoing transitions</returns>
        public IDictionary<string, ITransition> getContainedTransitions();

    }

}
