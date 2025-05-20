using alps.net.api.util;
using System.Collections.Generic;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a Choice Segment Path
    /// </summary>
    public class ChoiceSegmentPath : State, IChoiceSegmentPath
    {
        protected IState endState;
        protected IState initialState;
        protected bool isOptionalToStart = false;
        protected bool isOptionalToEnd = false;
        protected readonly ICompDict<string, IState> containedStates = new CompDict<string, IState>();
        protected IChoiceSegment segment;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "ChoiceSegmentPath";

        /// <summary>
        /// Constructor that creates a new empty instance of the Choice Segment Path class
        /// </summary>
        public ChoiceSegmentPath(ISubjectBehavior behavior) : base(behavior) { }

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ChoiceSegmentPath();
        }

        protected ChoiceSegmentPath() { }
        public ChoiceSegmentPath(ISubjectBehavior behavior, string labelForID = null, IGuardBehavior guardBehavior = null,
            IFunctionSpecification functionSpecification = null, ISet<ITransition> incomingTransition = null, ISet<ITransition> outgoingTransition = null,
            ISet<IState> containedStates = null, IState endState = null,
            IInitialStateOfChoiceSegmentPath initialStateOfChoiceSegmentPath = null, bool isOptionalToEndChoiceSegmentPath = false,
            bool isOptionalToStartChoiceSegmentPath = false, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForID, guardBehavior, functionSpecification, incomingTransition, outgoingTransition, comment, additionalLabel, additionalAttribute)
        {
            setEndState(endState);
            setInitialState(initialStateOfChoiceSegmentPath);
            setContainedStates(containedStates);
            setIsOptionalToEndChoiceSegmentPath(isOptionalToEndChoiceSegmentPath);
            setIsOptionalToStartChoiceSegmentPath(isOptionalToStartChoiceSegmentPath);
        }

        public ChoiceSegmentPath(IChoiceSegment choiceSegment, string labelForID = null, IGuardBehavior guardBehavior = null,
            IFunctionSpecification functionSpecification = null, ISet<ITransition> incomingTransition = null, ISet<ITransition> outgoingTransition = null,
            ISet<IState> containedStates = null, IState endState = null,
            IInitialStateOfChoiceSegmentPath initialStateOfChoiceSegmentPath = null, bool isOptionalToEndChoiceSegmentPath = false,
            bool isOptionalToStartChoiceSegmentPath = false, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(choiceSegment is null ? null : choiceSegment.getContainedBy(out ISubjectBehavior behavior) ? behavior : null, labelForID, guardBehavior,
                  functionSpecification, incomingTransition, outgoingTransition, comment, additionalLabel, additionalAttribute)
        {
            setEndState(endState);
            setInitialState(initialStateOfChoiceSegmentPath);
            setContainedStates(containedStates);
            setIsOptionalToEndChoiceSegmentPath(isOptionalToEndChoiceSegmentPath);
            setIsOptionalToStartChoiceSegmentPath(isOptionalToStartChoiceSegmentPath);
            setContainedBy(choiceSegment);
        }

        public void setContainedStates(ISet<IState> containedStates, int removeCascadeDepth = 0)
        {
            foreach (IState state in getContainedStates().Values)
            {
                removeContainedState(state.getModelComponentID(), removeCascadeDepth);
            }
            if (containedStates is null) return;
            foreach (IState state in containedStates)
            {
                addContainedState(state);
            }
        }

        public void addContainedState(IState containedState)
        {
            if (containedState is null) { return; }
            if (containedStates.TryAdd(containedState.getModelComponentID(), containedState))
            {
                publishElementAdded(containedState);
                containedState.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, containedState.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IState> getContainedStates()
        {
            return new Dictionary<string, IState>(containedStates);
        }

        public void removeContainedState(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (containedStates.TryGetValue(id, out IState state))
            {
                containedStates.Remove(id);
                state.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, state.getUriModelComponentID()));
                if (state.Equals(getInitialState())) setInitialState(null, removeCascadeDepth);
                if (state.Equals(getEndState())) setEndState(null, removeCascadeDepth);
            }
        }

        public void setEndState(IState state, int removeCascadeDepth = 0)
        {
            IState oldState = this.endState;
            // Might set it to null
            endState = state;

            if (oldState != null)
            {
                if (oldState.Equals(state)) return;
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasEndState, oldState.getUriModelComponentID()));
                removeContainedState(oldState.getModelComponentID(), removeCascadeDepth);
            }

            if (!(state is null))
            {
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasEndState, state.getUriModelComponentID()));
                addContainedState(state);
            }
        }


        public IState getEndState()
        {
            return endState;
        }


        public void setInitialState(IState state, int removeCascadeDepth = 0)
        {
            IState oldState = this.initialState;
            // Might set it to null
            initialState = state;

            if (oldState != null)
            {
                if (oldState.Equals(state)) return;
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasInitialState, oldState.getUriModelComponentID()));
                removeContainedState(oldState.getModelComponentID(), removeCascadeDepth);
            }

            if (!(state is null))
            {
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasInitialState, state.getUriModelComponentID()));
                addContainedState(state);
            }
        }


        public IState getInitialState()
        {
            return initialState;
        }



        public void setIsOptionalToEndChoiceSegmentPath(bool endChoice)
        {
            isOptionalToEnd = endChoice;
        }


        public bool getIsOptionalToEndChoiceSegmentPath()
        {
            return isOptionalToEnd;
        }


        public void setIsOptionalToStartChoiceSegmentPath(bool startChoice)
        {
            isOptionalToStart = startChoice;
        }


        public bool getIsOptionalToStartChoiceSegmentPath()
        {
            return isOptionalToStart;
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (element is IState state)
                {

                    if (predicate.Contains(OWLTags.hasEndState))
                    {
                        setEndState(state);
                        return true;
                    }
                    else if (predicate.Contains(OWLTags.hasInitialState))
                    {
                        setInitialState(state);
                        return true;
                    }
                    else if (predicate.Contains(OWLTags.contains))
                    {
                        addContainedState(state);
                        return true;
                    }

                }
                else if (predicate.Contains(OWLTags.belongsTo) && element is IChoiceSegment segment)
                {
                    setContainedBy(segment);
                    return true;
                }
            }
            if (predicate.Contains(OWLTags.isOptionalToEndChoiceSegmentPath))
            {
                bool isOptional = (objectContent.ToLower().Contains("true")) ? true : false;
                setIsOptionalToEndChoiceSegmentPath(isOptional);
            }
            if (predicate.Contains(OWLTags.isOptionalToStartChoiceSegmentPath))
            {
                bool isOptional = (objectContent.ToLower().Contains("true")) ? true : false;
                setIsOptionalToEndChoiceSegmentPath(isOptional);
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IState component in getContainedStates().Values)
                baseElements.Add(component);
            if (getContainedBy(out IChoiceSegment segment))
                baseElements.Add(segment);
            if (getEndState() != null)
                baseElements.Add(getEndState());
            if (getInitialState() != null)
                baseElements.Add(getInitialState());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IState state)
                {
                    if (state.Equals(getEndState()))
                        setEndState(null, removeCascadeDepth);
                    if (state.Equals(getInitialState()))
                        setInitialState(null, removeCascadeDepth);
                    else removeContainedState(state.getModelComponentID(), removeCascadeDepth);
                }
                getContainedBy(out IChoiceSegment localSegment);
                if (update is IChoiceSegment segment && segment.Equals(localSegment))
                    setContainedBy(null);
            }
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (containedStates.ContainsKey(oldID))
            {
                IState element = containedStates[oldID];
                containedStates.Remove(oldID);
                containedStates.Add(element.getModelComponentID(), element);
            }
            base.notifyModelComponentIDChanged(oldID, newID);
        }

        public void setContainedBy(IChoiceSegment container)
        {
            IChoiceSegment oldSegment = this.segment;
            // Might set it to null
            this.segment = container;

            if (oldSegment != null)
            {
                if (oldSegment.Equals(container)) return;
                oldSegment.unregister(this);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdBelongsTo, oldSegment.getUriModelComponentID()));
            }

            if (!(container is null))
            {
                publishElementAdded(container);
                container.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdBelongsTo, container.getUriModelComponentID()));
            }
        }


        public bool getContainedBy(out IChoiceSegment choiceSegment)
        {
            choiceSegment = segment;
            return segment != null;
        }

        public void removeFromContainer()
        {
            if (segment != null)
                segment.removeChoiceSegmentPath(getModelComponentID());
            segment = null;
        }
    }
}
