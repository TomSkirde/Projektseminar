using alps.net.api.util;
using System.Collections.Generic;
using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.FunctionalityCapsules;
using alps.net.api.parsing.graph;
using static alps.net.api.StandardPASS.IState;
using System.Diagnostics;
using System;
using System.Globalization;
using Serilog;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a state
    /// </summary>
    public class State : BehaviorDescribingComponent, IStateReference, IMacroState //IState
    {
        protected readonly ICompDict<string, ITransition> incomingTransitions = new CompDict<string, ITransition>();
        protected readonly ICompDict<string, ITransition> outgoingTransitions = new CompDict<string, ITransition>();
        protected readonly IImplementsFunctionalityCapsule<IState> implCapsule;
        protected readonly ISet<StateType> stateTypes = new HashSet<StateType>();
        protected IFunctionSpecification functionSpecification;
        protected IGuardBehavior guardBehavior;
        protected IAction action;
        protected IState referenceState;
        protected IMacroBehavior referenceMacroBehavior;
        protected readonly ICompDict<string, IStateReference> stateReferences = new CompDict<string, IStateReference>();

        private double has2DPageRatio = -1;
        private double hasRelative2D_Height = -1;
        private double hasRelative2D_Width = -1;
        private double hasRelative2D_PosX = -1;
        private double hasRelative2D_PosY = -1;



        public double get2DPageRatio()
        {
            return has2DPageRatio;
        }

        public void set2DPageRatio(double has2DPageRatio)
        {
            if (has2DPageRatio > 0)
            {
                this.has2DPageRatio = has2DPageRatio;
            }
            if (has2DPageRatio == 0)
            {
                this.has2DPageRatio = 1;
                Log.Warning("found 2D page ratio of 0. This is impossible. changed it to 1");
            }
            else
            {
                this.has2DPageRatio = Math.Abs(has2DPageRatio);
                Log.Warning("found negative 2d page ratio. Changed it to positive value");
            }
        }

        public double getRelative2DHeight()
        {
            return hasRelative2D_Height;
        }

        public void setRelative2DHeight(double relative2DHeight)
        {
            if (relative2DHeight >= 0 && relative2DHeight <= 1)
            {
                hasRelative2D_Height = relative2DHeight;
            }
            else
            {
                if (relative2DHeight < 0)
                {
                    hasRelative2D_Height = 0;
                    Log.Warning("Value for relative2DHeight is smaller than 0. Setting it to 0.");
                }
                else if (relative2DHeight > 1)
                {
                    hasRelative2D_Height = 1;
                    Log.Warning("Value for relative2DHeight is larger than 1. Setting it to 1.");
                }
            }

        }

        public double getRelative2DWidth()
        {
            return hasRelative2D_Width;
        }

        public void setRelative2DWidth(double relative2DWidth)
        {

            if (relative2DWidth >= 0 && relative2DWidth <= 1)
            {
                hasRelative2D_Width = relative2DWidth;
            }
            else
            {
                if (relative2DWidth < 0)
                {
                    hasRelative2D_Width = 0;
                    Log.Warning("Value for relative2DWidth is smaller than 0. Setting it to 0.");
                }
                else if (relative2DWidth > 1)
                {
                    hasRelative2D_Width = 1;
                    Log.Warning("Value for relative2DWidth is larger than 1. Setting it to 1.");
                }
            }

        }

        public double getRelative2DPosX()
        {
            return hasRelative2D_PosX;
        }

        public void setRelative2DPosX(double relative2DPosX)
        {
            if (relative2DPosX >= 0 && relative2DPosX <= 1)
            {
                hasRelative2D_PosX = relative2DPosX;
            }
            else
            {
                if (relative2DPosX < 0)
                {
                    hasRelative2D_PosX = 0;
                    Log.Warning("Value for relative2DPosX is smaller than 0. Setting it to 0.");
                }
                else if (relative2DPosX > 1)
                {
                    hasRelative2D_PosX = 1;
                    Log.Warning("Value for relative2DPosX is larger than 1. Setting it to 1.");
                }
            }
        }

        public double getRelative2DPosY()
        {
            return hasRelative2D_PosY;
        }

        public void setRelative2DPosY(double relative2DPosY)
        {
            if (relative2DPosY >= 0 && relative2DPosY <= 1)
            {
                hasRelative2D_PosY = relative2DPosY;
            }
            else
            {
                if (relative2DPosY < 0)
                {
                    hasRelative2D_PosY = 0;
                    Log.Warning("Value for relative2DPosY is smaller than 0. Setting it to 0.");
                }
                else if (relative2DPosY > 1)
                {
                    hasRelative2D_PosY = 1;
                    Log.Warning("Value for relative2DPosY is larger than 1. Setting it to 1.");
                }
            }
        }


        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "State";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new State();
        }

        protected State() { implCapsule = new ImplementsFunctionalityCapsule<IState>(this); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="action"></param>
        /// <param name="guardBehavior"></param>
        /// <param name="functionSpecification"></param>
        /// <param name="incomingTransition"></param>
        /// <param name="outgoingTransition"></param>
        /// <param name="additionalAttribute"></param>
        public State(ISubjectBehavior behavior, string labelForID = null, IGuardBehavior guardBehavior = null,
            IFunctionSpecification functionSpecification = null, ISet<ITransition> incomingTransition = null, ISet<ITransition> outgoingTransition = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null) : base(behavior, labelForID, comment, additionalLabel, additionalAttribute)
        {
            implCapsule = new ImplementsFunctionalityCapsule<IState>(this);
            setGuardBehavior(guardBehavior);
            setFunctionSpecification(functionSpecification);
            setIncomingTransitions(incomingTransition);
            setOutgoingTransitions(outgoingTransition);
            generateAction();
        }


        public virtual void addIncomingTransition(ITransition transition)
        {
            if (transition is null) { return; }
            if (incomingTransitions.TryAdd(transition.getModelComponentID(), transition))
            {
                transition.setTargetState(this);
                publishElementAdded(transition);
                transition.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasIncomingTransition, transition.getUriModelComponentID()));
            }
        }

        /// <summary>
        /// Method that returns the incoming transition attribute of the instance
        /// </summary>
        /// <returns>The incoming transition attribute of the instance</returns>
        public IDictionary<string, ITransition> getIncomingTransitions()
        {
            return new Dictionary<string, ITransition>(incomingTransitions);
        }


        public virtual void addOutgoingTransition(ITransition transition)
        {
            if (transition is null) { return; }
            if (outgoingTransitions.TryAdd(transition.getModelComponentID(), transition))
            {
                transition.setSourceState(this);
                publishElementAdded(transition);
                transition.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasOutgoingTransition, transition.getUriModelComponentID()));
            }
        }

        public void setOutgoingTransitions(ISet<ITransition> transitions, int removeCascadeDepth = 0)
        {
            foreach (ITransition transition in getOutgoingTransitions().Values)
            {
                removeOutgoingTransition(transition.getModelComponentID());
            }
            if (transitions is null) return;
            foreach (ITransition transition in transitions)
            {
                addOutgoingTransition(transition);
            }
        }

        public void setIncomingTransitions(ISet<ITransition> transitions, int removeCascadeDepth = 0)
        {
            foreach (ITransition transition in getIncomingTransitions().Values)
            {
                removeIncomingTransition(transition.getModelComponentID());
            }
            if (transitions is null) return;
            foreach (ITransition transition in transitions)
            {
                addIncomingTransition(transition);
            }
        }

        public void removeOutgoingTransition(string modelCompID, int removeCascadeDepth = 0)
        {
            if (modelCompID is null) return;
            if (outgoingTransitions.TryGetValue(modelCompID, out ITransition transition))
            {
                outgoingTransitions.Remove(modelCompID);
                transition.unregister(this);
                transition.setSourceState(null);
                action.updateRemoved(transition, this);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasOutgoingTransition, transition.getUriModelComponentID()));
            }
        }

        public void removeIncomingTransition(string modelCompID, int removeCascadeDepth = 0)
        {
            if (modelCompID is null) return;
            if (incomingTransitions.TryGetValue(modelCompID, out ITransition transition))
            {
                incomingTransitions.Remove(modelCompID);
                transition.unregister(this);
                transition.setTargetState(null);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasIncomingTransition, transition.getUriModelComponentID()));
            }
        }

        /// <summary>
        /// Method that returns the outgoing transition attribute of the instance
        /// </summary>
        /// <returns>The outgoing transition attribute of the instance</returns>
        public IDictionary<string, ITransition> getOutgoingTransitions()
        {
            return new Dictionary<string, ITransition>(outgoingTransitions);
        }


        public virtual void setFunctionSpecification(IFunctionSpecification funSpec, int removeCascadeDepth = 0)
        {
            IFunctionSpecification oldSpec = functionSpecification;
            // Might set it to null
            functionSpecification = funSpec;

            if (oldSpec != null)
            {
                if (oldSpec.Equals(funSpec)) return;
                oldSpec.unregister(this);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasFunctionSpecification, oldSpec.getUriModelComponentID()));
            }

            if (funSpec is not null)
            {
                publishElementAdded(funSpec);
                funSpec.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasFunctionSpecification, funSpec.getUriModelComponentID()));
            }
        }


        public IFunctionSpecification getFunctionSpecification()
        {
            return functionSpecification;
        }


        public void setGuardBehavior(IGuardBehavior guardBehav, int removeCascadeDepth = 0)
        {
            IGuardBehavior oldBehavior = guardBehavior;
            // Might set it to null
            guardBehavior = guardBehav;

            if (oldBehavior != null)
            {
                if (oldBehavior.Equals(guardBehav)) return;
                oldBehavior.unregister(this, removeCascadeDepth);
                oldBehavior.removeGuardedState(getModelComponentID());
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdGuardedBy, oldBehavior.getUriModelComponentID()));
            }

            if (guardBehav is not null)
            {
                publishElementAdded(guardBehav);
                guardBehav.register(this);
                guardBehav.addGuardedState(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdGuardedBy, guardBehav.getUriModelComponentID()));
            }
        }


        public IGuardBehavior getGuardBehavior()
        {
            return guardBehavior;
        }


        protected void generateAction(IAction newGeneratedAction = null)
        {
            IAction oldAction = action;
            // Might set it to null
            string label = (getModelComponentLabelsAsStrings().Count > 0) ? getModelComponentLabelsAsStrings()[0] : getModelComponentID();
            this.action = (newGeneratedAction is null) ? new Action(this, "ActionFor" + label) : newGeneratedAction;

            if (oldAction != null)
            {
                if (oldAction.Equals(action)) return;
                oldAction.unregister(this);
                oldAction.removeFromEverything();
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdBelongsTo, oldAction.getUriModelComponentID()));
            }

            if (!(action is null))
            {
                publishElementAdded(action);
                action.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdBelongsTo, action.getUriModelComponentID()));
            }
        }


        public IAction getAction()
        {
            return action;
        }

        public bool isStateType(StateType stateType)
        {
            return stateTypes.Contains(stateType);
        }

        public virtual void setIsStateType(StateType stateType)
        {
            switch (stateType)
            {
                case StateType.InitialStateOfBehavior:
                    if (stateTypes.Add(stateType))
                        addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, OWLTags.std + "InitialStateOfBehavior"));
                    break;
                case StateType.InitialStateOfChoiceSegmentPath:
                    if (stateTypes.Add(stateType))
                        addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, OWLTags.std + "InitialStateOfChoiceSegmentPath"));
                    break;
                case StateType.EndState:
                    if (stateTypes.Add(stateType))
                    {
                        addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, OWLTags.std + "EndState"));

                        if (getContainedBy(out ISubjectBehavior behavior) && (behavior is ISubjectBaseBehavior baseBehav))
                        {
                            baseBehav.registerEndState(this);
                        }
                    }
                    break;
            }

        }

        public virtual void removeStateType(StateType stateType)
        {
            // If the type was removed successfully
            if (stateTypes.Remove(stateType))
            {
                switch (stateType)
                {
                    case StateType.InitialStateOfBehavior:
                        removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, OWLTags.std + "InitialStateOfBehavior"));
                        if (getContainedBy(out ISubjectBehavior behav))
                        {
                            behav.setInitialState(null);
                        }
                        break;
                    case StateType.InitialStateOfChoiceSegmentPath:
                        removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, OWLTags.std + "InitialStateOfChoiceSegmentPath"));
                        break;
                    case StateType.EndState:
                        removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, OWLTags.std + "EndState"));
                        if (getContainedBy(out ISubjectBehavior behavior) && behavior is ISubjectBaseBehavior baseBehav)
                        {
                            baseBehav.unregisterEndState(getModelComponentID());
                        }
                        break;
                }

            }
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {


            if (implCapsule != null && implCapsule.parseAttribute(predicate, objectContent, lang, dataType, element))
            {
                return true;
            }
            else if (element != null)
            {
                //Console.Write("Parsing non null parseAttribute for State: " + this.getModelComponentID());
                //Console.WriteLine(" - predicate: " + predicate);
                if (element is ITransition transition)
                {
                    if (predicate.Contains(OWLTags.hasIncomingTransition))
                    {
                        addIncomingTransition(transition);
                        return true;
                    }
                    else if (predicate.Contains(OWLTags.hasOutgoingTransition))
                    {
                        addOutgoingTransition(transition);
                        return true;
                    }

                }
                else if (predicate.Contains(OWLTags.guardedBy) && element is IGuardBehavior guard)
                {
                    setGuardBehavior(guard);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasFunctionSpecification) && element is IFunctionSpecification specification)
                {
                    setFunctionSpecification(specification);
                    return true;
                }

                else if (predicate.Contains(OWLTags.belongsTo) && element is IAction action)
                {
                    generateAction(action);
                    return true;
                }

                else if (predicate.Contains(OWLTags.referencesMacroBehavior) && element is IMacroBehavior behavior)
                {
                    setReferencedMacroBehavior(behavior);
                    return true;
                }

            }

            else if (predicate.Contains(OWLTags.abstrHas2DPageRatio))
            {
                set2DPageRatio(double.Parse(objectContent, customCulture));
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasRelative2D_PosX))
            {
                setRelative2DPosX(double.Parse(objectContent, customCulture));
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasRelative2D_PosY))
            {
                setRelative2DPosY(double.Parse(objectContent, customCulture));
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasRelative2D_Height))
            {
                setRelative2DHeight(double.Parse(objectContent, customCulture));
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasRelative2D_Width))
            {
                setRelative2DWidth(double.Parse(objectContent, customCulture));
                return true;
            }

            if (predicate.Contains(OWLTags.type))
            {
                if (objectContent.ToLower().Contains("endstate") && !(objectContent.ToLower().Contains("sendstate")))
                {
                    setIsStateType(StateType.EndState);
                    return true;
                }
                else if (objectContent.ToLower().Contains("initialstateofbehavior"))
                {
                    setIsStateType(StateType.InitialStateOfBehavior);
                    return true;
                }
                else if (objectContent.ToLower().Contains("initialstateofchoicesegmentpath"))
                {
                    setIsStateType(StateType.InitialStateOfChoiceSegmentPath);
                    return true;
                }

                //Abstract and finalized are part of the individual states


            }

            /*
            if (predicate.Contains("referencesMacroBehavior"))
            {
                Debug.WriteLine("element: " + element + " object " + objectContent);
                //return true;
            }
            */


            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getAction() != null)
                baseElements.Add(getAction());
            if (getGuardBehavior() != null && specification == ConnectedElementsSetSpecification.ALL)
                baseElements.Add(getGuardBehavior());
            if (getFunctionSpecification() != null && (specification == ConnectedElementsSetSpecification.ALL || specification == ConnectedElementsSetSpecification.TO_ADD))
                baseElements.Add(getFunctionSpecification());
            if (specification == ConnectedElementsSetSpecification.ALL || specification == ConnectedElementsSetSpecification.TO_ADD ||
                specification == ConnectedElementsSetSpecification.TO_REMOVE_DIRECTLY_ADJACENT || specification == ConnectedElementsSetSpecification.TO_REMOVE_ADJACENT_AND_MORE)
            {
                foreach (ITransition transition in getOutgoingTransitions().Values)
                    baseElements.Add(transition);

                if (specification != ConnectedElementsSetSpecification.TO_REMOVE_DIRECTLY_ADJACENT)
                    foreach (ITransition transition in getIncomingTransitions().Values)
                        baseElements.Add(transition);
            }
            return baseElements;
        }


        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller);
            if (update is null) return;
            if (update.Equals(action))
            {
                // TODO what to do here?
            }
            else if (update.Equals(guardBehavior))
            {
                setGuardBehavior(null, removeCascadeDepth);
            }
            else if (update.Equals(functionSpecification))
            {
                setFunctionSpecification(null, removeCascadeDepth);
            }
            else
            {
                foreach (ITransition trans in incomingTransitions.Values)
                {
                    if (update.Equals(trans))
                    {
                        removeIncomingTransition(trans.getModelComponentID(), removeCascadeDepth);
                        return;
                    }
                }
                foreach (ITransition trans in outgoingTransitions.Values)
                {
                    if (update.Equals(trans))
                    {
                        removeOutgoingTransition(trans.getModelComponentID(), removeCascadeDepth);
                        return;
                    }
                }
            }
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (incomingTransitions.ContainsKey(oldID))
            {
                ITransition element = incomingTransitions[oldID];
                incomingTransitions.Remove(oldID);
                incomingTransitions.Add(element.getModelComponentID(), element);
            }

            if (outgoingTransitions.ContainsKey(oldID))
            {
                ITransition element = outgoingTransitions[oldID];
                outgoingTransitions.Remove(oldID);
                outgoingTransitions.Add(element.getModelComponentID(), element);
            }
            base.notifyModelComponentIDChanged(oldID, newID);
        }



        protected static readonly string STATE_REF_CLASS_NAME = "StateReference";

        /// <summary>
        /// Sets a state that is referenced by this state.
        /// According to the PASS standard, this functionality belongs to the "StateReference" class.
        /// Here, this functionality is inside the state class and should be used if the current instance should be a StateReference
        /// </summary>
        /// <param name="state">The referenced state</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public virtual void setReferencedState(IState state, int removeCascadeDepth = 0)
        {
            if (!(state.GetType().Equals(this.GetType()))) return;
            IState oldReference = referenceState;
            // Might set it to null
            this.referenceState = state;

            if (oldReference != null)
            {
                if (oldReference.Equals(state)) return;
                oldReference.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdReferences, oldReference.getUriModelComponentID()));
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + getClassName()));
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + STATE_REF_CLASS_NAME));
            }

            if (!(state is null))
            {
                state.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdReferences, state.getUriModelComponentID()));
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + getClassName()));
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + STATE_REF_CLASS_NAME));
            }
        }

        /// <summary>
        /// Gets the state that is referenced by this state.
        /// According to the PASS standard, this functionality belongs to the "StateReference" class.
        /// Here, this functionality is inside the state class and should be used if the current instance should be a StateReference
        /// </summary>
        /// <returns>The referenced state</returns>
        public IState getReferencedState()
        {
            return referenceState;
        }

        public bool isReference()
        {
            return !(referenceState is null);
        }

        public override bool register(IValueChangedObserver<IPASSProcessModelElement> observer)
        {
            bool added = base.register(observer);
            // Special case: State is parsed, action knows about state, state does not know action -> action registeres at the state, state sets reference to action
            if (added && observer is IAction action && getAction() is null)
            {
                generateAction(action);
            }
            return added;
        }

        public override bool unregister(IValueChangedObserver<IPASSProcessModelElement> observer, int removeCascadeDepth = 0)
        {
            bool added = base.unregister(observer, removeCascadeDepth);
            // Special case: State is parsed, action knows about state, state does not know action -> action registeres at the state, state sets reference to action
            if (added && observer is IAction action && getAction().Equals(action))
            {
                generateAction(null);
            }
            return added;
        }

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

        public void setImplementedInterfaces(ISet<IState> implementedInterface, int removeCascadeDepth = 0)
        {
            implCapsule.setImplementedInterfaces(implementedInterface, removeCascadeDepth);
        }

        public void addImplementedInterface(IState implementedInterface)
        {
            implCapsule.addImplementedInterface(implementedInterface);
        }

        public void removeImplementedInterfaces(string id, int removeCascadeDepth = 0)
        {
            implCapsule.removeImplementedInterfaces(id, removeCascadeDepth);
        }

        public IDictionary<string, IState> getImplementedInterfaces()
        {
            return implCapsule.getImplementedInterfaces();
        }


        public void setReferencedMacroBehavior(IMacroBehavior macroBehavior, int removeCascadeDepth = 0)
        {
            IMacroBehavior oldBehavior = this.referenceMacroBehavior;
            // Might set it to null
            this.referenceMacroBehavior = macroBehavior;

            if (oldBehavior != null)
            {
                if (oldBehavior.Equals(macroBehavior)) return;
                oldBehavior.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdReferencesMacroBehavior, oldBehavior.getUriModelComponentID()));
            }

            if (!(macroBehavior is null))
            {
                publishElementAdded(macroBehavior);
                macroBehavior.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdReferencesMacroBehavior, macroBehavior.getUriModelComponentID()));
            }
        }


        public IMacroBehavior getReferencedMacroBehavior()
        {
            return referenceMacroBehavior;
        }

    }
}

