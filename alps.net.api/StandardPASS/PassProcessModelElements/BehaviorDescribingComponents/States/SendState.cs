using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using Serilog;
using System.Collections.Generic;
using static alps.net.api.StandardPASS.IState;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a send state
    /// </summary>
    public class SendState : StandardPASSState, ISendState
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SendState";
        protected string exportTag = OWLTags.std;
        protected string exportClassname = className;
        protected ISiSiTimeDistribution sisiExecutionDuration;
        protected double sisiCostPerExecution;


        public override string getClassName()
        {
            return exportClassname;
        }

        protected override string getExportTag()
        {
            return exportTag;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SendState();
        }

        protected SendState() { }

        public SendState(ISubjectBehavior behavior, string labelForID = null, IGuardBehavior guardBehavior = null,
            ISendFunction functionSpecification = null,
            ISet<ITransition> incomingTransition = null, ISendTransition sendTransition = null,
            ISet<ISendingFailedTransition> sendingFailedTransitions = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForID, guardBehavior, functionSpecification, incomingTransition, null, comment, additionalLabel, additionalAttribute)
        {
            // Not passing these to base, rather pass null to avoid setting false values
            setSendTransition(sendTransition);
            setSendingFailedTransitions(sendingFailedTransitions);
        }

        public new ISendFunction getFunctionSpecification()
        {
            return (ISendFunction)base.getFunctionSpecification();
        }

        public override void setFunctionSpecification(IFunctionSpecification specification, int removeCascadingDepth = 0)
        {
            // Only set if it is SendFunction
            if (specification is ISendFunction)
            {
                base.setFunctionSpecification(specification, removeCascadingDepth);
            }
            else
            {
                base.setFunctionSpecification(null);
            }
        }

        public override void addOutgoingTransition(ITransition transition)
        {
            if (transition is null) return;
            if (transition is ISendTransition sendTrans)
            {
                setSendTransition(sendTrans);
            }
            else if (!(transition is IDoTransition || transition is IReceiveTransition))
            {
                // if no do or receive transition
                base.addOutgoingTransition(transition);
            }
        }


        public void setSendTransition(ISendTransition sendTransition, int removeCascadingDepth = 0)
        {
            if (sendTransition is null) return;
            foreach (ITransition trans in getOutgoingTransitions().Values)
            {
                // Can only have one send transition
                if (trans is ISendTransition)
                    if (trans.Equals(sendTransition)) return;
                removeOutgoingTransition(trans.getModelComponentID(), removeCascadingDepth);
                break;
            }
            base.addOutgoingTransition(sendTransition);
        }

        public ISendTransition getSendTransition()
        {
            foreach (ITransition trans in outgoingTransitions.Values)
            {
                if (trans is ISendTransition sendTrans) return sendTrans;
            }
            return null;
        }

        public void addSendingFailedTransition(ISendingFailedTransition sendingFailedTransition)
        {
            addOutgoingTransition(sendingFailedTransition);
        }

        public IDictionary<string, ISendingFailedTransition> getSendingFailedTransitions()
        {
            IDictionary<string, ISendingFailedTransition> failedTransitions = new Dictionary<string, ISendingFailedTransition>();
            foreach (ITransition trans in outgoingTransitions.Values)
            {
                if (trans is ISendingFailedTransition sendTrans) failedTransitions.Add(sendTrans.getModelComponentID(), sendTrans);
            }
            return failedTransitions;
        }

        public void removeSendingFailedTransition(string id, int removeCascadingDepth = 0)
        {
            if (id is null) return;
            if (outgoingTransitions.TryGetValue(id, out ITransition transition))
            {
                if (transition is ISendingFailedTransition sendFailedTransition)
                {
                    removeOutgoingTransition(sendFailedTransition.getModelComponentID(), removeCascadingDepth);
                }
            }
        }

        public void setSendingFailedTransitions(ISet<ISendingFailedTransition> transitions, int removeCascadingDepth = 0)
        {
            foreach (ITransition trans in getOutgoingTransitions().Values)
            {
                if (trans is ISendingFailedTransition sendFailed)
                    removeOutgoingTransition(sendFailed.getModelComponentID(), removeCascadingDepth);
            }
            if (transitions is null) return;
            foreach (ISendingFailedTransition sendingFailed in transitions)
            {
                addSendingFailedTransition(sendingFailed);
            }
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.hasFunctionSpecification) && element is IFunctionSpecification sendFunction)
                {
                    setFunctionSpecification(sendFunction);
                    return true;
                }

                if (predicate.Contains(OWLTags.hasOutgoingTransition) && element is ITransition transition)
                {
                    addOutgoingTransition(transition);
                    return true;
                }
            }
            else if (predicate.Contains(OWLTags.type))
            {
                if (objectContent.Contains("AbstractSendState"))
                {
                    setIsStateType(StateType.Abstract);
                    return true;
                }
                else if (objectContent.Contains("FinalizedSendState"))
                {
                    setIsStateType(StateType.Finalized);
                    return true;
                }
            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimDurationMeanValue))
            {
                if (this.sisiExecutionDuration == null)
                {
                    this.sisiExecutionDuration = new SisiTimeDistribution();
                }
                this.sisiExecutionDuration.meanValue = SisiTimeDistribution.ConvertXSDDurationStringToFractionsOfDay(objectContent);
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimDurationDeviation))
            {
                if (this.sisiExecutionDuration == null)
                {
                    this.sisiExecutionDuration = new SisiTimeDistribution();
                }
                this.sisiExecutionDuration.standardDeviation = SisiTimeDistribution.ConvertXSDDurationStringToFractionsOfDay(objectContent);
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimDurationMinValue))
            {
                if (this.sisiExecutionDuration == null)
                {
                    this.sisiExecutionDuration = new SisiTimeDistribution();
                }
                this.sisiExecutionDuration.minValue = SisiTimeDistribution.ConvertXSDDurationStringToFractionsOfDay(objectContent);
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimDurationMaxValue))
            {
                if (this.sisiExecutionDuration == null)
                {
                    this.sisiExecutionDuration = new SisiTimeDistribution();
                }
                this.sisiExecutionDuration.maxValue = SisiTimeDistribution.ConvertXSDDurationStringToFractionsOfDay(objectContent);
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimCostPerExecution))
            {
                try
                {
                    this.sisiCostPerExecution = double.Parse(objectContent);
                }
                catch (System.Exception e)
                {
                    Log.Warning("could not parse the value " + objectContent + " as valid double");
                }
                return true;
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override void setIsStateType(StateType stateType)
        {
            if (!stateTypes.Contains(stateType))
            {
                switch (stateType)
                {
                    case StateType.Abstract:
                        removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + getClassName()));
                        exportTag = OWLTags.abstr;
                        exportClassname = "Abstract" + className;
                        addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + getClassName()));
                        if (isStateType(StateType.Finalized))
                            removeStateType(StateType.Finalized);
                        break;
                    case StateType.Finalized:
                        removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + getClassName()));
                        exportTag = OWLTags.abstr;
                        exportClassname = "Finalized" + className;
                        addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + getClassName()));
                        if (isStateType(StateType.Abstract))
                            removeStateType(StateType.Abstract);
                        break;
                    default:
                        if (!stateType.Equals(StateType.EndState)) { base.setIsStateType(stateType); }
                        break;
                }
            }

        }

        public override void removeStateType(StateType stateType)
        {
            if (stateTypes.Contains(stateType))
            {
                switch (stateType)
                {
                    case StateType.Abstract:
                        removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, OWLTags.std + "Abstract" + getExportTag() + getClassName()));
                        stateTypes.Remove(stateType);
                        exportTag = OWLTags.std;
                        exportClassname = className;
                        addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + getClassName()));
                        break;
                    case StateType.Finalized:
                        removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, OWLTags.std + "Finalized" + getExportTag() + getClassName()));
                        stateTypes.Remove(stateType);
                        exportTag = OWLTags.std;
                        exportClassname = className;
                        addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + getClassName()));
                        break;
                    default:
                        base.removeStateType(stateType);
                        break;
                }
            }
        }

        public ISiSiTimeDistribution getSisiExecutionDuration()
        {
            return this.sisiExecutionDuration;
        }

        public void setSisiExecutionDuration(ISiSiTimeDistribution sisiExecutionDuration)
        {
            this.sisiExecutionDuration = sisiExecutionDuration;
        }


        public double getSisiCostPerExecution()
        {
            return this.sisiCostPerExecution;
        }

        public void setSisiCostPerExecution(double sisiCostPerExecution)
        {
            this.sisiCostPerExecution = sisiCostPerExecution;
        }
    }
}
