namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the InputPoolConstraint class
    /// </summary>

    public interface IInputPoolConstraint : IInteractionDescribingComponent
    {
        /// <summary>
        /// Sets the handling strategy for the input pool contstraint (how to handle incoming messages)
        /// </summary>
        /// <param name="inputPoolConstraintHandlingStrategy">the handling strategy</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setInputPoolConstraintHandlingStrategy(IInputPoolConstraintHandlingStrategy inputPoolConstraintHandlingStrategy, int removeCascadeDepth = 0);

        /// <summary>
        /// returns the current handling strategy for the input pool constraint (how to handle incoming messages)
        /// </summary>
        /// <returns>the handling strategy</returns>
        IInputPoolConstraintHandlingStrategy getInputPoolConstraintHandlingStrategy();

        /// <summary>
        /// Sets a limit for the input pool constraint
        /// </summary>
        /// <param name="nonNegativInteger">the new limit</param>
        void setLimit(int nonNegativInteger);

        /// <summary>
        /// Returns the current limit for the input pool constraint
        /// </summary>
        /// <returns>the current limit</returns>
        int getLimit();

    }

}
