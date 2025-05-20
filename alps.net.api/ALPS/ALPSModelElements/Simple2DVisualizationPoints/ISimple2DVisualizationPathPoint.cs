
namespace alps.net.api.ALPS
{
    /// <summary>
    /// An interface to define paths (consisting of points) for a simple visual representation of model elements
    /// A path is a double linked list of path points
    /// </summary>
    public interface ISimple2DVisualizationPathPoint : ISimple2DVisualizationPoint
    {
        /// <summary>
        /// Sets the next path point in this chain
        /// </summary>
        /// <param name="point">the new next point</param>
        void setNextPathPoint(ISimple2DVisualizationPathPoint point);

        /// <summary>
        /// Sets the previous path point in this chain
        /// </summary>
        /// <param name="point">the new previous point</param>
        void setPreviousPathPoint(ISimple2DVisualizationPathPoint point);

        /// <returns>The next point in the path</returns>
        ISimple2DVisualizationPathPoint getNextPathPoint();

        /// <returns>The previous point in the path</returns>
        ISimple2DVisualizationPathPoint getPreviousPathPoint();
    }
}
