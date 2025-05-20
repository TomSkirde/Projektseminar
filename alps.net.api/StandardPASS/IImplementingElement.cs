using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{

    public interface IImplementingElement
    {

        /// <summary>
        /// Removes a specified interface from the set of implemented interfaces.
        /// </summary>
        /// <param name="id">the id of the interface that should be removed</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeImplementedInterfaces(string id, int removeCascadeDepth = 0);

        /// <summary>
        /// Sets the set of implemented interfaces for the instance
        /// </summary>
        /// <param name="implementedInterfacesIDs">The set of implemented interfaces</param>
        void setImplementedInterfacesIDReferences(ISet<string> implementedInterfacesIDs);

        /// <summary>
        /// Adds an implemented interface
        /// </summary>
        /// <param name="implementedInterfaceID">the new interface</param>
        void addImplementedInterfaceIDReference(string implementedInterfaceID);

        /// <summary>
        /// Removes a specified interface from the set of implemented interfaces.
        /// </summary>
        /// <param name="id">the id of the interface that should be removed</param>
        void removeImplementedInterfacesIDReference(string id);

        /// <summary>
        /// Returns the interfaces implemented by this instance
        /// </summary>
        /// <returns>the implemented interfaces</returns>
        ISet<string> getImplementedInterfacesIDReferences();
    }
    /// <summary>
    /// An interface for classes that can (in a PASS context) implement other PASS objects which act as interfaces.
    /// </summary>
    /// <typeparam name="T">The type of the implemented classes, usually the type of the implementing class itself</typeparam>
    public interface IImplementingElement<T>: IImplementingElement where T : IPASSProcessModelElement
    {
        /// <summary>
        /// Sets the set of implemented interfaces for the instance
        /// </summary>
        /// <param name="implementedInterfaces">The set of implemented interfaces</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setImplementedInterfaces(ISet<T> implementedInterfaces, int removeCascadeDepth = 0);

        /// <summary>
        /// Adds an implemented interface
        /// </summary>
        /// <param name="implementedInterface">the new interface</param>
        void addImplementedInterface(T implementedInterface);

        /// <summary>
        /// Returns the interfaces implemented by this instance
        /// </summary>
        /// <returns>the implemented interfaces</returns>
        IDictionary<string, T> getImplementedInterfaces();

    }
}
