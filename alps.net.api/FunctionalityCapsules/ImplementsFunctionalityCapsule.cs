using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.FunctionalityCapsules
{
    /// <summary>
    /// This encapsulates the functionality for handling implements relations between elements.
    /// Every element can hold a capsule, delegating all the incoming calls to this capsule.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IImplementsFunctionalityCapsule<T> : IImplementingElement<T>, IFunctionalityCapsule<T>
        where T : IPASSProcessModelElement
    {
    }


    public class ImplementsFunctionalityCapsule<T> : IImplementsFunctionalityCapsule<T>
        where T : IPASSProcessModelElement
    {
        protected readonly ICompDict<string, T> implementedInterfaces =
            new CompDict<string, T>();

        protected readonly ISet<string> implementedInterfacesIDs = new HashSet<string>();
        protected readonly ICapsuleCallback callback;

        public ImplementsFunctionalityCapsule(ICapsuleCallback callback)
        {
            this.callback = callback;
        }


        public bool parseAttribute(string predicate, string objectContent, string lang, string dataType,
            IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.implements))
            {
                if (element is T fittingElement)
                {
                    addImplementedInterface(fittingElement);
                    return true;
                }
                else
                {
                    addImplementedInterfaceIDReference(objectContent);
                    return true;
                }
            }

            return false;
        }

        public void setImplementedInterfaces(ISet<T> implementedInterface, int removeCascadeDepth = 0)
        {
            foreach (T implInterface in getImplementedInterfaces().Values)
            {
                removeImplementedInterfaces(implInterface.getModelComponentID(), removeCascadeDepth);
            }

            if (implementedInterface is null) return;
            foreach (T implInterface in implementedInterface)
            {
                addImplementedInterface(implInterface);
            }
        }

        public void addImplementedInterface(T implementedInterface)
        {
            if (implementedInterface is null) { return; }

            if (implementedInterfaces.TryAdd(implementedInterface.getModelComponentID(), implementedInterface))
            {
                callback.publishElementAdded(implementedInterface);
                implementedInterface.register(callback);
                callback.addTriple(new PASSTriple(callback.getExportXmlName(), OWLTags.abstrImplements,
                    implementedInterface.getUriModelComponentID()));
            }
        }

        public void removeImplementedInterfaces(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (implementedInterfaces.TryGetValue(id, out T implInterface))
            {
                implementedInterfaces.Remove(id);
                implInterface.unregister(callback, removeCascadeDepth);
                callback.removeTriple(new PASSTriple(
                    callback.getExportXmlName(), OWLTags.abstrImplements, implInterface.getUriModelComponentID()));
            }
        }

        public IDictionary<string, T> getImplementedInterfaces()
        {
            return new Dictionary<string, T>(implementedInterfaces);
        }


        public void setImplementedInterfacesIDReferences(ISet<string> implementedInterfacesIDs)
        {
            implementedInterfacesIDs.Clear();
            foreach (string implementedInterfaceID in implementedInterfacesIDs)
                implementedInterfacesIDs.Add(implementedInterfaceID);
        }

        public void addImplementedInterfaceIDReference(string implementedInterfaceID)
        {
            implementedInterfacesIDs.Add(implementedInterfaceID);
        }

        public void removeImplementedInterfacesIDReference(string implementedInterfaceID)
        {
            implementedInterfacesIDs.Remove(implementedInterfaceID);
        }

        public ISet<string> getImplementedInterfacesIDReferences()
        {
            ISet<string> ts = new HashSet<string>(implementedInterfacesIDs);
            foreach (string implementedInterfaceID in this.implementedInterfaces.Keys)
                ts.Add(implementedInterfaceID);
            return ts;
        }
    }
}