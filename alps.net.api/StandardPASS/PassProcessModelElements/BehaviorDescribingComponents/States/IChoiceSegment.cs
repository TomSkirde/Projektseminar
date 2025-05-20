using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the choice segment class
    /// </summary>
    public interface IChoiceSegment : IState
    {
        /// <summary>
        /// Overrides the set of <see cref="IChoiceSegmentPath"/> that are contained by the segment
        /// </summary>
        /// <param name="choiceSegmentPaths">the new segment paths</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setContainsChoiceSegmentPaths(ISet<IChoiceSegmentPath> choiceSegmentPaths, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns all contained paths
        /// </summary>
        /// <returns>Returns a set of <see cref="IChoiceSegmentPath"/> that are contained by the segment</returns>
        IDictionary<string, IChoiceSegmentPath> getChoiceSegmentPaths();

        /// <summary>
        /// Adds a <see cref="IChoiceSegmentPath"/> that is contained by the segment
        /// </summary>
        /// <param name="choiceSegmentPath">the new path</param>
        void addContainsChoiceSegmentPath(IChoiceSegmentPath choiceSegmentPath);

        /// <summary>
        /// Removes a <see cref="IChoiceSegmentPath"/> that is contained by the segment.
        /// </summary>
        /// <param name="id">The ModelComponentID of the path.</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeChoiceSegmentPath(string id, int removeCascadeDepth = 0);

    }
}
