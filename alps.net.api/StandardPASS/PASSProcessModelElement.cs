using alps.net.api.FunctionalityCapsules;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Root class for the inheritance graphs. Represents a PASS process model element
    /// </summary>
    public class PASSProcessModelElement : ICapsuleCallback
    {

        /// <summary>
        /// This list contains the additional attributes as snd entry and the types for each of the additional attributes as first
        /// </summary>
        /// 

        protected string exportSubjectNodeName = null;

        public static readonly int CANNOT_PARSE = -1;

        protected readonly List<IValueChangedObserver<IPASSProcessModelElement>> observerList = new();
        protected IList<IPASSTriple> additionalIncompleteTriples = new List<IPASSTriple>();


        protected IUniqueList<IStringWithExtra> modelComponentLabels = new UniqueList<IStringWithExtra>();
        protected IUniqueList<IStringWithExtra> comments = new UniqueList<IStringWithExtra>();
        protected ICompDict<string, IPASSProcessModelElement> additionalElements = new CompDict<string, IPASSProcessModelElement>();
        protected Guid guid = Guid.NewGuid();
        protected string modelComponentID = "";
        protected IPASSGraph exportGraph;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "PASSProcessModelElement";

        public static CultureInfo customCulture = new("en-US");
        static PASSProcessModelElement()
        {
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
        }


        public virtual string getClassName()
        {
            return className;
        }

        protected virtual string getExportTag()
        {
            return OWLTags.std;
        }


        /// <summary>
        /// Constructor that creates a fully specified instance of the PASS Process Model Element class
        /// </summary>
        /// <param name="labelForID">a string describing the element used to generate the model id</param>
        /// <param name="comment">the comment</param>
        /// <param name="additionalAttributes">list of additional attributes</param>
        public PASSProcessModelElement(string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttributes = null)
        {
            if (labelForID is null or "")
                createUniqueModelComponentID(getClassName(), false);
            else
                createUniqueModelComponentID(labelForID, additionalLabel is null or "");

            if (additionalLabel != null && !additionalLabel.Equals(""))
                addModelComponentLabel(additionalLabel);
            if (comment != null && !comment.Equals(""))
                addComment(comment);

            if (additionalAttributes != null)
                addTriples(additionalAttributes);

            addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, OWLTags.stdNamedIndividual));
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + getClassName()));

        }


        /// <summary>
        /// Adds an incomplete Triple to the element that will either be parsed right away or delayed,
        /// depending on whether there is a graph with a base uri available or not.
        /// </summary>
        /// <param name="triple">the triple that is being saved</param>
        public void addTriple(IPASSTriple triple)
        {
            // TODO might check if subject is set correctly

            if (containsTriple(triple)) return;

            additionalIncompleteTriples.Add(triple);

            parseAttribute(triple, out _);
            exportGraph?.addTriple(triple);
        }

        /// <summary>
        /// Adds a  list ofIncomplete Triple to the element that will either be parsed right away, or delayed
        /// (depending on whether there is a graph available or not)
        /// </summary>
        /// <param name="triple">the triple that is being saved</param>
        public void addTriples(IList<IPASSTriple> triples)
        {
            if (triples == null) return;

            foreach (IPASSTriple triple in triples)
                addTriple(triple);
        }


        /// <summary>
        /// Adds a complete triple to the element.
        /// If the element contains an Incomplete Triple containing the same information, it will be deleted.
        /// The content of the triple will be parsed if possible.
        /// </summary>
        /// <param name="triple"></param>
        /*public void addTriple(Triple triple)
        {
            // Do not add the triple if it is already contained
            if (triple is null)
                return;
            if (additionalAttributeTriples.Contains(triple)) return;

            // If a graph with a base URI is available, the base uri of the Triple might clash with the base uri defined by the graph.
            // Convert the triple to an incomplete triple that is parsed back to a Triple using the graphs uri
            IIncompleteTriple incTriple = new IncompleteTriple(getExportXmlName(), triple);
            if (exportGraph is not null)
                addTriple(incTriple);

            // If no graph is available
            else
            {
                // Remove all incomplete triples that are encoding the same information
                if (getIncompleteTriple(incTriple) != null)
                    removeTriple(incTriple);

                // Parse the information encoded by the triple
                additionalAttributeTriples.Add(triple);
                parseAttribute(incTriple, out _);
            }
        }*/

        /// <summary>
        /// Tries to parse an incomplete triple and add it as complete triple
        /// this is only possible if a graph is given. A valid base uri must not be provided,
        /// otherwise an example base uri is used.
        /// </summary>
        /// <param name="triple">The incomplete triple</param>
        /*protected void completeIncompleteTriple(IIncompleteTriple triple)
        {
            if (exportGraph is null) return;
            INode subjectNode;


            // Generate subject node uri from modelComponentID
            if (exportSubjectNodeName == null || exportSubjectNodeName.Equals(""))
            {
                subjectNode = exportGraph.createUriNode(StaticFunctions.addGenericBaseURI(getModelComponentID()));
            }
            // Generate it from preset name
            else
            {

                if (exportSubjectNodeName.Equals(getBaseURI()))
                    subjectNode = exportGraph.createUriNode(new Uri(getBaseURI()));
                else
                    subjectNode = exportGraph.createUriNode(StaticFunctions.addGenericBaseURI(exportSubjectNodeName));

            }


            // other nodes are evaluated from the provided incomplete triple
            Triple completeTriple = triple.getRealTriple(exportGraph, subjectNode);

            //additionalAttributeTriples.Add(completeTriple);
            exportGraph.addTriple(triple);
            //additionalIncompleteTriples.Remove(triple);
        }*/

        public string getExportXmlName()
        {
            if (exportSubjectNodeName is not null && !exportSubjectNodeName.Equals(""))
            {
                return StaticFunctions.addGenericBaseURI(exportSubjectNodeName);
            }

            return StaticFunctions.addGenericBaseURI(getModelComponentID());
        }

        public IList<IPASSTriple> getIncompleteTriples()
        {
            return new List<IPASSTriple>(additionalIncompleteTriples);
        }

        /// <summary>
        /// Removes a triple from either the complete or incomplete triples.
        /// </summary>
        /// <param name="incTriple">An incomplete triple coding the value that should be deleted.
        /// The deleted object must not be incomplete, but can as well be a complete triple</param>
        /// <returns></returns>
        public bool removeTriple(IPASSTriple incTriple)
        {

            IPASSTriple foundIncompleteTriple = getIncompleteTriple(incTriple);
            if (foundIncompleteTriple != null)
            {

                if (exportGraph != null) exportGraph.removeTriple(foundIncompleteTriple);
                return additionalIncompleteTriples.Remove(foundIncompleteTriple);

            }
            return false;
        }

        /// <summary>
        /// Determines wheter there is a triple (complete or incomplete), holding the same data as the given incomplete triple
        /// </summary>
        /// <param name="incTriple">An incomplete triple coding the value that should be found.
        /// The found object must not be incomplete, but can as well be a complete triple</param>
        /// <returns></returns>
        public bool containsTriple(IPASSTriple incTriple)
        {
            return (getIncompleteTriple(incTriple) != null);
        }


        /// <summary>
        /// Returns an incomplete triple which contains the same data as the given incomplete triple.
        /// </summary>
        /// <param name="searchedTriple">An incomplete triple providing the data to be searched for</param>
        /// <returns></returns>
        protected IPASSTriple getIncompleteTriple(IPASSTriple searchedTriple)
        {
            string predicateToSearchFor = NodeHelper.cutURI(searchedTriple.getPredicate());
            string objectContentToSearchFor = NodeHelper.cutURI(searchedTriple.getObject());
            foreach (IPASSTriple triple in getIncompleteTriples())
            {
                string predicateToMatch = NodeHelper.cutURI(triple.getPredicate());
                string objectContentToMatch = NodeHelper.cutURI(triple.getObject());
                if (predicateToSearchFor.Equals(predicateToMatch) && objectContentToSearchFor.Equals(objectContentToMatch))
                    return triple;
            }
            return null;
        }

        /// <summary>
        /// Replaces an old triple with a new one.
        /// </summary>
        /// <param name="oldTriple">An incomplete triple holding the data to find the triple to be replaced</param>
        /// <param name="newTriple">An incomplete triple holding the data to replace the old triple</param>
        public void replaceTriple(IPASSTriple oldTriple, IPASSTriple newTriple)
        {
            if (oldTriple.Equals(newTriple)) return;
            if (oldTriple != null && containsTriple(oldTriple))
            {
                if (removeTriple(oldTriple))
                    addTriple(newTriple);
            }
            else
            {
                addTriple(newTriple);
            }
        }

        public string getBaseURI()
        {
            return ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER;
        }

        // ############################ ModelComponentID ############################

        public string getModelComponentID()
        {
            return modelComponentID;
        }

        public string getUriModelComponentID()
        {
            return StaticFunctions.addGenericBaseURI(getModelComponentID());
        }

        public void setModelComponentID(string id)
        {
            if (id is null || id.Equals("")) return;
            string oldID = modelComponentID;
            if (!id.Equals(oldID))
            {
                string modifiedID = id.Trim().Replace(" ", "_");

                IPASSTriple oldTriple = new PASSTriple(getExportXmlName(),
                    OWLTags.stdHasModelComponentID, getModelComponentID(),
                    new PASSTriple.LiteralDataType(OWLTags.xsdDataTypeString));

                modelComponentID = modifiedID;

                IPASSTriple newTriple = new PASSTriple(getExportXmlName(),
                    OWLTags.stdHasModelComponentID, modifiedID,
                    new PASSTriple.LiteralDataType(OWLTags.xsdDataTypeString));

                replaceTriple(oldTriple, newTriple);

                invalidateTriplesContainingString(oldID);
                if (exportGraph != null) exportGraph.modelComponentIDChanged(oldID, id);
                publishNewModelComponentID(oldID);
            }
        }


        /// <summary>
        /// This method is used to replace invalid triples with incomplete representations.
        /// A triple might be invalid if the base URI of the model graph or the ModelComponentID of the current element has changed.
        /// On such a change, the nodes inside the graph must be updated.
        /// Therefore, the triples are stored as incomplete triples, which are later parsed back to Triples once the new id or URI is known.
        /// </summary>
        /// <param name="containedString"></param>
        protected void invalidateTriplesContainingString(string containedString)
        {
            IList<IPASSTriple> triplesToBeChanged = new List<IPASSTriple>();
            IPASSTriple owlNamedIndivTriple = null;
            foreach (IPASSTriple triple in getIncompleteTriples())
            {
                if (triple.ToString().Contains(containedString))
                {
                    IPASSTriple newTriple = new PASSTriple(triple.getSubject(), triple.getPredicate(), triple.getObjectWithExtra());
                    if (newTriple.getPredicate().Contains(OWLTags.rdfTypeFull) &&
                        newTriple.getObject().Contains(OWLTags.owlNamedIndividualFull))
                        owlNamedIndivTriple = newTriple;
                    else
                        triplesToBeChanged.Add(newTriple);
                }
            }




            foreach (IPASSTriple triple in triplesToBeChanged)
            {
                removeTriple(triple);
            }

            // Needed as the underlying graph behaves weirdly and does not type the element with NamedIndividual
            if (owlNamedIndivTriple != null)
            {
                removeTriple(owlNamedIndivTriple);
            }
            //

            foreach (IPASSTriple triple in triplesToBeChanged)
            {
                addTriple(triple);
            }

            // Needed as the underlying graph behaves weirdly and does not type the element with NamedIndividual
            if (owlNamedIndivTriple != null)
            {
                addTriple(owlNamedIndivTriple);
            }
        }


        public string createUniqueModelComponentID(string labelForID = null, bool addLabel = true)
        {
            if (labelForID is null || labelForID.Equals(""))
            {
                setModelComponentID(guid.ToString());
            }
            else
            {
                setModelComponentID(labelForID + ("-") + guid.ToString());
                if (addLabel) addModelComponentLabel(labelForID);
            }
            return getModelComponentID();
        }

        // ############################ ModelComponent labels ############################

        public void setModelComponentLabels(IList<string> modelComponentLabels)
        {
            clearModelComponentLabels();
            foreach (string label in modelComponentLabels)
            {
                addModelComponentLabel(label);
            }
        }

        public void addModelComponentLabel(string modelComponentLabel)
        {
            IStringWithExtra extraString = new LanguageSpecificString(modelComponentLabel);
            modelComponentLabels.Add(extraString);
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasModelComponentLabel, extraString));

        }

        public void addModelComponentLabel(LanguageSpecificString modelComponentLabel)
        {
            foreach (IStringWithExtra label in modelComponentLabels)
            {
                if (label.Equals(modelComponentLabel)) return;
            }
            if (modelComponentLabels.Add(modelComponentLabel))
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasModelComponentLabel, modelComponentLabel));
        }

        public IList<string> getModelComponentLabelsAsStrings(bool addLanguageAttribute = false)
        {
            return modelComponentLabels.Select(label => addLanguageAttribute ? label.ToString() : label.getContent()).ToList();
        }

        public IList<IStringWithExtra> getModelComponentLabels()
        {
            return new List<IStringWithExtra>(modelComponentLabels);
        }

        public void clearModelComponentLabels()
        {
            // Cannot use clear() on the collection, since the triples must be removed as well
            for (int i = 0; i < modelComponentLabels.Count; i++)
            {
                removeModelComponentLabel(i);
            }
        }

        public void removeModelComponentLabel(LanguageSpecificString label)
        {
            if (label is null) return;
            if (modelComponentLabels.Contains(label))
            {
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasModelComponentLabel, label));
                modelComponentLabels.Remove(label);
            }
        }

        public void removeModelComponentLabel(int num)
        {
            if (num < 0 || num > modelComponentLabels.Count) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasModelComponentLabel, modelComponentLabels[num]));
            modelComponentLabels.RemoveAt(num);
        }

        // ############################ Comments ############################

        public void addComment(LanguageSpecificString comment)
        {
            if (comment is null || comment.ToString()!.Equals("")) return;
            comments.Add(comment);
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfsComment, comment));
        }

        public void addComment(string comment)
        {
            if (comment is null or "") return;
            IStringWithExtra extraComment = new LanguageSpecificString(comment);
            if (comments.Add(extraComment))
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfsComment, extraComment));
        }

        public IList<string> getComments()
        {
            return comments.Select(langString => langString.ToString()).ToList();
        }


        public void clearComments()
        {
            // Cannot use clear() on the collection, since the triples must be removed as well
            for (int i = 0; i < comments.Count; i++)
            {
                removeComment(i);
            }
        }

        public void removeComment(LanguageSpecificString comment)
        {
            if (comment is null) return;
            if (comments.Contains(comment))
            {
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasModelComponentLabel, comment.ToString()));
                comments.Remove(comment);
            }
        }

        public void removeComment(int num)
        {
            if (num < 0 || num > comments.Count) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfsComment, comments[num]));
            comments.RemoveAt(num);
        }


        // This is needed for the <completeObject> method not to loop infinitely
        protected bool parsingStarted = false;

        public virtual void completeObject(ref IDictionary<string, IParseablePASSProcessModelElement> allElements)
        {
            if (parsingStarted) return;
            parsingStarted = true;
            IList<IParseablePASSProcessModelElement> successfullyParsedElements = new List<IParseablePASSProcessModelElement>();
            foreach (IPASSTriple triple in getIncompleteTriples())
            {
                parseAttribute(triple, allElements, out IParseablePASSProcessModelElement parsedElement);
                if (parsedElement != null)
                {
                    successfullyParsedElements.Add(parsedElement);
                }
            }

            foreach (IParseablePASSProcessModelElement element in successfullyParsedElements)
            {
                element.completeObject(ref allElements);
            }
        }



        /*protected bool parseAttribute(Triple triple, out IParseablePASSProcessModelElement parsedElement)
        {

            // Calling parsing method
            // If attribute contains a reference to a PassProcessModelElement, pass this to the method
            IDictionary<string, IParseablePASSProcessModelElement> allElements = getDictionaryOfAllAvailableElements();
            return parseAttribute(triple, allElements, out parsedElement);
        }*/

        protected bool parseAttribute(IPASSTriple triple, out IParseablePASSProcessModelElement parsedElement)
        {

            // Calling parsing method
            // If attribute contains a reference to a PassProcessModelElement, pass this to the method
            IDictionary<string, IParseablePASSProcessModelElement> allElements = getDictionaryOfAllAvailableElements();
            return parseAttribute(triple, allElements, out parsedElement);
        }

        /*        protected bool parseAttribute(Triple triple, IDictionary<string, IParseablePASSProcessModelElement> allElements, out IParseablePASSProcessModelElement parsedElement)
                {

                    // Calling parsing method
                    // If attribute contains a reference to a PassProcessModelElement, pass this to the method
                    parsedElement = null;
                    setExportXMLName(NodeHelper.getNodeContent(triple.Subject));
                    string predicateContent = NodeHelper.getNodeContent(triple.Predicate);
                    string objectContent = NodeHelper.getNodeContent(triple.Object);
                    string lang = NodeHelper.getLangIfContained(triple.Object);
                    string dataType = NodeHelper.getDataTypeIfContained(triple.Object);

                    // This object content might be the name of another element (in the same model) which is referenced by the triple
                    string objectContentWithoutUri = StaticFunctions.removeBaseUri(objectContent);


                    if (allElements != null && allElements.ContainsKey(objectContentWithoutUri))
                    {
                        if (!parseAttribute(predicateContent, objectContentWithoutUri, lang, dataType, allElements[objectContentWithoutUri]) ||
                            allElements[objectContentWithoutUri].Equals(this))
                        {
                            return false;
                        }

                        parsedElement = allElements[objectContentWithoutUri];
                        successfullyParsedElement(parsedElement);
                        return true;
                    }
                    return parseAttribute(predicateContent, objectContent, lang, dataType, null);

                }*/

        protected bool parseAttribute(IPASSTriple triple, IDictionary<string, IParseablePASSProcessModelElement> allElements,
            out IParseablePASSProcessModelElement parsedElement)
        {
            // Calling parsing method
            // If attribute contains a reference to a PassProcessModelElement, pass this to the method
            parsedElement = null;
            setExportXMLName(triple.getSubject());
            string predicateContent = triple.getPredicate();
            var objWithExtra = triple.getObjectWithExtra();
            string objectContent = objWithExtra.getContent();
            string lang = objWithExtra is LanguageSpecificString ? objWithExtra.getExtra() : "";
            string dataType = objWithExtra is DataTypeString ? objWithExtra.getExtra() : "";

            // This object content might be the name of another element (in the same model) which is referenced by the triple
            string objectContentWithoutUri = StaticFunctions.removeAllUriPrefix(objectContent);


            if (allElements != null && allElements.ContainsKey(objectContentWithoutUri))
            {
                if (!parseAttribute(predicateContent, objectContentWithoutUri, lang, dataType, allElements[objectContentWithoutUri]) ||
                    allElements[objectContentWithoutUri].Equals(this))
                {
                    return false;
                }

                parsedElement = allElements[objectContentWithoutUri];
                successfullyParsedElement(parsedElement);
                return true;
            }
            return parseAttribute(predicateContent, objectContent, lang, dataType, null);
        }

        protected virtual void successfullyParsedElement(IParseablePASSProcessModelElement parsedElement) { return; }

        /// <summary>
        /// Provides access to the dictionary of all available elements inside the current model.
        /// </summary>
        /// <returns>A dictionary containing model component ids as keys and elements as values</returns>
        protected virtual IDictionary<string, IParseablePASSProcessModelElement> getDictionaryOfAllAvailableElements()
        {
            return null;
        }

        /// <summary>
        /// Gets called while parsing a triple from a set of triples where this element is subject.
        /// The predicate and objectContent are derived directly from the triple,
        /// lang and dataType might be null (they will never both be NonNull at the same time)
        /// If the object specifies an uri to another element and the collection of all available elements contains this element,
        /// the element is passed as well
        /// </summary>
        /// <param name="predicate">the predicate contained by the triple</param>
        /// <param name="objectContent">the content of the object contained by the triple</param>
        /// <param name="lang">the lang attribute of the object if one was specified</param>
        /// <param name="dataType">the datatype attribute of the object if one was specified</param>
        /// <param name="element">the element the objectContent points to (if it does and the element exists)</param>
        /// <returns></returns>
        protected virtual bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.ToLower().Contains(OWLTags.rdfsComment))
            {
                addComment(new LanguageSpecificString(objectContent, lang));
                return true;
            }

            else if (predicate.Contains(OWLTags.hasModelComponentLabel))
            {
                addModelComponentLabel(new LanguageSpecificString(objectContent, lang));
                return true;
            }

            else if (predicate.Contains(OWLTags.hasModelComponentID))
            {
                setModelComponentID(objectContent.Split('#')[objectContent.Split('#').Length - 1]);
                return true;
            }
            if (element is not null)
            {
                addElementWithUnspecifiedRelation(element);
                return true;
            }

            return false;
        }





        public virtual int canParse(string className)
        {
            if (className.ToLower().Equals(getClassName().ToLower()))
            {
                return getClassName().Length;
            }
            return -1;
        }

        // Observer/Publisher Methods

        public virtual bool register(IValueChangedObserver<IPASSProcessModelElement> observer)
        {
            if (observer != null && !observerList.Contains(observer))
            {
                observerList.Add(observer);
                // Might not call this every time
                informObserverAboutConnectedObjects(observer, ObserverInformType.ADDED);
                return true;
            }
            return false;
        }

        public virtual bool unregister(IValueChangedObserver<IPASSProcessModelElement> observer, int removeCascadeDepth = 0)
        {
            if (observer != null && observerList.Contains(observer))
            {
                observerList.Remove(observer);
                // Might not call this every time
                //if (removeCascadeDepth > 0)
                informObserverAboutConnectedObjects(observer, ObserverInformType.REMOVED, removeCascadeDepth);
                return true;
            }
            return false;

        }

        /// <summary>
        /// Publishes that an element has been added to this component
        /// </summary>
        /// <param name="element">the added element</param>
        public void publishElementAdded(IPASSProcessModelElement element)
        {
            IList<IValueChangedObserver<IPASSProcessModelElement>> localObserver = new List<IValueChangedObserver<IPASSProcessModelElement>>(observerList);
            foreach (IValueChangedObserver<IPASSProcessModelElement> observer in localObserver)
            {
                observer.updateAdded(element, this);
            }
        }

        /// <summary>
        /// Publishes that an element has been removed from this component
        /// </summary>
        /// <param name="element">the removed element</param>
        /// <param name="removeCascadeDepth">An integer that specifies the depth of a cascading delete for connected elements (to the deleted element)
        /// 0 deletes only the given element, 1 the adjacent elements etc.</param>
        public void publishElementRemoved(IPASSProcessModelElement element, int removeCascadeDepth = 0)
        {
            foreach (IValueChangedObserver<IPASSProcessModelElement> observer in new List<IValueChangedObserver<IPASSProcessModelElement>>(observerList))
            {
                observer.updateRemoved(element, this, removeCascadeDepth);
            }
        }

        protected void publishNewModelComponentID(string oldID)
        {
            IList<IValueChangedObserver<IPASSProcessModelElement>> localObserver = new List<IValueChangedObserver<IPASSProcessModelElement>>(observerList);
            foreach (IValueChangedObserver<IPASSProcessModelElement> observer in localObserver)
            {
                observer.notifyModelComponentIDChanged(oldID, this.getModelComponentID());
            }
        }

        public virtual void updateAdded(IPASSProcessModelElement update, IPASSProcessModelElement caller)
        {
            return;
        }

        public virtual void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            return;
        }

        protected enum ObserverInformType { ADDED, REMOVED }

        /// <summary>
        /// This enum specifies a group of elements that should be returned on function call
        /// </summary>
        public enum ConnectedElementsSetSpecification
        {
            /// <summary>
            /// All elements that are somehow connected to the class
            /// </summary>
            ALL,
            /// <summary>
            /// All elements that should be added to other components via cascading add
            /// </summary>
            TO_ADD,
            /// <summary>
            /// All elements that should be deleted from other components via cascading delete
            /// </summary>
            TO_REMOVE_DIRECTLY_ADJACENT,

            TO_REMOVE_ADJACENT_AND_MORE,

            TO_ALWAYS_REMOVE
        }

        protected void informObserverAboutConnectedObjects(IValueChangedObserver<IPASSProcessModelElement> observer, ObserverInformType informType, int removeCascadeDepth = 0)
        {
            if (informType == ObserverInformType.ADDED)
            {
                foreach (IPASSProcessModelElement element in getAllConnectedElements(ConnectedElementsSetSpecification.TO_ADD))
                    observer.updateAdded(element, this);
            }
            if (informType == ObserverInformType.REMOVED)
            {
                ConnectedElementsSetSpecification connectedSpecification;
                if (removeCascadeDepth > 1) connectedSpecification = ConnectedElementsSetSpecification.TO_REMOVE_ADJACENT_AND_MORE;
                else if (removeCascadeDepth == 1) connectedSpecification = ConnectedElementsSetSpecification.TO_REMOVE_DIRECTLY_ADJACENT;
                else connectedSpecification = ConnectedElementsSetSpecification.TO_ALWAYS_REMOVE;
                foreach (IPASSProcessModelElement element in getAllConnectedElements(connectedSpecification))
                    observer.updateRemoved(element, this, (removeCascadeDepth > 0) ? (removeCascadeDepth - 1) : 0);
            }
        }


        public void removeFromEverything(int removeCascadeDepth = 0)
        {
            publishElementRemoved(this, removeCascadeDepth);
        }



        public override bool Equals(object obj)
        {
            if (obj is IPASSProcessModelElement element)
            {
                return element.getModelComponentID().Equals(getModelComponentID());
            }
            return false;
        }

        public override int GetHashCode()
        {
            return modelComponentID.GetHashCode();
        }


        public virtual ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification) // TODO add depth everywhere
        {
            return new HashSet<IPASSProcessModelElement>();
        }

        public void setExportGraph(ref IPASSGraph graph)
        {

            if (this.exportGraph != null)
            {
                if (this.exportGraph.Equals(graph)) return;
                this.exportGraph.unregister(this);
            }
            this.exportGraph = graph;
            if (graph is null) return;
            graph.register(this);
            string baseURI = getBaseURI();
            if (baseURI != null && !baseURI.Equals(""))
                invalidateTriplesContainingString("");
            foreach (IPASSTriple triple in getIncompleteTriples())
            {
                graph.addTriple(triple);
            }
        }

        public virtual void notifyModelComponentIDChanged(string oldID, string newID)
        {
            foreach (IPASSTriple t in getIncompleteTriples())
            {
                if (t.ToString().Contains(oldID))
                {
                    IPASSTriple newIncompleteTriple;
                    string predicate = t.getPredicate();
                    if (t.getObjectWithExtra() != null)
                    {
                        IStringWithExtra extra = t.getObjectWithExtra();
                        extra.setContent(extra.getContent().Replace(oldID, newID));
                        newIncompleteTriple = new PASSTriple(getExportXmlName(), predicate, extra);
                    }
                    else
                    {
                        string objectStr = t.getObject().Replace(oldID, newID);
                        newIncompleteTriple = new PASSTriple(getExportXmlName(), predicate, objectStr);
                    }
                    replaceTriple(t, newIncompleteTriple);
                }
            }
            foreach (IPASSTriple t in getIncompleteTriples())
            {
                if (t.ToString().Contains(oldID))
                {
                    IPASSTriple oldIncompleteTriple = new PASSTriple(t.getSubject(), t.getPredicate(), t.getObjectWithExtra());
                    IPASSTriple newIncompleteTriple;
                    string predicate = oldIncompleteTriple.getPredicate();
                    if (oldIncompleteTriple.getObjectWithExtra() != null)
                    {
                        IStringWithExtra extra = oldIncompleteTriple.getObjectWithExtra();
                        extra.setContent(extra.getContent().Replace(oldID, newID));
                        newIncompleteTriple = new PASSTriple(getExportXmlName(), predicate, extra);
                    }
                    else
                    {
                        string objectStr = oldIncompleteTriple.getObject().Replace(oldID, newID);
                        newIncompleteTriple = new PASSTriple(getExportXmlName(), predicate, objectStr);
                    }
                    replaceTriple(oldIncompleteTriple, newIncompleteTriple);
                }
            }
        }

        public virtual IParseablePASSProcessModelElement getParsedInstance()
        {
            return new PASSProcessModelElement();
        }

        public void addElementWithUnspecifiedRelation(IPASSProcessModelElement element)
        {
            if (element is null) { return; }
            if (additionalElements.TryAdd(element.getModelComponentID(), element))
            {
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, element.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IPASSProcessModelElement> getElementsWithUnspecifiedRelation()
        {
            return new Dictionary<string, IPASSProcessModelElement>(additionalElements);
        }

        public void setElementsWithUnspecifiedRelation(ISet<IPASSProcessModelElement> elements)
        {
            foreach (IPASSProcessModelElement element in getElementsWithUnspecifiedRelation().Values)
            {
                removeElementWithUnspecifiedRelation(element.getModelComponentID());
            }
            if (elements is null) return;
            foreach (IPASSProcessModelElement element in elements)
            {
                addElementWithUnspecifiedRelation(element);
            }
        }

        public void removeElementWithUnspecifiedRelation(string id)
        {
            if (id is null) return;
            if (additionalElements.TryGetValue(id, out IPASSProcessModelElement element))
            {
                additionalElements.Remove(id);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, element.getUriModelComponentID()));
            }
        }

        public string getSubjectName()
        {
            return getModelComponentID();
        }

        public void notifyTriple(IPASSTriple triple)
        {
            addTriple(triple);
        }

        protected void setExportXMLName(string xmlTag)
        {
            if (exportSubjectNodeName is not (null or "")) return;

            exportSubjectNodeName = StaticFunctions.addGenericBaseURI(StaticFunctions.removeAllUriPrefix(xmlTag));

        }

        protected PASSProcessModelElement() { }
    }
}
