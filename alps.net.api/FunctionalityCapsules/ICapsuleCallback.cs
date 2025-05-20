using alps.net.api.parsing;
using alps.net.api.StandardPASS;

namespace alps.net.api.FunctionalityCapsules
{
    public interface ICapsuleCallback :IParseablePASSProcessModelElement
    {
        /// <summary>
        /// Publishes that an element has been added to this component
        /// </summary>
        /// <param name="element">the added element</param>
        void publishElementAdded(IPASSProcessModelElement element);

        /// <summary>
        /// Publishes that an element has been removed from this component
        /// </summary>
        /// <param name="element">the removed element</param>
        /// <param name="removeCascadeDepth">An integer that specifies the depth of a cascading delete for connected elements (to the deleted element)
        /// 0 deletes only the given element, 1 the adjacent elements etc.</param>
        void publishElementRemoved(IPASSProcessModelElement element, int removeCascadeDepth = 0);
    }
}
