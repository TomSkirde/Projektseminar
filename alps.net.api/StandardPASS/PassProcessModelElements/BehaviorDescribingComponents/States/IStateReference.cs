namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the state reference class
    /// </summary>
    public interface IStateReference : IState
    {
        /// <summary>
        /// Sets a state that is referenced by this state.
        /// </summary>
        /// <param name="state">The referenced state</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public void setReferencedState(IState state, int removeCascadeDepth = 0);
        /// <summary>
        /// Gets the state that is referenced by this state.
        /// </summary>
        /// <returns>The referenced state</returns>
        public IState getReferencedState();

    }

}
