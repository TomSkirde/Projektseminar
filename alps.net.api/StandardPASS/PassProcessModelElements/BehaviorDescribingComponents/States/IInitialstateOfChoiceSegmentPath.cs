namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the InitialStateOfChoiceSegmentPath class
    /// </summary>

    public interface IInitialStateOfChoiceSegmentPath : IState
    {
        /// <summary>
        /// Sets the choice segment path that contains this state as initial state
        /// </summary>
        /// <param name="choiceSegmentPath">the choice segment path</param>
        void setBelongsToChoiceSegmentPath(IChoiceSegmentPath choiceSegmentPath);

        /// <summary>
        /// Gets the choice segment path that contains this state as initial state
        /// </summary>
        /// <returns>the choice segment path</returns>
        IChoiceSegmentPath getChoiceSegmentPath();
    }

}
