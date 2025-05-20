using alps.net.api.parsing;
using alps.net.api.StandardPASS;
using System;
using System.Collections.Generic;

namespace alps.net.api.FunctionalityCapsules
{
    /// <summary>
    /// Encapsulates the extends behavior.
    /// Elements can hold this capsule and delegate methods to it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGuardsFunctionalityCapsule<T> : IGuardingElement<T>, IFunctionalityCapsule<T>
    {
    }

    public class GuardsFunctionalityCapsule<T> : IGuardsFunctionalityCapsule<T> where T : IPASSProcessModelElement
    {
        public void addGuardedElement(T guardedElement)
        {
            throw new NotImplementedException();
        }

        public void addGuardedElementIDReference(string guardedElementID)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, T> getGuardedElements()
        {
            throw new NotImplementedException();
        }

        public ISet<string> getGuardedElementsIDReferences()
        {
            throw new NotImplementedException();
        }

        public bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            throw new NotImplementedException();
        }

        public void removeGuardedElement(string id, int removeCascadeDepth = 0)
        {
            throw new NotImplementedException();
        }

        public void removeGuardedElementIDReference(string id)
        {
            throw new NotImplementedException();
        }

        public void setGuardedElements(ISet<T> guardedElements, int removeCascadeDepth = 0)
        {
            throw new NotImplementedException();
        }

        public void setGuardedElementsIDReferences(ISet<string> guardedElementsIDs)
        {
            throw new NotImplementedException();
        }
    }
}
