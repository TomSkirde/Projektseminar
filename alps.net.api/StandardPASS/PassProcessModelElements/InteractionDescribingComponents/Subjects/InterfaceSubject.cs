using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Xml;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an interface subject
    /// </summary>

    public class InterfaceSubject : Subject, IInterfaceSubject
    {
        protected IFullySpecifiedSubject referencedSubject;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "InterfaceSubject";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new InterfaceSubject();
        }

        protected InterfaceSubject() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="referencedSubject">If the InterfaceSubject is referencing another FullySpecifiedSubject, this can be passed here</param>
        /// <param name="comment"></param>
        /// <param name="incomingMessageExchange"></param>
        /// <param name="outgoingMessageExchange"></param>
        /// <param name="maxSubjectInstanceRestriction"></param>
        /// <param name="additionalAttribute"></param>
        /// <param name="fullySpecifiedSubject"></param>
        public InterfaceSubject(IModelLayer layer, string labelForID = null, ISet<IMessageExchange> incomingMessageExchange = null,
            ISet<IMessageExchange> outgoingMessageExchange = null, int maxSubjectInstanceRestriction = 1, IFullySpecifiedSubject referencedSubject = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, incomingMessageExchange, outgoingMessageExchange, maxSubjectInstanceRestriction, comment, additionalLabel, additionalAttribute)
        {
            setReferencedSubject(referencedSubject);
        }


        public void setReferencedSubject(IFullySpecifiedSubject fullySpecifiedSubject, int removeCascadeDepth = 0)
        {
            IFullySpecifiedSubject oldSubject = this.referencedSubject;
            // Might set it to null
            this.referencedSubject = fullySpecifiedSubject;

            if (oldSubject != null)
            {
                if (oldSubject.Equals(referencedSubject)) return;
                oldSubject.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdReferences, oldSubject.getUriModelComponentID()));
            }

            // Might set it to null
            this.referencedSubject = fullySpecifiedSubject;
            if (!(fullySpecifiedSubject is null))
            {
                publishElementAdded(fullySpecifiedSubject);
                fullySpecifiedSubject.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdReferences, fullySpecifiedSubject.getUriModelComponentID()));
            }
        }


        public IFullySpecifiedSubject getReferencedSubject()
        {
            return referencedSubject;
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.references) && element is IFullySpecifiedSubject subject)
                {
                    setReferencedSubject(subject);
                    return true;
                }
            }
            else if (predicate.Contains(OWLTags.abstrHasSimpleSimInterfaceSubjectResponseDefinition))
            {
                this.setSimpleSimInterfaceSubjectResponseDefinition(objectContent);
                return true;
            }

            //setSimpleSimInterfaceSubjectResponseDefinition
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getReferencedSubject() != null) baseElements.Add(getReferencedSubject());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is ISubject subj && subj.Equals(getReferencedSubject()))
                    setReferencedSubject(null, removeCascadeDepth);
            }
        }

        protected XmlNode simpleSimInterfaceSubjectResponseDefinition;
        public void setSimpleSimInterfaceSubjectResponseDefinition(string simpleSimInterfaceSubjectResponseDefinitionStringa)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(simpleSimInterfaceSubjectResponseDefinitionStringa);            
            this.simpleSimInterfaceSubjectResponseDefinition = xmlDoc;
        }

        public void setSimpleSimInterfaceSubjectResponseDefinition(XmlNode simpleSimInterfaceSubjectResponseDefinition)
        {
            this.simpleSimInterfaceSubjectResponseDefinition = simpleSimInterfaceSubjectResponseDefinition;
        }

        public XmlNode getSimpleSimInterfaceSubjectResponseDefinition()
        {
            return simpleSimInterfaceSubjectResponseDefinition;
        }
    }
}
