using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// From abstract pass ont: <br></br>
    /// StateGroups/GroupStates and/or Checklist are model objects that can "contain" other SBD-Model elements as their Sub-Shapes in order to group them.
    /// </summary>
    public class GroupState : State, IGroupState
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string CLASS_NAME = "GroupState";
        protected ICompDict<string, IBehaviorDescribingComponent> groupedComponents = new CompDict<string, IBehaviorDescribingComponent>();

        public override string getClassName()
        {
            return CLASS_NAME;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new GroupState();
        }

        protected GroupState() { }

        /// <summary>
        /// Constructor that creates a new fully specified instance of the group state class
        /// </summary>
        /// <param name="behavior"></param>
        /// <param name="labelForId">a string describing this element which is used to generate the unique model component id</param>
        /// <param name="guardBehavior"></param>
        /// <param name="functionSpecification"></param>
        /// <param name="incomingTransition"></param>
        /// <param name="outgoingTransition"></param>
        /// <param name="comment"></param>
        /// <param name="additionalLabel"></param>
        /// <param name="additionalAttribute"></param>
        public GroupState(ISubjectBehavior behavior, string labelForId = null, IGuardBehavior guardBehavior = null,
            IFunctionSpecification functionSpecification = null, ISet<ITransition> incomingTransition = null, ISet<ITransition> outgoingTransition = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForId, guardBehavior, functionSpecification, incomingTransition, outgoingTransition, comment, additionalLabel, additionalAttribute)
        { }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }


        public virtual bool addGroupedComponent(IBehaviorDescribingComponent component)
        {
            if (component is null) { return false; }

            if (!groupedComponents.TryAdd(component.getModelComponentID(), component)) return false;

            publishElementAdded(component);
            component.register(this);
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, component.getUriModelComponentID()));
            return true;
        }


        public void setGroupedComponents(ISet<IBehaviorDescribingComponent> components, int removeCascadeDepth = 0)
        {
            foreach (IBehaviorDescribingComponent component in this.getGroupedComponents().Values)
            {
                removeGroupedComponent(component.getModelComponentID(), removeCascadeDepth);
            }
            if (components is null) return;
            foreach (IBehaviorDescribingComponent component in components)
            {
                addGroupedComponent(component);
            }
        }

        public virtual bool removeGroupedComponent(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return false;

            if (!groupedComponents.TryGetValue(id, out IBehaviorDescribingComponent component)) return false;

            groupedComponents.Remove(id);
            component.unregister(this, removeCascadeDepth);
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, component.getUriModelComponentID()));
            return true;
        }
        public IDictionary<string, IBehaviorDescribingComponent> getGroupedComponents()
        {
            return new Dictionary<string, IBehaviorDescribingComponent>(groupedComponents);
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.contains) && element is IBehaviorDescribingComponent component)
                {
                    addGroupedComponent(component);
                    return true;

                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }
    }
}
