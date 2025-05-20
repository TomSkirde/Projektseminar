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
    /// Class that represents an GuardBehavior
    /// According to standard pass 1.1.0
    /// </summary>

    public class GuardBehavior : SubjectBehavior, IGuardBehavior
    {
        protected ICompDict<string, ISubjectBehavior> subjectBehaviors = new CompDict<string, ISubjectBehavior>();
        protected ICompDict<string, IState> guardedStates = new CompDict<string, IState>();
        protected IGuardsFunctionalityCapsule<IState> stateGuardCapsule;
        protected IGuardsFunctionalityCapsule<ISubjectBehavior> behaviorGuardCapsule;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "GuardBehavior";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new GuardBehavior();
        }

        protected GuardBehavior() { }
        public GuardBehavior(IModelLayer layer, string labelForID = null, ISubject subject = null, ISet<IBehaviorDescribingComponent> components = null,
            ISet<ISubjectBehavior> guardedBehaviors = null, ISet<IState> guardedStates = null, IState initialStateOfBehavior = null, int priorityNumber = 0, string comment = null,
            string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, subject, components, initialStateOfBehavior, priorityNumber, comment, additionalLabel, additionalAttribute)
        {
            setGuardedBehaviors(guardedBehaviors);
            setGuardedStates(guardedStates);
        }

        public void setGuardedBehaviors(ISet<ISubjectBehavior> behaviors, int removeCascadeDepth = 0)
        {
            foreach (ISubjectBehavior behavior in getGuardedBehaviors().Values)
            {
                removeGuardedBehavior(behavior.getModelComponentID(), removeCascadeDepth);
            }
            if (behaviors is null) return;
            foreach (ISubjectBehavior behavior in behaviors)
            {
                addGuardedBehavior(behavior);
            }
        }

        public void addGuardedBehavior(ISubjectBehavior behavior)
        {
            if (behavior is null) { return; }
            if (subjectBehaviors.TryAdd(behavior.getModelComponentID(), behavior))
            {
                publishElementAdded(behavior);
                behavior.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdGuardsBehavior, behavior.getUriModelComponentID()));
            }
        }

        public IDictionary<string, ISubjectBehavior> getGuardedBehaviors()
        {
            return new Dictionary<string, ISubjectBehavior>(subjectBehaviors);
        }

        public void removeGuardedBehavior(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (subjectBehaviors.TryGetValue(id, out ISubjectBehavior behavior))
            {
                subjectBehaviors.Remove(id);
                behavior.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdGuardsBehavior, behavior.getUriModelComponentID()));
            }
        }


        public void setGuardedStates(ISet<IState> guardedStates, int removeCascadeDepth = 0)
        {
            foreach (IState state in getGuardedStates().Values)
            {
                removeGuardedState(state.getModelComponentID(), removeCascadeDepth);
            }
            if (guardedStates is null) return;
            foreach (IState state in guardedStates)
            {
                addGuardedState(state);
            }
        }

        public void addGuardedState(IState state)
        {
            if (state is null) { return; }
            if (guardedStates.TryAdd(state.getModelComponentID(), state))
            {
                publishElementAdded(state);
                state.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdGuardsState, state.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IState> getGuardedStates()
        {
            return new Dictionary<string, IState>(guardedStates);
        }

        public void removeGuardedState(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (guardedStates.TryGetValue(id, out IState state))
            {
                guardedStates.Remove(id);
                state.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdGuardsState, state.getUriModelComponentID()));
            }
        }





        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.guardsBehavior) && element is ISubjectBehavior behavior)
                {
                    addGuardedBehavior(behavior);
                    return true;
                }

                else if (predicate.Contains(OWLTags.guardsState) && element is IState state)
                {
                    addGuardedState(state);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (ISubjectBehavior behavior in getGuardedBehaviors().Values) baseElements.Add(behavior);
            foreach (IState state in getGuardedStates().Values) baseElements.Add(state);
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IState state)
                    // Try to remove the state
                    removeGuardedState(state.getModelComponentID(), removeCascadeDepth);
                else if (update is ISubjectBehavior behavior)
                    // Try to remove the behavior
                    removeGuardedBehavior(behavior.getModelComponentID(), removeCascadeDepth);
            }
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (subjectBehaviors.ContainsKey(oldID))
            {
                ISubjectBehavior element = subjectBehaviors[oldID];
                subjectBehaviors.Remove(oldID);
                subjectBehaviors.Add(element.getModelComponentID(), element);
            }
            if (guardedStates.ContainsKey(oldID))
            {
                IState element = guardedStates[oldID];
                guardedStates.Remove(oldID);
                guardedStates.Add(element.getModelComponentID(), element);
            }
            base.notifyModelComponentIDChanged(oldID, newID);
        }

    }
}