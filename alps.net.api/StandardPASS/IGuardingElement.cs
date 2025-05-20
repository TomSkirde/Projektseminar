using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    public interface IGuardingElement<T>
    {
        /// <summary>
        /// Sets the set of guarded elements for the instance
        /// </summary>
        /// <param name="implementedInterfaces">The set of guarded elements</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setGuardedElements(ISet<T> guardedElements, int removeCascadeDepth = 0);

        /// <summary>
        /// Adds an guarded element
        /// </summary>
        /// <param name="implementedInterface">the new guarded element</param>
        void addGuardedElement(T guardedElement);

        /// <summary>
        /// Removes a specified guarded element from the set of guarded elements.
        /// </summary>
        /// <param name="id">the id of the guarded element that should be removed</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeGuardedElement(string id, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns the elements guarded by this instance
        /// </summary>
        /// <returns>the guarded elements</returns>
        IDictionary<string, T> getGuardedElements();

        /// <summary>
        /// Sets the set of guarded elements for the instance
        /// </summary>
        /// <param name="implementedInterfacesIDs">The set of guarded elements</param>
        void setGuardedElementsIDReferences(ISet<string> guardedElementsIDs);

        /// <summary>
        /// Adds an guarded element
        /// </summary>
        /// <param name="implementedInterfaceID">the new guarded element</param>
        void addGuardedElementIDReference(string guardedElementID);

        /// <summary>
        /// Removes a specified guarded element from the set of guarded elements.
        /// </summary>
        /// <param name="id">the id of the guarded element that should be removed</param>
        void removeGuardedElementIDReference(string id);

        /// <summary>
        /// Returns the elements guarded by this instance
        /// </summary>
        /// <returns>the guarded elements</returns>
        ISet<string> getGuardedElementsIDReferences();
    }
}
