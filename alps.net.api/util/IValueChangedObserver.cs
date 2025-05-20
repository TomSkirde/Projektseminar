
namespace alps.net.api.util
{
    /// <summary>
    /// Interface that represents an observer that waits for components to be added or removed.
    /// Once an element gets added to / removed from another component, this component might be notified (via <see cref="updateAdded"/>, <see cref="updateRemoved"/>)
    /// by the publisher (<see cref="IValueChangedPublisher"/>).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValueChangedObserver<T>
    {
        void updateAdded(T update, T caller);
        void updateRemoved(T update, T caller, int removeCascadeDepth = 0);

        void notifyModelComponentIDChanged(string oldID, string newID);
    }

    /// <summary>
    /// Interface that represents a publisher that informs <see cref="IValueChangedObserver"/> about components being added or removed.
    /// Once an element gets added to / removed from another component, this component can call the notify methods (<see cref="updateAdded"/>, <see cref="updateRemoved"/>)
    /// on its observers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValueChangedPublisher<T>
    {
        /// <summary>
        /// Registers an observer
        /// </summary>
        /// <param name="observer">the observer</param>
        bool register(IValueChangedObserver<T> observer);

        /// <summary>
        /// De-registers an observer
        /// </summary>
        /// <param name="observer">The observer</param>
        /// <param name="removeCascadeDepth">An integer parsing the depth of a cascading delete after this unregister method has been called</param>
        bool unregister(IValueChangedObserver<T> observer, int removeCascadeDepth = 0);
    }
}
