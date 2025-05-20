using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an InputPoolConstraint
    /// </summary>

    public class InputPoolConstraint : InteractionDescribingComponent, IInputPoolConstraint
    {
        protected IInputPoolConstraintHandlingStrategy inputPoolConstraintHandlingStrategy;
        protected int limit = 0;
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "InputPoolConstraint";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new InputPoolConstraint();
        }

        protected InputPoolConstraint() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="additionalAttribute"></param>
        /// <param name="inputPoolConstraintHandlingStrategy"></param>
        /// <param name="limit"></param>
        public InputPoolConstraint(IModelLayer layer, string labelForID = null, IInputPoolConstraintHandlingStrategy inputPoolConstraintHandlingStrategy = null,
            int limit = 0, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setInputPoolConstraintHandlingStrategy(inputPoolConstraintHandlingStrategy);
            setLimit(limit);
        }


        public void setInputPoolConstraintHandlingStrategy(IInputPoolConstraintHandlingStrategy inputPoolConstraintHandlingStrategy, int removeCascadeDepth = 0)
        {
            IInputPoolConstraintHandlingStrategy oldStrat = this.inputPoolConstraintHandlingStrategy;
            // Might set it to null
            this.inputPoolConstraintHandlingStrategy = inputPoolConstraintHandlingStrategy;

            if (oldStrat != null)
            {
                if (oldStrat.Equals(inputPoolConstraintHandlingStrategy)) return;
                oldStrat.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasHandlingStrategy, oldStrat.getUriModelComponentID()));
            }

            if (!(inputPoolConstraintHandlingStrategy is null))
            {
                publishElementAdded(inputPoolConstraintHandlingStrategy);
                inputPoolConstraintHandlingStrategy.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasHandlingStrategy, inputPoolConstraintHandlingStrategy.getUriModelComponentID()));
            }
        }


        public void setLimit(int nonNegativInteger)
        {
            if (nonNegativInteger == getLimit()) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasLimit, this.limit.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypeNonNegativeInt)));
            limit = (nonNegativInteger >= 0) ? nonNegativInteger : 0;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasLimit, this.limit.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypeNonNegativeInt)));
        }


        public IInputPoolConstraintHandlingStrategy getInputPoolConstraintHandlingStrategy()
        {
            return inputPoolConstraintHandlingStrategy;
        }


        public int getLimit()
        {
            return limit;
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasLimit))
            {
                string limit = objectContent;
                setLimit(int.Parse(limit));
                return true;
            }
            else if (element != null)
            {
                if (predicate.Contains(OWLTags.hasHandlingStrategy) && element is IInputPoolConstraintHandlingStrategy strategy)
                {
                    setInputPoolConstraintHandlingStrategy(strategy);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getInputPoolConstraintHandlingStrategy() != null) baseElements.Add(getInputPoolConstraintHandlingStrategy());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IInputPoolConstraintHandlingStrategy strategy && strategy.Equals(getInputPoolConstraintHandlingStrategy()))
                    setInputPoolConstraintHandlingStrategy(null, removeCascadeDepth);
            }
        }

    }
}