using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// Defines an interface for a model layer
    /// </summary>
    public interface IModelLayer : IALPSModelElement, IPrioritizableElement, IContainableElement<IPASSProcessModel>,
        IImplementingElement<IModelLayer>, IExtendingElement<IModelLayer>, IAbstractElement
    {
        /// <summary>
        /// Represents the type of the layer
        /// </summary>
        public enum LayerType
        {
            STANDARD,
            BASE,
            EXTENSION,
            MACRO,
            GUARD
        }
        

        /// <summary>
        /// Sets the layertype for the layer
        /// </summary>
        /// <param name="layerType"></param>
        void setLayerType(LayerType layerType);

        /// <summary>
        /// Returns the layer type for the current layer
        /// </summary>
        /// <returns></returns>
        LayerType getLayerType();
        

        /// <summary>
        /// Returns all elements contained inside the layer.
        /// The key is the ModelComponentID to each value item.
        /// </summary>
        /// <returns>A dictionary containing all elements</returns>
        IDictionary<string, IPASSProcessModelElement> getElements();

        /// <summary>
        /// Adds an element to the layer
        /// </summary>
        /// <param name="value"></param>
        void addElement(IPASSProcessModelElement value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IPASSProcessModelElement getElement(string id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extendedLayer"></param>
        /// <param name="removeCascadeDepth"></param>
        public void setExtendedLayer(IModelLayer extendedLayer, int removeCascadeDepth = 0);


        /// <summary>
        /// Returns a fully specified subject depending on its position
        /// (inside the list of all fully specified subjects in the layer)
        /// </summary>
        /// <param name="numberOfElement">the position</param>
        /// <returns></returns>
        IFullySpecifiedSubject getFullySpecifiedSubject(int numberOfElement);


        /// <summary>
        /// Returns an interface subject depending on its position
        /// (inside the list of interface subjects in the layer)
        /// </summary>
        /// <param name="numberOfElement">the position</param>
        /// <returns></returns>
        IInterfaceSubject getInterfaceSubject(int numberOfElement);

        /// <summary>
        /// Returns a multi subject depending on its position
        /// (inside the list of interface subjects in the layer)
        /// </summary>
        /// <param name="numberOfElement">the position</param>
        /// <returns></returns>
        IMultiSubject getMultiSubject(int numberOfElement);


        /// <summary>
        /// Returns a single subject depending on its position
        /// (inside the list of single subjects in the layer)
        /// </summary>
        /// <param name="numberOfElement">the position</param>
        /// <returns></returns>
        ISingleSubject getSingleSubject(int numberOfElement);


        /// <summary>
        /// Returns a message exchange depending on its position
        /// (inside the list of message exchanges in the layer)
        /// </summary>
        /// <param name="numberOfElement">the position</param>
        /// <returns></returns>
        IMessageExchange getMessageExchange(int numberOfElement);


        /// <summary>
        /// Returns an input pool constraint depending on its position
        /// (inside the list of input pool constraints in the layer)
        /// </summary>
        /// <param name="numberOfElement">the position</param>
        /// <returns>The object</returns>
        IInputPoolConstraint getInputPoolConstraint(int numberOfElement);


        /// <summary>
        /// Returns a message sender type constraint depending on its position
        /// (inside the list of message sender type constraints in the layer)
        /// </summary>
        /// <param name="numberOfElement">the position</param>
        /// <returns>The object</returns>
        IMessageSenderTypeConstraint getMessageSenderTypeConstraint(int numberOfElement);


        /// <summary>
        /// Returns a message type constraint depending on its position
        /// (inside the list of message type constraints in the layer)
        /// </summary>
        /// <param name="numberOfElement">the position</param>
        /// <returns>The object</returns>
        IMessageTypeConstraint getMessageTypeConstraint(int numberOfElement);


        /// <summary>
        /// Returns a sender type constraint depending on its position
        /// (inside the list of sender type constraints in the layer)
        /// </summary>
        /// <param name="numberOfElement">the position</param>
        /// <returns>The object</returns>
        ISenderTypeConstraint getSenderTypeConstraint(int numberOfElement);


        /// <summary>
        /// Returns an input pool constraint handling strategy depending on its position
        /// (inside the list of input pool constraint handling strategies in the layer)
        /// </summary>
        /// <param name="numberOfElement">the position</param>
        /// <returns>The object</returns>
        IInputPoolConstraintHandlingStrategy getInputPoolConstraintHandlingStrategy(int numberOfElement);

        /// <summary>
        /// Returns a message exchange list depending on its position
        /// (inside the list of message exchange lists in the layer)
        /// </summary>
        /// <param name="numberOfElement">the position</param>
        /// <returns>The object</returns>
        IMessageExchangeList getMessageExchangeList(int numberOfElement);

        /// <summary>
        /// Returns a message specification depending on its position
        /// (inside the list of message specifications in the layer)
        /// </summary>
        /// <param name="numberOfElement">the position</param>
        /// <returns>The object</returns>
        IMessageSpecification getMessageSpecification(int numberOfElement);

        /// <summary>
        /// Deletes an element depending on its id, if it is contained inside the layer
        /// </summary>
        /// <param name="modelComponentID">the id of the element</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        bool removeContainedElement(string modelComponentID, int removeCascadeDepth = 0);

    }
}
