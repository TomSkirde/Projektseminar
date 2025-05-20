using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System;
using System.Collections.Generic;
using static alps.net.api.StandardPASS.IReceiveTransitionCondition;

namespace alps.net.api.StandardPASS
{

    /// <summary>
    /// Class that represents a receive transition condition
    /// </summary>
    public class ReceiveTransitionCondition : MessageExchangeCondition, IReceiveTransitionCondition
    {
        /// <summary>
        /// Used to parse send types from this library to a new owl on export
        /// </summary>
        private readonly string[] receiveTypeOWLExportNames = {
            OWLTags.stdReceiveTypeStandard,
            OWLTags.stdReceiveTypeReceiveFromKnown,
            OWLTags.stdReceiveTypeReceiveFromAll
        };

        /// <summary>
        /// Used to parse send types from owl to this library
        /// </summary>
        private readonly string[] receiveTypeOWLNames = {
            OWLTags.receiveTypeStandard,
            OWLTags.receiveTypeReceiveFromKnown,
            OWLTags.receiveTypeReceiveFromAll
        };


        protected int lowerBound;
        protected int upperBound;
        protected ReceiveTypes receiveType;
        protected ISubject messageSentFromSubject;
        protected IMessageSpecification receptionOfMessage;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "ReceiveTransitionCondition";
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ReceiveTransitionCondition();
        }

        protected ReceiveTransitionCondition() { }

        public override string getClassName()
        {
            return className;
        }

        public ReceiveTransitionCondition(ITransition transition, string labelForID = null, string toolSpecificDefintion = null, IMessageExchange messageExchange = null,
            int upperBound = 0, int lowerBound = 0, ReceiveTypes receiveType = ReceiveTypes.STANDARD, ISubject requiredMessageSendFromSubject = null,
            IMessageSpecification requiresReceptionOfMessage = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(transition, labelForID, toolSpecificDefintion, messageExchange, comment, additionalLabel, additionalAttribute)
        {
            setMultipleReceiveLowerBound(lowerBound);
            setMultipleReceiveUpperBound(upperBound);
            setReceiveType(receiveType);
            setMessageSentFrom(requiredMessageSendFromSubject);
            setReceptionOfMessage(requiresReceptionOfMessage);

            // TODO specification, subject, exchange redundand
        }


        public void setMultipleReceiveLowerBound(int lowerBound)
        {
            if (this.lowerBound == lowerBound) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasMultiReceiveLowerBound, this.lowerBound.ToString(),
                new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            this.lowerBound = (lowerBound > 0) ? lowerBound : 1;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasMultiReceiveLowerBound, lowerBound.ToString(),
                new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }


        public void setMultipleReceiveUpperBound(int upperBound)
        {
            if (this.upperBound == upperBound) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasMultiReceiveUpperBound, this.upperBound.ToString(),
                new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            this.upperBound = (upperBound > 0) ? upperBound : 1;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasMultiReceiveUpperBound, upperBound.ToString(),
                new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }


        public void setReceiveType(ReceiveTypes receiveType)
        {
            ReceiveTypes oldType = this.receiveType;
            this.receiveType = receiveType;

            if (oldType.Equals(receiveType)) return;

            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasReceiveType, receiveTypeOWLExportNames[(int)oldType]));
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasReceiveType, receiveTypeOWLExportNames[(int)receiveType]));
        }


        public void setMessageSentFrom(ISubject subject, int removeCascadeDepth = 0)
        {
            ISubject oldSubj = this.messageSentFromSubject;
            // Might set it to null
            this.messageSentFromSubject = subject;

            if (oldSubj != null)
            {
                if (oldSubj.Equals(subject)) return;
                oldSubj.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdRequiresMessageSentFrom, oldSubj.getUriModelComponentID()));
            }

            if (!(subject is null))
            {
                publishElementAdded(subject);
                subject.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdRequiresMessageSentFrom, subject.getUriModelComponentID()));
            }
        }


        public void setReceptionOfMessage(IMessageSpecification messageSpecification, int removeCascadeDepth = 0)
        {
            IMessageSpecification oldSpec = this.receptionOfMessage;
            // Might set it to null
            this.receptionOfMessage = messageSpecification;

            if (oldSpec != null)
            {
                if (oldSpec.Equals(messageSpecification)) return;
                oldSpec.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdRequiresReceptionOfMessage, oldSpec.getUriModelComponentID()));
            }

            if (!(messageSpecification is null))
            {
                publishElementAdded(messageSpecification);
                messageSpecification.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdRequiresReceptionOfMessage, messageSpecification.getUriModelComponentID()));
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


        public ReceiveTypes getReceiveType()
        {
            return receiveType;
        }


        public ISubject getMessageSentFrom()
        {
            return messageSentFromSubject;
        }


        public IMessageSpecification getReceptionOfMessage()
        {
            return receptionOfMessage;
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasMultiReceiveLowerBound))
            {
                string tmpLowerBound = objectContent;
                setMultipleReceiveLowerBound(int.Parse(tmpLowerBound));
                return true;
            }
            else if (predicate.Contains(OWLTags.hasMultiReceiveUpperBound))
            {
                string tmpUpperBound = objectContent;
                setMultipleReceiveUpperBound(int.Parse(tmpUpperBound));
                return true;
            }
            else if (predicate.Contains(OWLTags.hasReceiveType))
            {
                foreach (int i in Enum.GetValues(typeof(ReceiveTypes)))
                {
                    if (objectContent.Contains(receiveTypeOWLNames[i]))
                    {
                        setReceiveType((ReceiveTypes)i);
                        return true;
                    }
                }
            }
            else if (element != null)
            {


                if (predicate.Contains(OWLTags.requiresMessageSentFrom) && element is ISubject subject)
                {
                    setMessageSentFrom(subject);
                    return true;
                }
                if (predicate.Contains(OWLTags.requiresReceptionOfMessage) && element is IMessageSpecification messageSpecification)
                {
                    setReceptionOfMessage(messageSpecification);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getMessageSentFrom() != null)
                baseElements.Add(getMessageSentFrom());
            if (getReceptionOfMessage() != null)
                baseElements.Add(getReceptionOfMessage());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is ISubject subj && subj.Equals(getMessageSentFrom())) setMessageSentFrom(null, removeCascadeDepth);
                if (update is IMessageSpecification specification && specification.Equals(getReceptionOfMessage())) setReceptionOfMessage(null, removeCascadeDepth);
            }
        }
    }
}