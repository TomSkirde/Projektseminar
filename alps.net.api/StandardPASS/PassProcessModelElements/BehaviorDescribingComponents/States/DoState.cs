using alps.net.api.util;
using System.Collections.Generic;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using static alps.net.api.StandardPASS.IState;
using alps.net.api.StandardPASS.PassProcessModelElements.DataDescribingComponents;
using Serilog;
using Serilog.Configuration;
using System;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a DoState
    /// </summary>

    public class DoState : StandardPASSState, IDoState
    {
        protected readonly ICompDict<string, IDataMappingFunction> generalDataMappingFunctions = new CompDict<string, IDataMappingFunction>();
        protected readonly ICompDict<string, IDataMappingIncomingToLocal> dataMappingIncomingToLocalDict = new CompDict<string, IDataMappingIncomingToLocal>();
        protected readonly ICompDict<string, IDataMappingLocalToOutgoing> dataMappingLocalToOutgoingDict = new CompDict<string, IDataMappingLocalToOutgoing>();

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "DoState";
        protected string exportTag = OWLTags.std;
        protected string exportClassname = className;


        protected ISiSiTimeDistribution sisiExecutionDuration;
        protected double sisiCostPerExecution;
        //public ISiSiTimeDistribution sisiExecutionDuration { get; set; }
        //public double sisiCostPerExecution { get; set; } = 0;
        private double _sisiEndStayChance = 0;

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
        public double getSisiEndStayChance() { return this._sisiEndStayChance; }
        public void setSisiEndStayChance(double value)
        {
            // Add validation logic
            if (value >= 0.0 && value <= 1.0)
            {
                _sisiEndStayChance = value;
            }
            else
            {
                if (value < 0)
                {
                    _sisiEndStayChance = 0;
                    Log.Warning("Value for sisiEndStayChance is smaller than 0. Setting it to 0.");
                }
                else if (value > 1)
                {
                    _sisiEndStayChance = 1;
                    Log.Warning("Value for sisiEndStayChance is larger than 1. Setting it to 1.");
                }
            }

        }

        protected SimpleSimTimeCategory sisiVSMTimeCategory;

        public SimpleSimTimeCategory getSisiVSMTimeCategory() { return this.sisiVSMTimeCategory; }

        public void setSisiVSMTimeCategory(SimpleSimTimeCategory simpleSimTimeCategory)
        {
            this.sisiVSMTimeCategory = simpleSimTimeCategory;
        }


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
            return new DoState();
        }

        protected DoState() { }
        public DoState(ISubjectBehavior behavior, string labelForID = null, IGuardBehavior guardBehavior = null,
            IDoFunction doFunction = null, ISet<ITransition> incomingTransitions = null, ISet<ITransition> outgoingTransitions = null,
            ISet<IDataMappingIncomingToLocal> dataMappingIncomingToLocal = null, ISet<IDataMappingLocalToOutgoing> dataMappingLocalToOutgoing = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttributes = null)
            : base(behavior, labelForID, guardBehavior, doFunction, incomingTransitions, null, comment, additionalLabel, additionalAttributes)
        {
            // Set those attributes locally and pass null to base (so no wrong attributes will be set)
            setDataMappingFunctionsIncomingToLocal(dataMappingIncomingToLocal);
            setDataMappingFunctionsLocalToOutgoing(dataMappingLocalToOutgoing);
            setOutgoingTransitions(outgoingTransitions);
        }


        // #################### DataMappingFunctions ####################

        public void setDataMappingFunctionsIncomingToLocal(ISet<IDataMappingIncomingToLocal> dataMappingIncomingToLocal, int removeCascadeDepth = 0)
        {
            foreach (IDataMappingIncomingToLocal mapping in getDataMappingFunctionsIncomingToLocal().Values)
            {
                removeDataMappingFunctionIncomingToLocal(mapping.getModelComponentID(), removeCascadeDepth);
            }
            if (dataMappingIncomingToLocal is null) return;
            foreach (IDataMappingIncomingToLocal mapping in dataMappingIncomingToLocal)
            {
                addDataMappingFunctionIncomingToLocal(mapping);
            }
        }

        public void addDataMappingFunctionIncomingToLocal(IDataMappingIncomingToLocal dataMappingIncomingToLocal)
        {
            if (dataMappingIncomingToLocal is null) { return; }
            if (dataMappingIncomingToLocalDict.TryAdd(dataMappingIncomingToLocal.getModelComponentID(), dataMappingIncomingToLocal))
            {
                publishElementAdded(dataMappingIncomingToLocal);
                dataMappingIncomingToLocal.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataMappingFunction, dataMappingIncomingToLocal.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IDataMappingIncomingToLocal> getDataMappingFunctionsIncomingToLocal()
        {
            return new Dictionary<string, IDataMappingIncomingToLocal>(dataMappingIncomingToLocalDict);
        }

        public void removeDataMappingFunctionIncomingToLocal(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (dataMappingIncomingToLocalDict.TryGetValue(id, out IDataMappingIncomingToLocal mapping))
            {
                dataMappingIncomingToLocalDict.Remove(id);
                mapping.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataMappingFunction, mapping.getUriModelComponentID()));
            }
        }


        public void setDataMappingFunctionsLocalToOutgoing(ISet<IDataMappingLocalToOutgoing> dataMappingLocalToOutgoing, int removeCascadeDepth = 0)
        {
            foreach (IDataMappingLocalToOutgoing mapping in getDataMappingFunctionsLocalToOutgoing().Values)
            {
                removeDataMappingFunctionLocalToOutgoing(mapping.getModelComponentID(), removeCascadeDepth);
            }
            if (dataMappingLocalToOutgoing is null) return;
            foreach (IDataMappingLocalToOutgoing mapping in dataMappingLocalToOutgoing)
            {
                addDataMappingFunctionLocalToOutgoing(mapping);
            }
        }

        public void addDataMappingFunctionLocalToOutgoing(IDataMappingLocalToOutgoing dataMappingLocalToOutgoing)
        {
            if (dataMappingLocalToOutgoing is null) { return; }
            if (dataMappingLocalToOutgoingDict.TryAdd(dataMappingLocalToOutgoing.getModelComponentID(), dataMappingLocalToOutgoing))
            {
                publishElementAdded(dataMappingLocalToOutgoing);
                dataMappingLocalToOutgoing.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataMappingFunction, dataMappingLocalToOutgoing.getUriModelComponentID()));
            }
        }



        public IDictionary<string, IDataMappingLocalToOutgoing> getDataMappingFunctionsLocalToOutgoing()
        {
            return new Dictionary<string, IDataMappingLocalToOutgoing>(dataMappingLocalToOutgoingDict);
        }

        public void removeDataMappingFunctionLocalToOutgoing(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (dataMappingLocalToOutgoingDict.TryGetValue(id, out IDataMappingLocalToOutgoing mapping))
            {
                dataMappingLocalToOutgoingDict.Remove(id);
                mapping.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataMappingFunction, mapping.getUriModelComponentID()));
            }
        }


        public void addDataMappingFunction(IDataMappingFunction dataMappingFunction)
        {
            if (dataMappingFunction is null) { return; }
            if (generalDataMappingFunctions.TryAdd(dataMappingFunction.getModelComponentID(), dataMappingFunction))
            {
                publishElementAdded(dataMappingFunction);
                dataMappingFunction.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataMappingFunction, dataMappingFunction.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IDataMappingFunction> getDataMappingFunctions()
        {
            return new Dictionary<string, IDataMappingFunction>(generalDataMappingFunctions);
        }


        public void removeDataMappingFunction(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (generalDataMappingFunctions.TryGetValue(id, out IDataMappingFunction mapping))
            {
                dataMappingLocalToOutgoingDict.Remove(id);
                mapping.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataMappingFunction, mapping.getUriModelComponentID()));
            }
        }


        // ########################################


        public override void addOutgoingTransition(ITransition transition)
        {
            if (!(transition is ISendTransition || transition is IReceiveTransition))
                base.addOutgoingTransition(transition);
        }


        public new void setFunctionSpecification(IFunctionSpecification specification, int removeCascadeDepth = 0)
        {
            if (specification is IDoFunction)
            {
                base.setFunctionSpecification(specification, removeCascadeDepth);
            }
            else
            {
                base.setFunctionSpecification(null, removeCascadeDepth);
            }
        }

        public new IDoFunction getFunctionSpecification()
        {
            return (IDoFunction)base.getFunctionSpecification();
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.hasDataMappingFunction) && element is IDataMappingIncomingToLocal incomingMapping)
                {
                    addDataMappingFunctionIncomingToLocal(incomingMapping);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasDataMappingFunction) && element is IDataMappingLocalToOutgoing outgoingMapping)
                {
                    addDataMappingFunctionLocalToOutgoing(outgoingMapping);
                    return true;

                }
                else if (predicate.Contains(OWLTags.hasDataMappingFunction) && element is IDataMappingFunction functionMapping)
                {
                    addDataMappingFunction(functionMapping);
                    return true;
                }
                else if (predicate.Contains(OWLTags.hasFunctionSpecification) && element is IDoFunction function)
                {
                    setFunctionSpecification(function);
                    return true;
                }

                if (predicate.Contains(OWLTags.abstrHasSimpleSimDurationMeanValue))
                {

                }
            }
            else if (predicate.Contains(OWLTags.type))
            {
                if (objectContent.Contains("AbstractDoState"))
                {
                    setIsStateType(StateType.Abstract);
                    return true;
                }
                else if (objectContent.Contains("FinalizedDoState"))
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
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimEndStayChance))
            {
                try
                {
                    _sisiEndStayChance = double.Parse(objectContent);
                }
                catch (System.Exception e)
                {
                    Log.Warning("could not parse the value " + objectContent + " as valid double");
                }
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimVSMTimeCategory))
            {
                try
                {
                    this.sisiVSMTimeCategory = parseSimpleSimTimeCategory(objectContent);
                }
                catch (System.Exception e)
                {
                    Log.Warning("could not parse the value " + objectContent + " as valid Time Category");
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
                        base.setIsStateType(stateType);
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


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IDataMappingIncomingToLocal component in getDataMappingFunctionsIncomingToLocal().Values)
                baseElements.Add(component);
            foreach (IDataMappingLocalToOutgoing component in getDataMappingFunctionsLocalToOutgoing().Values)
                baseElements.Add(component);
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IDataMappingIncomingToLocal mappingIn)
                    removeDataMappingFunctionIncomingToLocal(mappingIn.getModelComponentID(), removeCascadeDepth);
                if (update is IDataMappingLocalToOutgoing mappingOut)
                    removeDataMappingFunctionLocalToOutgoing(mappingOut.getModelComponentID(), removeCascadeDepth);
            }
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (dataMappingIncomingToLocalDict.ContainsKey(oldID))
            {
                IDataMappingIncomingToLocal element = dataMappingIncomingToLocalDict[oldID];
                dataMappingIncomingToLocalDict.Remove(oldID);
                dataMappingIncomingToLocalDict.Add(element.getModelComponentID(), element);
            }

            if (dataMappingLocalToOutgoingDict.ContainsKey(oldID))
            {
                IDataMappingLocalToOutgoing element = dataMappingLocalToOutgoingDict[oldID];
                dataMappingLocalToOutgoingDict.Remove(oldID);
                dataMappingLocalToOutgoingDict.Add(element.getModelComponentID(), element);
            }

            base.notifyModelComponentIDChanged(oldID, newID);
        }

        public void setEndState(bool isEndState)
        {
            if (isEndState)
            {
                if (!this.isStateType(StateType.EndState))
                {
                    this.setIsStateType(StateType.EndState);
                }
            }
            else
            {
                if (this.isStateType(StateType.EndState))
                {
                    this.removeStateType(StateType.EndState);
                }
            }
        }

        public bool isEndState()
        {
            return this.isStateType(StateType.EndState);
        }

        /// <summary>
        /// The method will try to pars a given value and return on the the according enum types
        /// If pasring is not possible the default value will be given.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static SimpleSimTimeCategory parseSimpleSimTimeCategory(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = "nothing correct";
            }

            foreach (SimpleSimTimeCategory type in Enum.GetValues(typeof(SimpleSimTimeCategory)))
            {
                if (value.ToLower().Contains(type.ToString().ToLower()))
                {
                    return type;
                }
            }

            return SimpleSimTimeCategory.Standard;

        }
    }
}
