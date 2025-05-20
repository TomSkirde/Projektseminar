using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.util;
using System.Collections.Generic;
using alps.net.api.src;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an action. This is a construct used in the ontology, but is only implemented here to guarantee a correct standard.
    /// A user should not create own actions, they will be created automatically when creating a state.
    /// They are only used for export.
    /// However, when imported, the correct actions should be loaded and parsed correctly.
    /// This class ensures that.
    /// </summary>
    public class Action : BehaviorDescribingComponent, IAction
    {
        protected IState state;
        protected ICompDict<string, ITransition> transitions = new CompDict<string, ITransition>();

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "Action";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new Action();
        }

        protected Action() { }
        /// <summary>
        /// Constructor that creates a new fully specified instance of the action class
        /// </summary>
        public Action(IState state, string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base((state != null) ? (state.getContainedBy(out ISubjectBehavior behavior) ? behavior : null) : null,
                labelForID, comment, additionalLabel, additionalAttribute)
        {
            setContainsState(state);
        }

        /// <summary>
        /// Sets the corresponding state.
        /// Not public (explanation in class xml)
        /// </summary>
        /// <param name="state">the state</param>
        /// <param name="parsed">should express whether this method was called while parsing or not</param>
        protected void setContainsState(IState state, bool parsed = false)
        {
            IState oldState = this.state;
            // Might set it to null
            this.state = state;


            if (oldState != null)
            {
                if (oldState.Equals(state)) return;
                oldState.unregister(this);
                //oldState.replaceGeneratedActionWithParsed(null);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, oldState.getUriModelComponentID()));
            }

            if (!(state is null))
            {
                publishElementAdded(state);
                state.register(this);

                // Get all outgoing transitions from the state
                updateContainedTransitions();

                // Only if this action was parsed (and not created automatically),
                // overwrite the (previously automatically created) action of the state
                //if (state.getAction() is null)
                //if (parsed)
                //state.replaceGeneratedActionWithParsed(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, state.getUriModelComponentID()));
            }
        }

        public IState getState()
        {
            return state;
        }

        /// <summary>
        /// Sets the corresponding transitions.
        /// Not public (explanation in class xml)
        /// </summary>
        protected void updateContainedTransitions()
        {
            foreach (ITransition transition in getContainedTransitions().Values)
            {
                removeContainedTransition(transition.getModelComponentID());
            }
            if (state is null) return;
            foreach (ITransition transition in state.getOutgoingTransitions().Values)
            {
                addContainsTransition(transition);
            }
        }

        /// <summary>
        /// Adds a corresponding transition.
        /// Not public (explanation in class xml)
        /// </summary>
        protected void addContainsTransition(ITransition containedTransition)
        {
            if (containedTransition is null) { return; }
            if (transitions.TryAdd(containedTransition.getModelComponentID(), containedTransition))
            {
                publishElementAdded(containedTransition);
                containedTransition.register(this);
                if (containedTransition.getSourceState() != null && state != null)
                {
                    if (containedTransition.getSourceState().Equals(state)) state.addOutgoingTransition(containedTransition);
                }
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, containedTransition.getUriModelComponentID()));
            }
        }

        public IDictionary<string, ITransition> getContainedTransitions()
        {
            return new Dictionary<string, ITransition>(transitions);
        }

        /// <summary>
        /// Removes a contained transition.
        /// Not public (explanation in class xml)
        /// </summary>
        /// <param name="id">the id of the transition</param>
        protected void removeContainedTransition(string id)
        {
            if (id is null) return;
            if (transitions.TryGetValue(id, out ITransition transition))
            {
                transitions.Remove(id);
                transition.unregister(this);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, transition.getUriModelComponentID()));
            }
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getState() != null)
                baseElements.Add(getState());

            if (specification == ConnectedElementsSetSpecification.TO_ADD || specification == ConnectedElementsSetSpecification.ALL)
                foreach (ITransition transition in getContainedTransitions().Values)
                    baseElements.Add(transition);
            return baseElements;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.contains))
                {
                    if (element is IState state)
                    {
                        setContainsState(state, true);
                        return true;
                    }

                    else if (element is ITransition transition)
                    {
                        addContainsTransition(transition);
                        return true;
                    }
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (transitions.ContainsKey(oldID))
            {
                ITransition element = transitions[oldID];
                transitions.Remove(oldID);
                transitions.Add(element.getModelComponentID(), element);
            }

            base.notifyModelComponentIDChanged(oldID, newID);
        }


        public override void updateAdded(IPASSProcessModelElement update, IPASSProcessModelElement caller)
        {
            base.updateAdded(update, caller);
            if (update is null) return;

            // Only relevant for parsing:
            // When a registered transition calls a change -> has a new state that might have been null before (not parsed)
            // -> check if the state matches our belonging state and add the transition to the state
            if (caller is ITransition transition && getContainedTransitions().ContainsKey(transition.getModelComponentID()))
            {
                IState localState = getState();
                if (update.Equals(localState) && localState != null)
                {
                    if (transition.getSourceState() != null && transition.getSourceState().Equals(localState)) localState.addOutgoingTransition(transition);
                }
            }
            // When a registered state calls a change -> might have a new transition
            // -> re-set our transitions
            if (caller is IState state && getState() != null && getState().Equals(state))
            {
                if (update is ITransition)
                {
                    updateContainedTransitions();
                }
            }
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update.Equals(getState()))
                    removeFromEverything();

                // When a registered state calls a change -> might have deleted a transition
                // -> re-set our transitions
                if (caller is IState state && getState() != null && getState().Equals(state))
                {
                    if (update is ITransition)
                    {
                        updateContainedTransitions();
                    }
                }
            }

        }
    }
}