using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// This class specifies the most general interface for elements contained inside a PASS model
    /// <br></br>
    /// <br></br>
    /// From PASS Doc: <i>All sub-class/instances of subclasses of a PASS-ProcessModelElement can be considered elements of PASS Process Models. 
    /// Every element/sub-class of SimplePASSElement is also a child of PASSProcessModelElement.
    /// This is simply a surrogate class to group all simple elements together and differ them from StandardPASS</i>
    /// </summary>
    public interface IPASSProcessModelElement : IValueChangedPublisher<IPASSProcessModelElement>, IValueChangedObserver<IPASSProcessModelElement>
    {
        /// <summary>
        /// In some cases the full model component id (with uri) is needed.
        /// If no graph exists, the baseUri is just a tag added in front acting as placeholder for the real uri.
        /// </summary>
        /// <returns>the id with the uri in front</returns>
        string getUriModelComponentID();

        /// <summary>
        /// Returns the unique id the identifies the current element
        /// </summary>
        /// <returns></returns>
        string getModelComponentID();

        /// <summary>
        /// Attempts to remove the elment from every part that contains this element (model, layer, behavior...)
        /// </summary>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeFromEverything(int removeCascadeDepth = 0);

        /// <summary>
        /// <para>Sets the unique id for the current model.
        /// Sets the id to the exact passed value.</para>
        /// <para>The user must assure that the id is unique inside the current model,
        /// otherwise exceptions might be thrown while using the model.</para>
        /// <para>To safely create a component with a unique id, use <see cref="createUniqueModelComponentID(string)"/></para>
        /// </summary>
        /// <param name="id">the id that will be set as modelComponentID</param>
        void setModelComponentID(string id);


        /// <summary>
        /// <para>Creates a unique id for the element using a specified label.
        /// For this, a <see cref="Guid"/> is used.</para>
        /// <para>The specified label is only used to provide simpler reading.
        /// Leaving the label blank or not passing anything causes the element to use guid-only as id.</para>
        /// </summary>
        /// <param name="label">the label that is used together with a guid to generate a valid and unique identifier for the element</param>
        /// <param name="addLabel">If set to true, a modelComponentLabel is also added for the given label.
        /// If false, only the id will be created.</param>
        /// <returns>the created id for the current element</returns>
        string createUniqueModelComponentID(string labelForID = null, bool addLabel = true);



        /// <summary>
        /// Method that sets the model component label list, overriding the previous content
        /// </summary>
        /// <param name="modelComponentLabel">the model component label list</param>
        void setModelComponentLabels(IList<string> modelComponentLabel);

        /// <summary>
        /// Method that sets the model component label attribute
        /// </summary>
        /// <param name="modelComponentLabel">the model component label</param>
        void addModelComponentLabel(string modelComponentLabel);

        /// <summary>
        /// Method that sets the model component label attribute
        /// </summary>
        /// <param name="modelComponentLabel">the model component label</param>
        void addModelComponentLabel(LanguageSpecificString modelComponentLabel);

        /// <summary>
        /// Method that returns the model component labels as strings.
        /// Additional language info can be added to each string by setting <paramref name="addLanguageAttribute"/> true
        /// </summary>
        /// <param name="addLanguageAttribute">If set to true, information about the language the label is written in is included in the each label</param>
        /// <returns>The model component label list</returns>
        IList<string> getModelComponentLabelsAsStrings(bool addLanguageAttribute = false);

        /// <summary>
        /// Method that returns the model component labels as objects with additional language info
        /// </summary>
        /// <returns></returns>
        IList<IStringWithExtra> getModelComponentLabels();

        /// <summary>
        /// Clears the list of model component labels
        /// </summary>
        void clearModelComponentLabels();

        /// <summary>
        /// Removes a label that has the specified string as content
        /// </summary>
        /// <param name="label"></param>
        void removeModelComponentLabel(LanguageSpecificString label);

        public void removeModelComponentLabel(int num);

        /// <summary>
        /// Method that adds a comment attribute
        /// </summary>
        /// <param name="comment">the comment</param>
        void addComment(string comment);

        void addComment(LanguageSpecificString comment);

        /// <summary>
        /// Method that returns the comment attribute
        /// </summary>
        /// <returns>The comment attribute</returns>
        IList<string> getComments();

        /// <summary>
        /// Clears the list of comments
        /// </summary>
        void clearComments();

        void removeComment(LanguageSpecificString comment);

        void removeComment(int num);

        /// <summary>
        /// Adds an element that is in some undefined relation to the current element.
        /// This method is only for adding additional elements/information to another element that cannot be added in another way (using specified methods etc.)
        /// </summary>
        /// <param name="element">the new element</param>
        public void addElementWithUnspecifiedRelation(IPASSProcessModelElement element);

        /// <returns>A set of elements that are in some undefined relation to the current element</returns>
        public IDictionary<string, IPASSProcessModelElement> getElementsWithUnspecifiedRelation();

        /// <summary>
        /// Overrides the elements that are in some undefined relation to the current element.
        /// This method is only for adding additional elements/information to another element that cannot be added in another way (using specified methods etc.)
        /// </summary>
        /// <param name="behaviors">the new elements</param>
        public void setElementsWithUnspecifiedRelation(ISet<IPASSProcessModelElement> elements);

        /// <summary>
        /// Removes an elements that is in some undefined relation to the current element.
        /// </summary>
        /// <param name="id">the id of the element</param>
        public void removeElementWithUnspecifiedRelation(string id);
    }
}
