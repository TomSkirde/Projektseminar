using alps.net.api.ALPS;
using alps.net.api.FunctionalityCapsules;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a subject behavior
    /// </summary>
    public class SubjectBehavior : PASSProcessModelElement, ISubjectBehavior
    {
        /// <summary>
        /// Contains all components held by the subject behavior
        /// </summary>
        protected ICompDict<string, IBehaviorDescribingComponent> behaviorDescriptionComponents =
            new CompDict<string, IBehaviorDescribingComponent>();

        // The capsules are used to externalize functionality that is used across multiple classes redundandly
        protected readonly IImplementsFunctionalityCapsule<ISubjectBehavior> implCapsule;
        protected readonly IExtendsFunctionalityCapsule<ISubjectBehavior> extendsCapsule;

        protected IState initialStateOfBehavior;
        protected int priorityNumber;
        protected ISubject subj;
        protected IModelLayer layer;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SubjectBehavior";

        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SubjectBehavior();
        }

        protected SubjectBehavior()
        {
            implCapsule = new ImplementsFunctionalityCapsule<ISubjectBehavior>(this);
            extendsCapsule = new ExtendsFunctionalityCapsule<ISubjectBehavior>(this);
        }

        /// <summary>
        /// Creates a new SubjectBehavior from scratch
        /// </summary>
        public SubjectBehavior(IModelLayer layer, string labelForID = null, ISubject subject = null,
            ISet<IBehaviorDescribingComponent> behaviorDescribingComponents = null,
            IState initialStateOfBehavior = null,
            int priorityNumber = 0, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute)
        {
            implCapsule = new ImplementsFunctionalityCapsule<ISubjectBehavior>(this);
            extendsCapsule = new ExtendsFunctionalityCapsule<ISubjectBehavior>(this);
            setContainedBy(layer);
            setSubject(subject);
            setBehaviorDescribingComponents(behaviorDescribingComponents);
            setInitialState(initialStateOfBehavior);
            setPriorityNumber(priorityNumber);
        }


        public bool getContainedBy(out IModelLayer layer)
        {
            layer = this.layer;
            return layer != null;
        }

        public void setContainedBy(IModelLayer layer)
        {
            if (layer == null) return;
            if (layer.Equals(this.layer)) return;
            this.layer = layer;
            layer.addElement(this);
        }

        protected override IDictionary<string, IParseablePASSProcessModelElement> getDictionaryOfAllAvailableElements()
        {
            if (layer == null) return null;
            if (!layer.getContainedBy(out IPASSProcessModel model)) return null;
            IDictionary<string, IPASSProcessModelElement> allElements = model.getAllElements();
            IDictionary<string, IParseablePASSProcessModelElement> allParseableElements =
                new Dictionary<string, IParseablePASSProcessModelElement>();
            foreach (KeyValuePair<string, IPASSProcessModelElement> pair in allElements)
                if (pair.Value is IParseablePASSProcessModelElement parseable)
                    allParseableElements.Add(pair.Key, parseable);
            return allParseableElements;
        }


        // ######################## BehaviorDescribingComponent()s methods ########################


        public virtual bool addBehaviorDescribingComponent(IBehaviorDescribingComponent component)
        {
            if (component is null) { return false; }

            if (behaviorDescriptionComponents.TryAdd(component.getModelComponentID(), component))
            {
                publishElementAdded(component);
                if (component is IContainableElement<ISubjectBehavior> containable)
                    containable.setContainedBy(this);
                component.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains,
                    component.getUriModelComponentID()));
                return true;
            }

            return false;
        }


        public void setBehaviorDescribingComponents(ISet<IBehaviorDescribingComponent> components,
            int removeCascadeDepth = 0)
        {
            foreach (IBehaviorDescribingComponent component in this.getBehaviorDescribingComponents().Values)
            {
                removeBehaviorDescribingComponent(component.getModelComponentID(), removeCascadeDepth);
            }

            if (components is null) return;
            foreach (IBehaviorDescribingComponent component in components)
            {
                addBehaviorDescribingComponent(component);
            }
        }

        public virtual bool removeBehaviorDescribingComponent(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return false;
            if (behaviorDescriptionComponents.TryGetValue(id, out IBehaviorDescribingComponent component))
            {
                behaviorDescriptionComponents.Remove(id);
                component.unregister(this, removeCascadeDepth);
                if (layer != null)
                    if (layer.getContainedBy(out IPASSProcessModel model))
                        model.removeElement(id);

                foreach (IBehaviorDescribingComponent otherComponent in getBehaviorDescribingComponents().Values)
                {
                    otherComponent.updateRemoved(component, this, removeCascadeDepth);
                }

                if (component.Equals(initialStateOfBehavior))
                {
                    setInitialState(null);
                }

                if (component is IState state && state.isStateType(IState.StateType.EndState))
                    removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasEndState,
                        state.getUriModelComponentID()));
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains,
                    component.getUriModelComponentID()));
                return true;
            }

            return false;
        }

        public IDictionary<string, IBehaviorDescribingComponent> getBehaviorDescribingComponents()
        {
            return new Dictionary<string, IBehaviorDescribingComponent>(behaviorDescriptionComponents);
        }


        public void setInitialState(IState initialStateOfBehavior, int removeCascadeDepth = 0)
        {
            IState oldInitialState = this.initialStateOfBehavior;
            // Might set it to null
            this.initialStateOfBehavior = initialStateOfBehavior;

            if (oldInitialState != null)
            {
                if (oldInitialState.Equals(initialStateOfBehavior)) return;
                //removeBehaviorDescribingComponent(oldInitialState.getModelComponentID(), removeCascadeDepth);
                oldInitialState.removeStateType(IState.StateType.InitialStateOfBehavior);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasInitialState,
                    oldInitialState.getUriModelComponentID()));
            }

            this.initialStateOfBehavior = initialStateOfBehavior;

            if (initialStateOfBehavior != null)
            {
                addBehaviorDescribingComponent(initialStateOfBehavior);
                initialStateOfBehavior.setIsStateType(IState.StateType.InitialStateOfBehavior);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasInitialState,
                    initialStateOfBehavior.getUriModelComponentID()));
            }
        }

        public void setPriorityNumber(int positiveNumber)
        {
            if (positiveNumber == this.priorityNumber) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasPriorityNumber,
                this.priorityNumber.ToString(),
                new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            this.priorityNumber = (positiveNumber > 0) ? positiveNumber : 1;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasPriorityNumber, positiveNumber.ToString(),
                new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }


        public IState getInitialStateOfBehavior()
        {
            return initialStateOfBehavior;
        }

        public int getPriorityNumber()
        {
            return priorityNumber;
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(
            ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IBehaviorDescribingComponent component in getBehaviorDescribingComponents().Values)
                baseElements.Add(component);
            if (getInitialStateOfBehavior() != null) baseElements.Add(getInitialStateOfBehavior());
            if (specification == ConnectedElementsSetSpecification.ALL)
                if (getSubject() != null)
                    baseElements.Add(getSubject());
            return baseElements;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType,
            IParseablePASSProcessModelElement element)
        {
            if (implCapsule != null && implCapsule.parseAttribute(predicate, objectContent, lang, dataType, element))
                return true;
            else if (extendsCapsule != null &&
                     extendsCapsule.parseAttribute(predicate, objectContent, lang, dataType, element))
                return true;
            else if (element != null)
            {
                if (predicate.Contains(OWLTags.contains) && element is IBehaviorDescribingComponent component)
                {
                    addBehaviorDescribingComponent(component);
                    return true;
                }

                else if ((predicate.Contains(OWLTags.hasInitialStateOfBehavior) ||
                          predicate.Contains(OWLTags.hasInitialState)) && element is IState initialState)
                {
                    setInitialState(initialStateOfBehavior);
                    return true;
                }

                if (predicate.Contains(OWLTags.belongsTo) && element is IFullySpecifiedSubject subj)
                {
                    setSubject(subj);
                    return true;
                }
            }
            else if (predicate.Contains(OWLTags.hasPriorityNumber))
            {
                string prio = objectContent;
                setPriorityNumber(int.Parse(prio));
                return true;
            }

            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        protected override void successfullyParsedElement(IParseablePASSProcessModelElement parsedElement)
        {
            base.successfullyParsedElement(parsedElement);
            if (parsedElement is IContainableElement<ISubjectBehavior> containable)
                containable.setContainedBy(this);
        }


        public override void updateAdded(IPASSProcessModelElement update, IPASSProcessModelElement caller)
        {
            base.updateAdded(update, caller);
            if (update is IBehaviorDescribingComponent behaviorComp)
                addBehaviorDescribingComponent(behaviorComp);
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller,
            int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IBehaviorDescribingComponent component)
                {
                    removeBehaviorDescribingComponent(component.getModelComponentID(), removeCascadeDepth);
                }
            }
        }


        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (behaviorDescriptionComponents.ContainsKey(oldID))
            {
                IBehaviorDescribingComponent element = behaviorDescriptionComponents[oldID];
                behaviorDescriptionComponents.Remove(oldID);
                behaviorDescriptionComponents.Add(element.getModelComponentID(), element);
            }

            base.notifyModelComponentIDChanged(oldID, newID);
        }

        public virtual void setSubject(ISubject subj, int removeCascadeDepth = 0)
        {
            if (subj is IFullySpecifiedSubject fullySpecified)
            {
                ISubject oldSubj = this.subj;

                // Might set it to null
                this.subj = subj;

                if (oldSubj is not null)
                {
                    if (oldSubj.Equals(subj)) return;
                    if (oldSubj is IParseablePASSProcessModelElement parseable)
                        removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdBelongsTo,
                            parseable.getUriModelComponentID()));
                    if (oldSubj is IFullySpecifiedSubject oldFullySpecified)
                    {
                        oldFullySpecified.removeBehavior(getModelComponentID());
                    }
                }

                if (fullySpecified is not null)
                {
                    if (fullySpecified is IParseablePASSProcessModelElement parseable)
                        addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdBelongsTo,
                            parseable.getUriModelComponentID()));
                    fullySpecified.addBehavior(this);
                }
            }
        }


        public ISubject getSubject()
        {
            return subj;
        }


        // ##################### Capsule Methods (Calls only get forwarded) #####################

        public void setImplementedInterfacesIDReferences(ISet<string> implementedInterfacesIDs)
        {
            implCapsule.setImplementedInterfacesIDReferences(implementedInterfacesIDs);
        }

        public void addImplementedInterfaceIDReference(string implementedInterfaceID)
        {
            implCapsule.addImplementedInterfaceIDReference(implementedInterfaceID);
        }

        public void removeImplementedInterfacesIDReference(string implementedInterfaceID)
        {
            implCapsule.removeImplementedInterfacesIDReference(implementedInterfaceID);
        }

        public ISet<string> getImplementedInterfacesIDReferences()
        {
            return implCapsule.getImplementedInterfacesIDReferences();
        }

        public void setImplementedInterfaces(ISet<ISubjectBehavior> implementedInterface, int removeCascadeDepth = 0)
        {
            implCapsule.setImplementedInterfaces(implementedInterface, removeCascadeDepth);
        }

        public void addImplementedInterface(ISubjectBehavior implementedInterface)
        {
            implCapsule.addImplementedInterface(implementedInterface);
        }

        public void removeImplementedInterfaces(string id, int removeCascadeDepth = 0)
        {
            implCapsule.removeImplementedInterfaces(id, removeCascadeDepth);
        }

        public IDictionary<string, ISubjectBehavior> getImplementedInterfaces()
        {
            return implCapsule.getImplementedInterfaces();
        }

        public void setExtendedElement(ISubjectBehavior element)
        {
            extendsCapsule.setExtendedElement(element);
        }

        public void setExtendedElementID(string elementID)
        {
            extendsCapsule.setExtendedElementID(elementID);
        }

        public ISubjectBehavior getExtendedElement()
        {
            return extendsCapsule.getExtendedElement();
        }

        public string getExtendedElementID()
        {
            return extendsCapsule.getExtendedElementID();
        }

        public bool isExtension()
        {
            return extendsCapsule.isExtension();
        }

        public void removeFromContainer()
        {
            if (layer != null)
                layer.removeContainedElement(getModelComponentID());
            layer = null;
        }

        // ###########################################################
    }
}