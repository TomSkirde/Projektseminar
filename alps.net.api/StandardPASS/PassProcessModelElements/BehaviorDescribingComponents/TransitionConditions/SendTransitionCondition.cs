using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System;
using System.Collections.Generic;
using static alps.net.api.StandardPASS.ISendTransitionCondition;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a send transition condition
    /// </summary>
    public class SendTransitionCondition : MessageExchangeCondition, ISendTransitionCondition
    {


        /// <summary>
        /// Used to parse send types from this library to a new owl on export
        /// </summary>
        private readonly string[] sendTypeOWLExportNames = {
            OWLTags.stdSendTypeStandard,
            OWLTags.stdSendTypeSendToNew,
            OWLTags.stdSendTypeSendToKnown,
            OWLTags.stdSendTypeSendToAll
        };

        /// <summary>
        /// Used to parse send types from owl to this library
        /// </summary>
        private readonly string[] sendTypeOWLNames = {
            OWLTags.sendTypeStandard,
            OWLTags.sendTypeSendToNew,
            OWLTags.sendTypeSendToKnown,
            OWLTags.sendTypeSendToAll
        };

        protected int lowerBound;
        protected int upperBound;
        protected SendTypes sendType;
        protected ISubject messageSentTo;
        protected IMessageSpecification messageSpecification;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SendTransitionCondition";

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SendTransitionCondition();
        }

        protected SendTransitionCondition() { }
        public override string getClassName()
        {
            return className;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="toolSpecificDefintion"></param>
        /// <param name="messageExchange"></param>
        /// <param name="upperBound"></param>
        /// <param name="lowerBound"></param>
        /// <param name="sendType"></param>
        /// <param name="requiredMessageSendToSubject"></param>
        /// <param name="requiresSendingOfMessage"></param>
        /// <param name="additionalAttribute"></param>
        public SendTransitionCondition(ITransition transition, string labelForID = null, string toolSpecificDefintion = null,
            IMessageExchange messageExchange = null, int upperBound = 0, int lowerBound = 0, SendTypes sendType = SendTypes.STANDARD,
            ISubject messageSentFromSubject = null, IMessageSpecification receptionOfMessage = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(transition, labelForID, toolSpecificDefintion, messageExchange, comment, additionalLabel, additionalAttribute)
        {
            setMultipleSendLowerBound(lowerBound);
            setMultipleSendUpperBound(upperBound);
            setSendType(sendType);
            setRequiresMessageSentTo(messageSentFromSubject);
            setRequiresSendingOfMessage(receptionOfMessage);
        }


        public void setMultipleSendLowerBound(int lowerBound)
        {
            if (lowerBound == this.lowerBound) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasMultiSendLowerBound, this.lowerBound.ToString(),
                new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            this.lowerBound = (lowerBound > 0) ? lowerBound : 1;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasMultiSendLowerBound, lowerBound.ToString(),
                new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));

        }


        public void setMultipleSendUpperBound(int upperBound)
        {
            if (upperBound == this.upperBound) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasMultiSendUpperBound, this.upperBound.ToString(),
                new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            this.upperBound = (upperBound > 0) ? upperBound : 1;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasMultiSendUpperBound, upperBound.ToString(),
                new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }


        public void setSendType(SendTypes sendType)
        {
            SendTypes oldType = this.sendType;
            this.sendType = sendType;

            if (oldType.Equals(sendType)) return;

            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasSendType, sendTypeOWLExportNames[(int)oldType]));
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasSendType, sendTypeOWLExportNames[(int)sendType]));

        }


        public void setRequiresMessageSentTo(ISubject subject, int removeCascadeDepth = 0)
        {
            ISubject oldSubj = this.messageSentTo;
            // Might set it to null
            this.messageSentTo = subject;

            if (oldSubj != null)
            {
                if (oldSubj.Equals(subject)) return;
                oldSubj.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdRequiresMessageSentTo, oldSubj.getUriModelComponentID()));
            }

            if (!(subject is null))
            {
                publishElementAdded(subject);
                subject.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdRequiresMessageSentTo, subject.getUriModelComponentID()));
            }
        }


        public void setRequiresSendingOfMessage(IMessageSpecification messageSpecification, int removeCascadeDepth = 0)
        {
            IMessageSpecification oldSpec = this.messageSpecification;
            // Might set it to null
            this.messageSpecification = messageSpecification;

            if (oldSpec != null)
            {
                if (oldSpec.Equals(messageSpecification)) return;
                oldSpec.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdRequiresSendingOfMessage, oldSpec.getUriModelComponentID()));
            }

            if (!(messageSpecification is null))
            {
                publishElementAdded(messageSpecification);
                messageSpecification.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdRequiresSendingOfMessage, messageSpecification.getUriModelComponentID()));
            }
        }



        public int getMultipleLowerBound()
        {
            return lowerBound;
        }


        public int getMultipleUpperBound()
        {
            return upperBound;
        }


        public SendTypes getSendType()
        {
            return sendType;
        }


        public ISubject getRequiresMessageSentTo()
        {
            return messageSentTo;
        }


        public IMessageSpecification getRequiresSendingOfMessage()
        {
            return messageSpecification;
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getRequiresMessageSentTo() != null)
                baseElements.Add(getRequiresMessageSentTo());
            if (getRequiresSendingOfMessage() != null)
                baseElements.Add(getRequiresSendingOfMessage());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is ISubject subj && subj.Equals(getRequiresMessageSentTo())) setRequiresMessageSentTo(null, removeCascadeDepth);
                if (update is IMessageSpecification specification && specification.Equals(getRequiresSendingOfMessage())) setRequiresSendingOfMessage(null, removeCascadeDepth);
            }
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasMultiSendLowerBound))
            {
                string lower = objectContent;
                setMultipleSendLowerBound(int.Parse(lower));
                return true;
            }

            else if (predicate.Contains(OWLTags.hasMultiSendUpperBound))
            {
                string upper = objectContent;
                setMultipleSendUpperBound(int.Parse(upper));
                return true;
            }
            else if (predicate.Contains(OWLTags.hasSendType))
            {
                foreach (int i in Enum.GetValues(typeof(SendTypes)))
                {
                    if (objectContent.Contains(sendTypeOWLNames[i]))
                    {
                        setSendType((SendTypes)i);
                        return true;
                    }
                }


            }
            else if (element != null)
            {
                if (predicate.Contains(OWLTags.requiresMessageSentTo) && element is ISubject subject)
                {
                    setRequiresMessageSentTo(subject);
                    return true;
                }

                if (predicate.Contains(OWLTags.requiresSendingOfMessage) && element is IMessageSpecification messageSpecification)
                {
                    setRequiresSendingOfMessage(messageSpecification);
                    return true;
                }


            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }



    }
}