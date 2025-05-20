namespace alps.net.api.util
{
    /// <summary>
    /// This interface defines a bit of a loose hierarchy for the pass process model elements.
    /// A class can implement it and define which class its "container parent" is.
    /// I.e. a State might implement IContainableElement<ISubjectBehavior>, because states are always contained
    /// inside a behavior. if the state is added to a behavior, the behavior can set itself as container
    /// while only checking if the given IPASSProcessModelElement is IContainableElement<ISubjectBehavior>,
    /// it does not need to know it is a specific element (a state)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContainableElement<T>
    {
        /// <summary>
        /// Sets the container for this element
        /// </summary>
        /// <param name="container">the container class</param>
        void setContainedBy(T container);

        /// <summary>
        /// Returns the container this element belongs to
        /// </summary>
        /// <param name="container">the container instance</param>
        /// <returns>true if the container is not null and the element is currently contained by another instance</returns>
        bool getContainedBy(out T container);

        void removeFromContainer();
    }
}
