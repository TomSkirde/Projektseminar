using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a message type constraint
    /// </summary>

    public class MessageTypeConstraint : InputPoolConstraint, IMessageTypeConstraint
    {
        protected IMessageSpecification messageSpecification;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "MessageTypeConstraint";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new MessageTypeConstraint();
        }

        protected MessageTypeConstraint() { }

        /// <summary>
        /// Constructor that creates a new fully specified instance of the message type constraint class
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="inputPoolConstraintHandlingStrategy"></param>
        /// <param name="limit"></param>
        /// <param name="messageSpecification"></param>
        /// <param name="additionalAttribute"></param>
        public MessageTypeConstraint(IModelLayer layer, string labelForID = null, IInputPoolConstraintHandlingStrategy inputPoolConstraintHandlingStrategy = null,
            int limit = 0, IMessageSpecification messageSpecification = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, inputPoolConstraintHandlingStrategy, limit, comment, additionalLabel, additionalAttribute)
        {
            setReferencedMessageSpecification(messageSpecification);

        }


        public void setReferencedMessageSpecification(IMessageSpecification messageSpecification, int removeCascadeDepth = 0)
        {
            IMessageSpecification oldMessage = messageSpecification;
            // Might set it to null
            this.messageSpecification = messageSpecification;

            if (oldMessage != null)
            {
                if (oldMessage.Equals(messageSpecification)) return;
                oldMessage.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdReferences, oldMessage.getUriModelComponentID()));
            }

            if (!(messageSpecification is null))
            {
                publishElementAdded(messageSpecification);
                messageSpecification.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdReferences, messageSpecification.getUriModelComponentID()));
            }
        }


        public IMessageSpecification getReferencedMessageSpecification()
        {
            return messageSpecification;
        }



        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.references) && element is IMessageSpecification specification)
                {
                    setReferencedMessageSpecification(specification);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }



        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getReferencedMessageSpecification() != null)
                baseElements.Add(getReferencedMessageSpecification());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IMessageSpecification specification && specification.Equals(getReferencedMessageSpecification()))
                    setReferencedMessageSpecification(null, removeCascadeDepth);
            }
        }
    }
}
