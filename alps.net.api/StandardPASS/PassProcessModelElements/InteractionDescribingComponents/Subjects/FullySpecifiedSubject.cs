using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using Serilog;
using System.Collections.Generic;
using System.Globalization;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an FullySpecifiedSubject
    /// </summary>

    public class FullySpecifiedSubject : Subject, IFullySpecifiedSubject
    {
        protected ISubjectBehavior subjectBaseBehavior;
        protected ICompDict<string, ISubjectBehavior> subjectBehaviors = new CompDict<string, ISubjectBehavior>();
        protected ISubjectDataDefinition subjectDataDefinition;
        protected ICompDict<string, IInputPoolConstraint> inputPoolConstraints = new CompDict<string, IInputPoolConstraint>();
        protected ISubjectExecutionMapping subjectExecutionMapping;
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "FullySpecifiedSubject";

        //simple simulation (sisi) related elements

        public double sisiExecutionCostPerHour { get; set; } = 0;
        public SimpleSimVSMSubjectTypes sisiVSMSubjectType { get; set; } = SimpleSimVSMSubjectTypes.Standard;
        public double sisiVSMInventory { get; set; } = 0;
        public double sisiVSMProcessQuantity { get; set; } = 0;
        public double sisiVSMQualityRate { get; set; } = 0;
        public double sisiVSMAvailability { get; set; } = 0;

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new FullySpecifiedSubject();
        }

        protected FullySpecifiedSubject() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="incomingMessageExchange"></param>
        /// <param name="outgoingMessageExchange"></param>
        /// <param name="maxSubjectInstanceRestriction"></param>
        /// <param name="additionalAttribute"></param>
        /// <param name="inputPoolConstraint"></param>
        /// <param name="subjectDataDefinition"></param>
        public FullySpecifiedSubject(IModelLayer layer, string labelForID = null, ISet<IMessageExchange> incomingMessageExchange = null,
            ISubjectBaseBehavior subjectBaseBehavior = null, ISet<ISubjectBehavior> subjectBehaviors = null,
            ISet<IMessageExchange> outgoingMessageExchange = null, int maxSubjectInstanceRestriction = 1, ISubjectDataDefinition subjectDataDefinition = null,
            ISet<IInputPoolConstraint> inputPoolConstraints = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, incomingMessageExchange, outgoingMessageExchange, maxSubjectInstanceRestriction, comment, additionalLabel, additionalAttribute)
        {
            setDataDefintion(subjectDataDefinition);
            setInputPoolConstraints(inputPoolConstraints);
            if (subjectBehaviors != null)
                setBehaviors(subjectBehaviors);

            if (subjectBaseBehavior is null)
            {
                string label = (this.getModelComponentLabelsAsStrings().Count > 0) ? this.getModelComponentLabelsAsStrings()[0] : getModelComponentID();
                setBaseBehavior(new SubjectBehavior(layer, "defaultBehavior", this, null, null, 0,
                    "This is the subject behavior to the fully specified subject: " + label));
            }
            else
                setBaseBehavior(subjectBaseBehavior);
        }


        public void setBaseBehavior(ISubjectBehavior subjectBaseBehavior, int removeCascadeDepth = 0)
        {
            ISubjectBehavior oldBehavior = this.subjectBaseBehavior;
            // Might set it to null
            this.subjectBaseBehavior = subjectBaseBehavior;

            if (oldBehavior != null)
            {
                if (oldBehavior.Equals(subjectBaseBehavior)) return;
                // We do only remove the triple for the old behavior, as it is still listed as normal behavior (just not as baseBehavior)
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContainsBaseBehavior, oldBehavior.getUriModelComponentID()));
            }

            if (!(subjectBaseBehavior is null))
            {
                // NOT registering and publishing because we call addBehavior (happens there)
                addBehavior(subjectBaseBehavior);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContainsBaseBehavior, subjectBaseBehavior.getUriModelComponentID()));
            }
        }



        public bool addBehavior(ISubjectBehavior behavior)
        {
            if (behavior is null) { return false; }
            if (subjectBehaviors.TryAdd(behavior.getModelComponentID(), behavior))
            {
                publishElementAdded(behavior);
                behavior.register(this);
                behavior.setSubject(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContainsBehavior, behavior.getUriModelComponentID()));
                return true;
            }
            return false;
        }



        public void setBehaviors(ISet<ISubjectBehavior> behaviors, int removeCascadeDepth = 0)
        {
            foreach (ISubjectBehavior behavior in this.getBehaviors().Values)
            {
                removeBehavior(behavior.getModelComponentID(), removeCascadeDepth);
            }
            if (behaviors is null) return;
            foreach (ISubjectBehavior behavior in behaviors)
            {
                addBehavior(behavior);
            }
        }

        public bool removeBehavior(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return false;
            if (subjectBehaviors.TryGetValue(id, out ISubjectBehavior behavior))
            {
                if (behavior.Equals(getSubjectBaseBehavior()))
                    setBaseBehavior(null, removeCascadeDepth);
                subjectBehaviors.Remove(id);
                behavior.unregister(this, removeCascadeDepth);
                behavior.setSubject(null);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContainsBehavior, behavior.getUriModelComponentID()));
                return true;
            }
            return false;
        }
        public IDictionary<string, ISubjectBehavior> getBehaviors()
        {
            return new Dictionary<string, ISubjectBehavior>(subjectBehaviors);
        }



        public void setDataDefintion(ISubjectDataDefinition subjectDataDefinition, int removeCascadeDepth = 0)
        {
            ISubjectDataDefinition oldDef = subjectDataDefinition;
            // Might set it to null
            this.subjectDataDefinition = subjectDataDefinition;

            if (oldDef != null)
            {
                if (oldDef.Equals(subjectDataDefinition)) return;
                oldDef.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataDefintion, oldDef.getUriModelComponentID()));
            }


            if (!(subjectDataDefinition is null))
            {
                publishElementAdded(subjectDataDefinition);
                subjectDataDefinition.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataDefintion, subjectDataDefinition.getUriModelComponentID()));
            }
        }



        public bool addInputPoolConstraint(IInputPoolConstraint constraint)
        {
            if (constraint is null) { return false; }
            if (inputPoolConstraints.TryAdd(constraint.getModelComponentID(), constraint))
            {
                publishElementAdded(constraint);
                constraint.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasInputPoolConstraint, constraint.getUriModelComponentID()));
                return true;
            }
            return false;
        }



        public void setInputPoolConstraints(ISet<IInputPoolConstraint> constraints, int removeCascadeDepth = 0)
        {
            foreach (IInputPoolConstraint constraint in this.getInputPoolConstraints().Values)
            {
                removeInputPoolConstraint(constraint.getModelComponentID(), removeCascadeDepth);
            }
            if (constraints is null) return;
            foreach (IInputPoolConstraint constraint in constraints)
            {
                addInputPoolConstraint(constraint);
            }
        }

        public bool removeInputPoolConstraint(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return false;
            if (inputPoolConstraints.TryGetValue(id, out IInputPoolConstraint constraint))
            {
                inputPoolConstraints.Remove(id);
                constraint.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasInputPoolConstraint, constraint.getUriModelComponentID()));
                return true;
            }
            return false;
        }
        public IDictionary<string, IInputPoolConstraint> getInputPoolConstraints()
        {
            return new Dictionary<string, IInputPoolConstraint>(inputPoolConstraints);
        }


        public ISubjectBehavior getSubjectBaseBehavior()
        {
            return subjectBaseBehavior;
        }


        public ISubjectDataDefinition getSubjectDataDefinition()
        {
            return subjectDataDefinition;
        }

        // TODO interface anpassen

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            //CultureInfo customCulture = new CultureInfo("en-US");
            //customCulture.NumberFormat.NumberDecimalSeparator = ".";

            if (element != null)
            {
                if (element is ISubjectBehavior subjectBehavior)
                {
                    if (predicate.Contains(OWLTags.containsBaseBehavior))
                    {
                        setBaseBehavior(subjectBehavior);
                        return true;
                    }

                    else if (predicate.Contains(OWLTags.containsBehavior))
                    {
                        addBehavior(subjectBehavior);
                        return true;
                    }
                }

                else if (predicate.Contains(OWLTags.hasDataDefintion) && element is ISubjectDataDefinition dataDefinition)
                {
                    setDataDefintion(dataDefinition);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasInputPoolConstraint) && element is IInputPoolConstraint poolConstraint)
                {
                    addInputPoolConstraint(poolConstraint);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasSubjectExecutionMapping) && element is ISubjectExecutionMapping mapping)
                {
                    setSubjectExecutionMapping(mapping);
                    return true;
                }

            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimExecutionCostPerHour))
            {
                try
                {
                    this.sisiExecutionCostPerHour = double.Parse(objectContent, customCulture);
                }
                catch (System.Exception e)
                {
                    Log.Warning("could not parse the value " + objectContent + " as valid double");
                }
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimVSMSubjectType))
            {
                this.sisiVSMSubjectType = parseSimpleSimVSMSubjectType(objectContent);
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimVSMAvailability))
            {
                try
                {
                    this.sisiVSMInventory = double.Parse(objectContent, customCulture);
                }
                catch (System.Exception e)
                {
                    Log.Warning("could not parse the value " + objectContent + " as valid double");
                }
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimiVSMProcessQuantity))
            {
                try
                {
                    this.sisiVSMProcessQuantity = double.Parse(objectContent, customCulture);
                }
                catch (System.Exception e)
                {
                    Log.Warning("could not parse the value " + objectContent + " as valid double");
                }
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimiVSMQualityRate))
            {
                try
                {
                    this.sisiVSMQualityRate = double.Parse(objectContent, customCulture);
                }
                catch (System.Exception e)
                {
                    Log.Warning("could not parse the value " + objectContent + " as valid double");
                }
                return true;
            }
            else if (predicate.Contains(OWLTags.hasExecutionMappingDefinition))
            {
                //System.Console.WriteLine("Found an Execution Mapping: " + objectContent);
                if (this.subjectExecutionMapping != null)
                {
                    string newlabel = "SubjectExecutionMappingOf" + this.modelComponentID;
                    string newID = newlabel;
                    ISubjectExecutionMapping newMappingObject =
                        new SubjectExecutionMapping(this.layer, newlabel, objectContent);
                    this.subjectExecutionMapping = newMappingObject;
                }
                return true;
            }


            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getSubjectBaseBehavior() != null)
                baseElements.Add(getSubjectBaseBehavior());

            foreach (ISubjectBehavior behavior in getBehaviors().Values)
                baseElements.Add(behavior);

            if (getSubjectDataDefinition() != null)
                baseElements.Add(getSubjectDataDefinition());

            foreach (IInputPoolConstraint constraint in getInputPoolConstraints().Values)
                baseElements.Add(constraint);
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is ISubjectBehavior behavior)
                {
                    if (behavior.Equals(getSubjectBaseBehavior()))
                        setBaseBehavior(null, removeCascadeDepth);
                    else removeBehavior(behavior.getModelComponentID(), removeCascadeDepth);
                }
                if (update is ISubjectDataDefinition def && def.Equals(getSubjectDataDefinition()))
                    setDataDefintion(null, removeCascadeDepth);
                if (update is IInputPoolConstraint constraint)
                    removeInputPoolConstraint(constraint.getModelComponentID(), removeCascadeDepth);
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
            if (inputPoolConstraints.ContainsKey(oldID))
            {
                IInputPoolConstraint element = inputPoolConstraints[oldID];
                inputPoolConstraints.Remove(oldID);
                inputPoolConstraints.Add(element.getModelComponentID(), element);
            }
            base.notifyModelComponentIDChanged(oldID, newID);
        }

        private static SimpleSimVSMSubjectTypes parseSimpleSimVSMSubjectType(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = "nothing correct";
            }

            if (value.ToLower().Contains("production"))
            {
                return SimpleSimVSMSubjectTypes.ProductionSubject;
            }
            else if (value.ToLower().Contains("storage"))
            {
                return SimpleSimVSMSubjectTypes.StorageSubject;
            }
            else
            {
                return SimpleSimVSMSubjectTypes.Standard;
            }
        }

        public ISubjectExecutionMapping getSubjectExecutionMapping()
        {
            return this.subjectExecutionMapping;
        }

        public void setSubjectExecutionMapping(ISubjectExecutionMapping subjectExecutionMapping)
        {
            this.subjectExecutionMapping = subjectExecutionMapping;
        }
    }

}
