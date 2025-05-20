using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an message exchange conditon
    /// </summary>

    public class MessageExchangeCondition : TransitionCondition, IMessageExchangeCondition
    {
        protected IMessageExchange messageExchange;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "MessageExchangeCondition";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new MessageExchangeCondition();
        }

        protected MessageExchangeCondition() { }

        public MessageExchangeCondition(ITransition transition, string labelForID = null, string toolSpecificDefinition = null, IMessageExchange messageExchange = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(transition, labelForID, comment, toolSpecificDefinition, additionalLabel, additionalAttribute)
        {
            setRequiresPerformedMessageExchange(messageExchange);
        }


        public void setRequiresPerformedMessageExchange(IMessageExchange messageExchange, int removeCascadeDepth = 0)
        {
            IMessageExchange oldExchange = this.messageExchange;
            // Might set it to null
            this.messageExchange = messageExchange;

            if (oldExchange != null)
            {
                if (oldExchange.Equals(messageExchange)) return;
                oldExchange.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdRequiresPerformedMessageExchange, oldExchange.getUriModelComponentID()));
            }

            if (!(messageExchange is null))
            {
                publishElementAdded(messageExchange);
                messageExchange.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdRequiresPerformedMessageExchange, messageExchange.getUriModelComponentID()));
                /*if (messageExchange.getContainedBy(out IModelLayer layer)
                    setContainedBy(layer);*/
            }
        }


        public IMessageExchange getRequiresPerformedMessageExchange()
        {
            return messageExchange;
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.requiresPerformedMessageExchange) && element is IMessageExchange exchange)
                {
                    setRequiresPerformedMessageExchange(exchange);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getRequiresPerformedMessageExchange() != null)
                baseElements.Add(getRequiresPerformedMessageExchange());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IMessageExchange exchange && exchange.Equals(getRequiresPerformedMessageExchange())) setRequiresPerformedMessageExchange(null, removeCascadeDepth);
            }
        }
    }
}