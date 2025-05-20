using alps.net.api.ALPS;
using alps.net.api.FunctionalityCapsules;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System;
using System.Collections.Generic;
using static alps.net.api.StandardPASS.ITransition;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a transition class
    /// </summary>
    public class Transition : BehaviorDescribingComponent, ITransition
    {
        protected IAction belongsToAction;
        protected IState sourceState;
        protected IState targetState;
        protected ITransitionCondition transitionCondition;
        private ITransition.TransitionType transitionType;
        protected readonly IImplementsFunctionalityCapsule<ITransition> implCapsule;
        protected bool isAbstractType = false;

        private const string ABSTRACT_NAME = "AbstractPASSTransition";

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "Transition";

        private double has2DPageRatio = -1;
        private double hasRelative2D_BeginX = -1;
        private double hasRelative2D_BeginY = -1;
        private double hasRelative2D_EndX = -1;
        private double hasRelative2D_EndY = -1;
        private List<ISimple2DVisualizationPathPoint> pathPoints = new List<ISimple2DVisualizationPathPoint>();


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new Transition();
        }

        protected Transition()
        {
            //Console.WriteLine("Standard Transition constructor");
            implCapsule = new ImplementsFunctionalityCapsule<ITransition>(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="action"></param>
        /// <param name="sourceState"></param>
        /// <param name="targetState"></param>
        /// <param name="transitionCondition"></param>
        /// <param name="additionalAttribute"></param>
        /// <param name="transitionType"></param>
        public Transition(IState sourceState, IState targetState, string labelForID = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(null, labelForID, comment, additionalLabel, additionalAttribute)
        {
            //Console.WriteLine("Second Transition constructor type: " + transitionType);
            implCapsule = new ImplementsFunctionalityCapsule<ITransition>(this);
            ISubjectBehavior behavior = null;
            if (sourceState != null)
                sourceState.getContainedBy(out behavior);
            if (behavior is null && targetState != null)
                targetState.getContainedBy(out behavior);
            if (behavior != null)
                setContainedBy(behavior);
            setSourceState(sourceState);
            setTargetState(targetState);
            setTransitionCondition(transitionCondition);
            setTransitionType(transitionType);
        }

        public Transition(ISubjectBehavior behavior, string labelForID = null, IState sourceState = null, IState targetState = null,
            ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForID, comment, additionalLabel, additionalAttribute)
        {
            //Console.WriteLine("Third Transition constructor");
            implCapsule = new ImplementsFunctionalityCapsule<ITransition>(this);
            setSourceState(sourceState);
            setTargetState(targetState);
            setTransitionCondition(transitionCondition);
            setTransitionType(transitionType);
        }


        /// <summary>
        /// Used to set the action that belongs to this transition.
        /// Only called from inside the class, should not be visible to the user (the action is set/removed automatically when a source state is added/removed)
        /// </summary>
        protected void setBelongsToAction(IAction action, int removeCascadeDepth = 0)
        {
            IAction oldAction = belongsToAction;
            // Might set it to null
            this.belongsToAction = action;

            if (oldAction != null)
            {
                if (oldAction.Equals(action)) return;
                oldAction.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdBelongsTo, oldAction.getUriModelComponentID()));
            }

            if (!(action is null))
            {
                publishElementAdded(action);
                action.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdBelongsTo, action.getUriModelComponentID()));
            }
        }


        public virtual void setSourceState(IState sourceState, int removeCascadeDepth = 0)
        {
            IState oldSourceState = this.sourceState;
            // Might set it to null
            this.sourceState = sourceState;

            if (oldSourceState != null)
            {
                if (oldSourceState.Equals(sourceState)) return;
                oldSourceState.unregister(this, removeCascadeDepth);
                oldSourceState.removeOutgoingTransition(getModelComponentID());
                setBelongsToAction(null);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasSourceState, oldSourceState.getUriModelComponentID()));
            }
            if (!(sourceState is null))
            {
                publishElementAdded(sourceState);
                sourceState.register(this);
                sourceState.addOutgoingTransition(this);
                setBelongsToAction(sourceState.getAction());
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasSourceState, sourceState.getUriModelComponentID()));
            }
        }


        public virtual void setTargetState(IState targetState, int removeCascadeDepth = 0)
        {
            IState oldState = this.targetState;
            // Might set it to null
            this.targetState = targetState;

            if (oldState != null)
            {
                if (oldState.Equals(targetState)) return;
                oldState.unregister(this, removeCascadeDepth);
                oldState.removeIncomingTransition(getModelComponentID());
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasTargetState, oldState.getUriModelComponentID()));
            }

            if (!(targetState is null))
            {
                publishElementAdded(targetState);
                targetState.register(this);
                targetState.addIncomingTransition(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasTargetState, targetState.getUriModelComponentID()));
            }
        }


        public virtual void setTransitionCondition(ITransitionCondition transitionCondition, int removeCascadeDepth = 0)
        {
            ITransitionCondition oldCond = this.transitionCondition;
            // Might set it to null
            this.transitionCondition = transitionCondition;

            if (oldCond != null)
            {
                if (oldCond.Equals(transitionCondition)) return;
                oldCond.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasTransitionCondition, oldCond.getUriModelComponentID()));
            }

            if (!(transitionCondition is null))
            {
                publishElementAdded(transitionCondition);
                transitionCondition.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasTransitionCondition, transitionCondition.getUriModelComponentID()));
            }
        }


        public IAction getBelongsToAction()
        {
            return belongsToAction;
        }


        public IState getSourceState()
        {
            return sourceState;
        }


        public IState getTargetState()
        {
            return targetState;
        }


        public ITransitionCondition getTransitionCondition()
        {
            return transitionCondition;
        }

        public void setTransitionType(ITransition.TransitionType type)
        {
            transitionType = type;
        }

        public ITransition.TransitionType getTransitionType()
        {
            return transitionType;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {


            if (implCapsule != null && implCapsule.parseAttribute(predicate, objectContent, lang, dataType, element))
                return true;
            else if (element != null)
            {
                if (predicate.Contains(OWLTags.belongsTo) && element is IAction action)
                {
                    setBelongsToAction(action);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasTransitionCondition) && element is ITransitionCondition condition)
                {
                    setTransitionCondition(condition);
                    return true;
                }

                else if (element is IState state)
                {


                    if (predicate.Contains(OWLTags.hasSourceState))
                    {
                        setSourceState(state);
                        return true;
                    }
                    else if (predicate.Contains(OWLTags.hasTargetState))
                    {
                        setTargetState(state);
                        return true;
                    }

                }

                else if (element is ISimple2DVisualizationPathPoint point)
                {
                    //Console.WriteLine(this.getModelComponentID() + ": PathPoint:" + point.getModelComponentID());
                    if (this.pathPoints == null) this.pathPoints = new List<ISimple2DVisualizationPathPoint>();

                    this.pathPoints.Add(point);
                }

            }
            else //element == null --> data value
            {


                if (predicate.Contains(OWLTags.type))
                {
                    // Console.WriteLine(" - parsing object content transition type: " + objectContent);

                    if (objectContent.ToLower().Contains(ITransition.TransitionType.Finalized.ToString().ToLower()))
                    {
                        setTransitionType(ITransition.TransitionType.Finalized);
                        setIsAbstract(true);
                        return true;
                    }
                    else if (objectContent.ToLower().Contains(ITransition.TransitionType.Precedence.ToString().ToLower()))
                    {
                        setTransitionType(ITransition.TransitionType.Precedence);
                        setIsAbstract(true);
                        return true;
                    }
                    else if (objectContent.ToLower().Contains(ITransition.TransitionType.Trigger.ToString().ToLower()))
                    {
                        setTransitionType(ITransition.TransitionType.Trigger);
                        setIsAbstract(true);
                        return true;
                    }
                    else if (objectContent.ToLower().Contains(ITransition.TransitionType.Advice.ToString().ToLower()))
                    {
                        setTransitionType(ITransition.TransitionType.Advice);
                        setIsAbstract(true);
                        return true;
                    }
                    else if (objectContent.Contains(ABSTRACT_NAME))
                    {
                        setIsAbstract(true);
                        return true;
                    }


                }
                else if (predicate.Contains(OWLTags.abstrHas2DPageRatio))
                {
                    set2DPageRatio(double.Parse(objectContent));
                    return true;
                }
                else if (predicate.Contains(OWLTags.abstrHasRelative2D_BeginX))
                {
                    setRelative2DBeginX(double.Parse(objectContent));
                    return true;
                }
                else if (predicate.Contains(OWLTags.abstrHasRelative2D_BeginY))
                {
                    setRelative2DBeginY(double.Parse(objectContent));
                    return true;
                }
                else if (predicate.Contains(OWLTags.abstrHasRelative2D_EndY))
                {
                    setRelative2DEndY(double.Parse(objectContent));
                    return true;
                }
                else if (predicate.Contains(OWLTags.abstrHasRelative2D_EndX))
                {
                    setRelative2DEndX(double.Parse(objectContent));
                    return true;
                }

            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getBelongsToAction() != null && specification == ConnectedElementsSetSpecification.ALL || specification == ConnectedElementsSetSpecification.TO_ADD)
                baseElements.Add(getBelongsToAction());
            if (specification != ConnectedElementsSetSpecification.TO_ALWAYS_REMOVE)
            {
                if (getSourceState() != null)
                    baseElements.Add(getSourceState());
                if (getTargetState() != null)
                    baseElements.Add(getTargetState());
            }
            if (getTransitionCondition() != null)
                baseElements.Add(getTransitionCondition());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IAction action && action.Equals(getBelongsToAction()))
                    setBelongsToAction(null, removeCascadeDepth);
                if (update is IState state)
                {
                    if (state.Equals(getSourceState()))
                        setSourceState(null, removeCascadeDepth);
                    if (state.Equals(getTargetState()))
                        setTargetState(null, removeCascadeDepth);
                }
                if (update is ITransitionCondition condition && condition.Equals(getTransitionCondition()))
                    setTransitionCondition(null, removeCascadeDepth);
            }
        }

        public override void updateAdded(IPASSProcessModelElement update, IPASSProcessModelElement caller)
        {
            if (update != null)
            {
                if (!(caller is null) && caller.Equals(getSourceState()))
                {
                    if (update is IAction action)
                    {
                        setBelongsToAction(action);
                    }
                }
            }
        }


        public void setIsAbstract(bool isAbstract)
        {
            this.isAbstractType = isAbstract;
            if (isAbstract)
            {
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + ABSTRACT_NAME));
            }
            else
            {
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, getExportTag() + ABSTRACT_NAME));
            }
        }

        public bool isAbstract()
        {
            return isAbstractType;
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

        public void setImplementedInterfaces(ISet<ITransition> implementedInterface, int removeCascadeDepth = 0)
        {
            implCapsule.setImplementedInterfaces(implementedInterface, removeCascadeDepth);
        }

        public void addImplementedInterface(ITransition implementedInterface)
        {
            implCapsule.addImplementedInterface(implementedInterface);
        }

        public void removeImplementedInterfaces(string id, int removeCascadeDepth = 0)
        {
            implCapsule.removeImplementedInterfaces(id, removeCascadeDepth);
        }

        public IDictionary<string, ITransition> getImplementedInterfaces()
        {
            return implCapsule.getImplementedInterfaces();
        }

        public List<ISimple2DVisualizationPathPoint> getSimple2DPathPoints()
        {
            return this.pathPoints;
        }

        public void addSimple2DPathPoint(ISimple2DVisualizationPathPoint point)
        {
            this.pathPoints.Add(point);
        }

        public double get2DPageRatio() { return has2DPageRatio; }
        public void set2DPageRatio(double has2DPageRatio)
        {
            if (has2DPageRatio >= 0)
            {
                this.has2DPageRatio = has2DPageRatio;
            }
            else
            {
                throw new ArgumentOutOfRangeException("has2DPageRatio", "Value must be a positive double or 0.");
            }
        }

        public double getRelative2DBeginX()
        {
            return hasRelative2D_BeginX;
        }

        public void setRelative2DBeginX(double relative2DBeginX)
        {
            if (relative2DBeginX >= 0 && relative2DBeginX <= 1)
            {
                hasRelative2D_BeginX = relative2DBeginX;
            }
            else
            {
                throw new ArgumentOutOfRangeException("relative2DBeginX", "Value must be between 0 and 1 (inclusive).");
            }
        }

        public double getRelative2DBeginY()
        {
            return hasRelative2D_BeginY;
        }

        public void setRelative2DBeginY(double relative2DBeginY)
        {
            if (relative2DBeginY >= 0 && relative2DBeginY <= 1)
            {
                hasRelative2D_BeginY = relative2DBeginY;
            }
            else
            {
                throw new ArgumentOutOfRangeException("relative2DBeginY", "Value must be between 0 and 1 (inclusive).");
            }
        }

        public double getRelative2DEndX()
        {
            return hasRelative2D_EndX;
        }

        public void setRelative2DEndX(double relative2DEndX)
        {
            if (relative2DEndX >= 0 && relative2DEndX <= 1)
            {
                hasRelative2D_EndX = relative2DEndX;
            }
            else
            {
                throw new ArgumentOutOfRangeException("relative2DEndX", "Value must be between 0 and 1 (inclusive).");
            }
        }

        public double getRelative2DEndY() { return hasRelative2D_EndY; }

        public void setRelative2DEndY(double relative2DEndY)
        {
            if (relative2DEndY >= 0 && relative2DEndY <= 1)
            {
                hasRelative2D_EndY = relative2DEndY;
            }
            else
            {
                throw new ArgumentOutOfRangeException("relative2DEndY", "Value must be between 0 and 1 (inclusive).");
            }
        }
    }
}
