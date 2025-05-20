using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a send transition
    /// </summary>
    public class SendTransition : CommunicationTransition, ISendTransition
    {
        protected readonly ICompDict<string, IDataMappingLocalToOutgoing> dataMappingsLocalToOutgoing = new CompDict<string, IDataMappingLocalToOutgoing>();

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SendTransition";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SendTransition();
        }

        protected SendTransition() { }

        public SendTransition(IState sourceState, IState targetState, string labelForID = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, ISet<IDataMappingLocalToOutgoing> dataMappingLocalToOutgoing = null, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null) : base(sourceState, targetState, labelForID, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute)
        {
            setDataMappingFunctionsLocalToOutgoing(dataMappingLocalToOutgoing);
        }

        public SendTransition(ISubjectBehavior behavior, string label = null,
            IState sourceState = null, IState targetState = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, ISet<IDataMappingLocalToOutgoing> dataMappingLocalToOutgoing = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, label, sourceState, targetState, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute)
        {
            setDataMappingFunctionsLocalToOutgoing(dataMappingLocalToOutgoing);
        }




        public void addDataMappingFunction(IDataMappingLocalToOutgoing dataMappingLocalToOutgoing)
        {
            if (dataMappingLocalToOutgoing is null) { return; }
            if (dataMappingsLocalToOutgoing.TryAdd(dataMappingLocalToOutgoing.getModelComponentID(), dataMappingLocalToOutgoing))
            {
                publishElementAdded(dataMappingLocalToOutgoing);
                dataMappingLocalToOutgoing.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataMappingFunction, dataMappingLocalToOutgoing.getModelComponentID()));
            }
        }

        public void removeDataMappingFunction(string mappingID, int removeCascadeDepth = 0)
        {
            if (mappingID is null) return;
            if (dataMappingsLocalToOutgoing.TryGetValue(mappingID, out IDataMappingLocalToOutgoing mapping))
            {
                dataMappingsLocalToOutgoing.Remove(mappingID);
                mapping.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataMappingFunction, mappingID));
            }
        }

        public void setDataMappingFunctionsLocalToOutgoing(ISet<IDataMappingLocalToOutgoing> dataMappingsLocalToOutgoing, int removeCascadeDepth = 0)
        {
            foreach (IDataMappingLocalToOutgoing mapping in getDataMappingFunctions().Values)
            {
                removeDataMappingFunction(mapping.getModelComponentID(), removeCascadeDepth);
            }
            if (dataMappingsLocalToOutgoing is null) return;
            foreach (IDataMappingLocalToOutgoing mapping in dataMappingsLocalToOutgoing)
            {
                addDataMappingFunction(mapping);
            }
        }

        public IDictionary<string, IDataMappingLocalToOutgoing> getDataMappingFunctions()
        {
            return new Dictionary<string, IDataMappingLocalToOutgoing>(dataMappingsLocalToOutgoing);
        }

        public new ISendTransitionCondition getTransitionCondition()
        {
            return (ISendTransitionCondition)transitionCondition;
        }

        public new void setTransitionCondition(ITransitionCondition condition, int removeCascadeDepth = 0)
        {
            if (condition is ISendTransitionCondition receiveCondition) base.setTransitionCondition(receiveCondition, removeCascadeDepth);
            else base.setTransitionCondition(null, removeCascadeDepth);
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.hasDataMappingFunction) && element is IDataMappingLocalToOutgoing dataMapping)
                {
                    addDataMappingFunction(dataMapping);
                    return true;
                }

                if (predicate.Contains(OWLTags.hasTransitionCondition) && element is ITransitionCondition sendCondition)
                {
                    setTransitionCondition(sendCondition);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override void setSourceState(IState state, int removeCascadeDepth = 0)
        {
            if (state is ISendState)
            {
                base.setSourceState(state, removeCascadeDepth);
            }
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IDataMappingLocalToOutgoing mapping in getDataMappingFunctions().Values)
                baseElements.Add(mapping);
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IDataMappingLocalToOutgoing mapping) removeDataMappingFunction(mapping.getModelComponentID(), removeCascadeDepth);
            }
        }


        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (dataMappingsLocalToOutgoing.ContainsKey(oldID))
            {
                IDataMappingLocalToOutgoing element = dataMappingsLocalToOutgoing[oldID];
                dataMappingsLocalToOutgoing.Remove(oldID);
                dataMappingsLocalToOutgoing.Add(element.getModelComponentID(), element);
            }

            base.notifyModelComponentIDChanged(oldID, newID);
        }
    }
}