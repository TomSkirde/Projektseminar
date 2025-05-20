using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace alps.net.api.StandardPASS
{

    /// <summary>
    /// Class that represents an message exchange
    /// </summary>

    public class MessageExchange : InteractionDescribingComponent, IMessageExchange
    {
        protected IMessageSpecification messageType;
        protected ISubject receiver;
        protected ISubject sender;
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "MessageExchange";
        private IMessageExchange.MessageExchangeType messageExchangeType = IMessageExchange.MessageExchangeType.StandardMessageExchange;

        protected bool isAbstractType = false;
        private const string ABSTRACT_NAME = "AbstractPASSMessageExchange";

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



        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new MessageExchange();
        }

        protected MessageExchange() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="messageSpecification"></param>
        /// <param name="senderSubject"></param>
        /// <param name="receiverSubject"></param>
        /// <param name="additionalAttribute"></param>
        public MessageExchange(IModelLayer layer, string labelForID = null, IMessageSpecification messageSpecification = null, ISubject senderSubject = null,
            ISubject receiverSubject = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setMessageType(messageSpecification);
            setReceiver(receiverSubject);
            setSender(senderSubject);
        }


        public void setMessageType(IMessageSpecification messageType, int removeCascadeDepth = 0)
        {
            IMessageSpecification oldSpecification = this.messageType;
            // Might set it to null
            this.messageType = messageType;

            if (oldSpecification != null)
            {
                if (oldSpecification.Equals(messageType)) return;
                oldSpecification.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasMessageType, oldSpecification.getUriModelComponentID()));
            }

            if (!(messageType is null))
            {
                publishElementAdded(messageType);
                messageType.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasMessageType, messageType.getUriModelComponentID()));
            }
        }


        public void setReceiver(ISubject receiver, int removeCascadeDepth = 0)
        {
            ISubject oldReceiver = this.receiver;
            // Might set it to null
            this.receiver = receiver;

            if (oldReceiver != null)
            {
                if (oldReceiver.Equals(receiver)) return;
                oldReceiver.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasReceiver, oldReceiver.getUriModelComponentID()));
            }

            if (!(receiver is null))
            {
                publishElementAdded(receiver);
                receiver.register(this);
                receiver.addIncomingMessageExchange(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasReceiver, receiver.getUriModelComponentID()));
            }
        }


        public void setSender(ISubject sender, int removeCascadeDepth = 0)
        {
            ISubject oldSender = this.sender;
            // Might set it to null
            this.sender = sender;

            if (oldSender != null)
            {
                if (oldSender.Equals(sender)) return;
                oldSender.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasSender, oldSender.getUriModelComponentID()));
            }

            if (!(sender is null))
            {
                publishElementAdded(sender);
                sender.register(this);
                sender.addOutgoingMessageExchange(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasSender, sender.getUriModelComponentID()));
            }
        }


        public IMessageSpecification getMessageType()
        {
            return messageType;
        }


        public ISubject getReceiver()
        {
            return receiver;
        }


        public ISubject getSender()
        {
            return sender;
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {


            if (element != null)
            {
                if (predicate.Contains(OWLTags.hasMessageType) && element is IMessageSpecification specification)
                {
                    setMessageType(specification);
                    return true;
                }

                //int i = getAdditionalAttribute().IndexOf(allElements[s].getModelComponentID());

                else if (element is ISubject subject)
                {
                    if (predicate.Contains(OWLTags.hasReceiver))
                    {
                        setReceiver(subject);
                        return true;
                    }
                    else if (predicate.Contains(OWLTags.hasSender))
                    {
                        setSender(subject);
                        return true;
                    }
                }
            }
            if (predicate.Contains(OWLTags.type))
            {
                // Console.WriteLine(" - parsing object content transition type: " + objectContent);

                if (objectContent.ToLower().Contains(IMessageExchange.MessageExchangeType.FinalizedMessageExchange.ToString().ToLower()))
                {
                    setMessageExchangeType(IMessageExchange.MessageExchangeType.FinalizedMessageExchange);
                    setIsAbstract(true);
                    return true;
                }
                else if (objectContent.ToLower().Contains(IMessageExchange.MessageExchangeType.AbstractMessageExchange.ToString().ToLower()))
                {
                    setMessageExchangeType(IMessageExchange.MessageExchangeType.AbstractMessageExchange);
                    setIsAbstract(true);
                    return true;
                }
                else if (objectContent.Contains(ABSTRACT_NAME))
                {
                    setIsAbstract(true);
                    return true;
                }


            }


            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (specification == ConnectedElementsSetSpecification.ALL || specification == ConnectedElementsSetSpecification.TO_ADD)
            {
                if (getReceiver() != null) baseElements.Add(getReceiver());
                if (getSender() != null) baseElements.Add(getSender());
                if (getMessageType() != null) baseElements.Add(getMessageType());
            }
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is ISubject subj)
                {
                    if (subj.Equals(getReceiver()))
                        setReceiver(null, removeCascadeDepth);
                    if (subj.Equals(getSender()))
                        setSender(null, removeCascadeDepth);
                }
                if (update is IMessageSpecification specification && specification.Equals(getMessageType()))
                    setMessageType(null, removeCascadeDepth);
            }
        }

        public void setMessageExchangeType(IMessageExchange.MessageExchangeType type)
        {
            this.messageExchangeType = type;
        }

        public IMessageExchange.MessageExchangeType getMessageExchangeType()
        {
            return this.messageExchangeType;
        }
    }
}
