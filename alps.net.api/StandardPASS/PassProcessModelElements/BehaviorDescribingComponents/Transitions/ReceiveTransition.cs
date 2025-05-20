using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a receive transition
    /// </summary>
    public class ReceiveTransition : CommunicationTransition, IReceiveTransition
    {
        protected readonly ICompDict<string, IDataMappingIncomingToLocal> dataMappingsIncomingToLocal =
            new CompDict<string, IDataMappingIncomingToLocal>();

        protected int priorityNumber = 1;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "ReceiveTransition";


        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ReceiveTransition();
        }

        protected ReceiveTransition() { }

        public ReceiveTransition(IState sourceState, IState targetState, string labelForID = null,
            ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard,
            ISet<IDataMappingIncomingToLocal> dataMappingIncomingToLocal = null,
            int priorityNumber = 0, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(sourceState, targetState, labelForID, transitionCondition, transitionType, comment, additionalLabel,
                additionalAttribute)
        {
            setDataMappingFunctionsIncomingToLocal(dataMappingIncomingToLocal);
            setPriorityNumber(priorityNumber);
        }

        public ReceiveTransition(ISubjectBehavior behavior, string label = null,
            IState sourceState = null, IState targetState = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard,
            ISet<IDataMappingIncomingToLocal> dataMappingIncomingToLocal = null,
            int priorityNumber = 0, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, label, sourceState, targetState, transitionCondition, transitionType, comment,
                additionalLabel, additionalAttribute)
        {
            setDataMappingFunctionsIncomingToLocal(dataMappingIncomingToLocal);
            setPriorityNumber(priorityNumber);
        }

        public new void setSourceState(IState state, int removeCascadeDepth = 0)
        {
            if (state is IReceiveState)
            {
                base.setSourceState(state, removeCascadeDepth);
            }
        }

        public void addDataMappingFunction(IDataMappingIncomingToLocal dataMappingIncomingToLocal)
        {
            if (dataMappingIncomingToLocal is null) { return; }

            if (dataMappingsIncomingToLocal.TryAdd(dataMappingIncomingToLocal.getModelComponentID(),
                    dataMappingIncomingToLocal))
            {
                publishElementAdded(dataMappingIncomingToLocal);
                dataMappingIncomingToLocal.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataMappingFunction,
                    dataMappingIncomingToLocal.getModelComponentID()));
            }
        }

        public void removeDataMappingFunction(string mappingID, int removeCascadeDepth = 0)
        {
            if (mappingID is null) return;
            if (dataMappingsIncomingToLocal.TryGetValue(mappingID, out IDataMappingIncomingToLocal mapping))
            {
                dataMappingsIncomingToLocal.Remove(mappingID);
                mapping.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataMappingFunction, mappingID));
            }
        }

        public void setDataMappingFunctionsIncomingToLocal(
            ISet<IDataMappingIncomingToLocal> dataMappingsIncomingToLocal, int removeCascadeDepth = 0)
        {
            foreach (IDataMappingIncomingToLocal mapping in getDataMappingFunctions().Values)
            {
                removeDataMappingFunction(mapping.getModelComponentID(), removeCascadeDepth);
            }

            if (dataMappingsIncomingToLocal is null) return;
            foreach (IDataMappingIncomingToLocal mapping in dataMappingsIncomingToLocal)
            {
                addDataMappingFunction(mapping);
            }
        }

        public IDictionary<string, IDataMappingIncomingToLocal> getDataMappingFunctions()
        {
            return new Dictionary<string, IDataMappingIncomingToLocal>(dataMappingsIncomingToLocal);
        }


        public void setPriorityNumber(int positiveInteger)
        {
            if (positiveInteger == this.priorityNumber) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasPriorityNumber,
                this.priorityNumber.ToString(),
                new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            priorityNumber = (positiveInteger > 0) ? positiveInteger : 1;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasPriorityNumber, priorityNumber.ToString(),
                new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }


        public int getPriorityNumber()
        {
            return priorityNumber;
        }


        public new IReceiveTransitionCondition getTransitionCondition()
        {
            return (IReceiveTransitionCondition)transitionCondition;
        }

        public override void setTransitionCondition(ITransitionCondition condition, int removeCascadeDepth = 0)
        {
            if (condition is IReceiveTransitionCondition receiveCondition)
                base.setTransitionCondition(receiveCondition);
            else base.setTransitionCondition(null);
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType,
            IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasPriorityNumber))
            {
                string prio = objectContent;
                setPriorityNumber(int.Parse(prio));
                return true;
            }
            else if (element != null)
            {
                if (predicate.Contains(OWLTags.hasDataMappingFunction) &&
                    element is IDataMappingIncomingToLocal dataMapping)
                {
                    addDataMappingFunction(dataMapping);
                    return true;
                }

                if (predicate.Contains(OWLTags.hasTransitionCondition) && element is ITransitionCondition receiveCond)
                {
                    setTransitionCondition(receiveCond);
                    return true;
                }
            }

            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(
            ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IDataMappingIncomingToLocal mapping in getDataMappingFunctions().Values)
                baseElements.Add(mapping);
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller,
            int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IDataMappingIncomingToLocal mapping)
                    removeDataMappingFunction(mapping.getModelComponentID(), removeCascadeDepth);
            }
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (dataMappingsIncomingToLocal.ContainsKey(oldID))
            {
                IDataMappingIncomingToLocal element = dataMappingsIncomingToLocal[oldID];
                dataMappingsIncomingToLocal.Remove(oldID);
                dataMappingsIncomingToLocal.Add(element.getModelComponentID(), element);
            }

            base.notifyModelComponentIDChanged(oldID, newID);
        }
    }
}