using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;

namespace alps.net.api.FunctionalityCapsules
{
    /// <summary>
    /// Encapsulates the extends behavior.
    /// Elements can hold this capsule and delegate methods to it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IExtendsFunctionalityCapsule<T> : IExtendingElement<T>, IFunctionalityCapsule<T>
    {
    }


    public class ExtendsFunctionalityCapsule<T> : IExtendsFunctionalityCapsule<T> where T : IPASSProcessModelElement
    {
        protected T extendedElement;
        protected string extendedElementID;
        protected readonly ICapsuleCallback callback;

        public ExtendsFunctionalityCapsule(ICapsuleCallback callback)
        {
            this.callback = callback;
        }

        public T getExtendedElement()
        {
            return extendedElement;
        }

        public string getExtendedElementID()
        {
            if ((extendedElement is not null) && !extendedElement.getModelComponentID().Equals(extendedElementID))
            {
                setExtendedElementID(extendedElement.getModelComponentID());
            }
            return extendedElementID;
        }

        public bool isExtension()
        {
            if (getExtendedElement() != null || getExtendedElementID() != null)
                return true;
            return false;
        }

        public bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.extends))
            {
                if (element is T fittingElement)
                {
                    setExtendedElement(fittingElement);
                    return true;
                }
                else
                {
                    setExtendedElementID(objectContent);
                    return true;
                }
            }
            return false;
        }

        public void setExtendedElement(T element)
        {
            T oldExtends = extendedElement;
            // Might set it to null
            this.extendedElement = element;

            if (oldExtends is not null)
            {
                if (oldExtends.Equals(element)) return;
                oldExtends.unregister(callback);
                callback.removeTriple(new PASSTriple(callback.getExportXmlName(), OWLTags.abstrExtends, oldExtends.getUriModelComponentID()));
            }

            if (extendedElement is not null)
            {
                callback.publishElementAdded(extendedElement);
                extendedElement.register(callback);
                callback.addTriple(new PASSTriple(callback.getExportXmlName(), OWLTags.abstrExtends, extendedElement.getUriModelComponentID()));
            }
        }

        public void setExtendedElementID(string elementID)
        {
            this.extendedElementID = elementID;
        }
    }
}
