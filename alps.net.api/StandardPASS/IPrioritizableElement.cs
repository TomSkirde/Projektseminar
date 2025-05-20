
namespace alps.net.api.StandardPASS
{
    public interface IPrioritizableElement
    {
        /// <summary>
        /// Sets the priority number of the transition, must be greater than or equal to 0
        /// </summary>
        /// <param name="nonNegativInteger">the priority number</param>
        void setPriorityNumber(int nonNegativInteger);

        /// <summary>
        /// Returns the priority number of the transition
        /// </summary>
        /// <returns>the priority number</returns>
        int getPriorityNumber();
    }
}
