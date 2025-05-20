using alps.net.api.ALPS;
using alps.net.api.FunctionalityCapsules;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a subject
    /// </summary>
    public class Subject : InteractionDescribingComponent, ISubject
    {

        // Needs StartSubject as type


        protected int instanceRestriction;
        protected readonly ICompDict<string, IMessageExchange> incomingExchange = new CompDict<string, IMessageExchange>();
        protected readonly ICompDict<string, IMessageExchange> outgoingExchange = new CompDict<string, IMessageExchange>();
        protected readonly IImplementsFunctionalityCapsule<ISubject> implCapsule;
        protected readonly IExtendsFunctionalityCapsule<ISubject> extendsCapsule;
        protected readonly IList<ISubject.Role> roles = new List<ISubject.Role>();
        protected bool isAbstractType = false;

        private const string ABSTRACT_NAME = "AbstractSubject";

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "Subject";

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
            if (has2DPageRatio >= 0)
            {
                this.has2DPageRatio = has2DPageRatio;
            }
            else
            {
                throw new ArgumentOutOfRangeException("has2DPageRatio", "Value must be a positive double or 0.");
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
                throw new ArgumentOutOfRangeException("relative2DHeight", "Value must be between 0 and 1 (inclusive).");
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
                throw new ArgumentOutOfRangeException("relative2DWidth", "Value must be between 0 and 1 (inclusive).");
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
                throw new ArgumentOutOfRangeException("relative2DPosX", "Value must be between 0 and 1 (inclusive).");
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
                throw new ArgumentOutOfRangeException("relative2DPosY", "Value must be between 0 and 1 (inclusive).");
            }
        }

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new Subject();
        }

        protected Subject()
        {
            implCapsule = new ImplementsFunctionalityCapsule<ISubject>(this);
            extendsCapsule = new ExtendsFunctionalityCapsule<ISubject>(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="incomingMessageExchange"></param>
        /// <param name="outgoingMessageExchange"></param>
        /// <param name="maxSubjectInstanceRestriction"></param>
        /// <param name="additionalAttribute"></param>
        public Subject(IModelLayer layer, string labelForID = null, ISet<IMessageExchange> incomingMessageExchange = null,
            ISet<IMessageExchange> outgoingMessageExchange = null, int maxSubjectInstanceRestriction = 1, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        {
            extendsCapsule = new ExtendsFunctionalityCapsule<ISubject>(this);
            implCapsule = new ImplementsFunctionalityCapsule<ISubject>(this);
            setIncomingMessageExchanges(incomingMessageExchange);
            setInstanceRestriction(maxSubjectInstanceRestriction);
            setOutgoingMessageExchanges(outgoingMessageExchange);
        }

        public int getInstanceRestriction() { return instanceRestriction; }

        public void setInstanceRestriction(int restriction)
        {
            if (restriction == this.instanceRestriction) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdhasInstanceRestriction, instanceRestriction.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypeNonNegativeInt)));
            this.instanceRestriction = (restriction >= 0) ? restriction : 0;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdhasInstanceRestriction, instanceRestriction.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypeNonNegativeInt)));
        }



        public void addIncomingMessageExchange(IMessageExchange exchange)
        {
            if (exchange is null) { return; }
            if (incomingExchange.TryAdd(exchange.getModelComponentID(), exchange))
            {
                publishElementAdded(exchange);
                exchange.register(this);
                exchange.setReceiver(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasIncomingMessageExchange, exchange.getUriModelComponentID()));
            }
        }

        public void addOutgoingMessageExchange(IMessageExchange exchange)
        {
            if (exchange is null) { return; }
            if (outgoingExchange.TryAdd(exchange.getModelComponentID(), exchange))
            {
                publishElementAdded(exchange);
                exchange.register(this);
                exchange.setSender(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasOutgoingMessageExchange, exchange.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IMessageExchange> getIncomingMessageExchanges()
        {
            return new Dictionary<string, IMessageExchange>(incomingExchange);
        }

        public IDictionary<string, IMessageExchange> getOutgoingMessageExchanges()
        {
            return new Dictionary<string, IMessageExchange>(outgoingExchange);
        }


        public void setIncomingMessageExchanges(ISet<IMessageExchange> exchanges, int removeCascadeDepth = 0)
        {
            foreach (IMessageExchange exchange in getIncomingMessageExchanges().Values)
            {
                removeIncomingMessageExchange(exchange.getModelComponentID(), removeCascadeDepth);
            }
            if (exchanges is null) return;
            foreach (IMessageExchange exchange in exchanges)
            {
                addIncomingMessageExchange(exchange);
            }
        }

        public void setOutgoingMessageExchanges(ISet<IMessageExchange> exchanges, int removeCascadeDepth = 0)
        {
            foreach (IMessageExchange exchange in getOutgoingMessageExchanges().Values)
            {
                removeOutgoingMessageExchange(exchange.getModelComponentID(), removeCascadeDepth);
            }
            if (exchanges is null) return;
            foreach (IMessageExchange exchange in exchanges)
            {
                addOutgoingMessageExchange(exchange);
            }
        }

        public void removeIncomingMessageExchange(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (incomingExchange.TryGetValue(id, out IMessageExchange exchange))
            {
                incomingExchange.Remove(id);
                exchange.unregister(this, removeCascadeDepth);
                exchange.setReceiver(null);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasIncomingMessageExchange, exchange.getUriModelComponentID()));
            }
        }

        public void removeOutgoingMessageExchange(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (outgoingExchange.TryGetValue(id, out IMessageExchange exchange))
            {
                outgoingExchange.Remove(id);
                exchange.unregister(this, removeCascadeDepth);
                exchange.setSender(null);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasOutgoingMessageExchange, exchange.getUriModelComponentID()));
            }
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (implCapsule != null && implCapsule.parseAttribute(predicate, objectContent, lang, dataType, element))
                return true;
            else if (extendsCapsule != null && extendsCapsule.parseAttribute(predicate, objectContent, lang, dataType, element))
                return true;
            else if (predicate.Contains(OWLTags.hasInstanceRestriction))
            {
                string restr = objectContent;
                setInstanceRestriction(int.Parse(restr));
                return true;
            }
            else if (predicate.Contains(OWLTags.type) && objectContent.Contains("StartSubject"))
            {
                assignRole(ISubject.Role.StartSubject);
                return true;
            }
            else if (element != null)
            {
                if (element is IMessageExchange messageExchange)
                {
                    if (predicate.Contains(OWLTags.hasIncomingMessageExchange))
                    {
                        addIncomingMessageExchange(messageExchange);
                        return true;
                    }
                    else if (predicate.Contains(OWLTags.hasOutgoingMessageExchange))
                    {
                        addOutgoingMessageExchange(messageExchange);
                        return true;
                    }
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
            else
            {
                if (predicate.Contains(OWLTags.type))
                {
                    if (objectContent.Contains(ABSTRACT_NAME))
                    {
                        setIsAbstract(true);
                        return true;
                    }
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public void assignRole(ISubject.Role role)
        {
            if (!roles.Contains(role))
            {
                roles.Add(role);
                if (role == ISubject.Role.StartSubject)
                {
                    addTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, OWLTags.stdStartSubject));
                    if (getContainedBy(out IModelLayer layer))
                    {
                        if (layer.getContainedBy(out IPASSProcessModel model))
                            model.addStartSubject(this);
                    }
                }
            }
        }

        public bool isRole(ISubject.Role role)
        {
            return roles.Contains(role);
        }

        public void removeRole(ISubject.Role role)
        {
            if (roles.Contains(role))
            {
                roles.Remove(role);
                if (role == ISubject.Role.StartSubject)
                {
                    removeTriple(new PASSTriple(getExportXmlName(), OWLTags.rdfType, OWLTags.stdStartSubject));
                    if (getContainedBy(out IModelLayer layer))
                    {
                        if (layer.getContainedBy(out IPASSProcessModel model))
                            model.removeStartSubject(getModelComponentID());
                    }
                }
            }
        }


        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (incomingExchange.ContainsKey(oldID))
            {
                IMessageExchange element = incomingExchange[oldID];
                incomingExchange.Remove(oldID);
                incomingExchange.Add(element.getModelComponentID(), element);
            }
            if (outgoingExchange.ContainsKey(oldID))
            {
                IMessageExchange element = outgoingExchange[oldID];
                outgoingExchange.Remove(oldID);
                outgoingExchange.Add(element.getModelComponentID(), element);
            }
            base.notifyModelComponentIDChanged(oldID, newID);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IMessageExchange exchange in getIncomingMessageExchanges().Values) baseElements.Add(exchange);
            foreach (IMessageExchange exchange in getOutgoingMessageExchanges().Values) baseElements.Add(exchange);
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IMessageExchange exchange)
                {
                    // Try to remove the incoming exchange
                    removeIncomingMessageExchange(exchange.getModelComponentID(), removeCascadeDepth);
                    // Try to remove the outgoing exchange
                    removeOutgoingMessageExchange(exchange.getModelComponentID(), removeCascadeDepth);
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

        public void setImplementedInterfaces(ISet<ISubject> implementedInterface, int removeCascadeDepth = 0)
        {
            implCapsule.setImplementedInterfaces(implementedInterface, removeCascadeDepth);
        }

        public void addImplementedInterface(ISubject implementedInterface)
        {
            implCapsule.addImplementedInterface(implementedInterface);
        }

        public void removeImplementedInterfaces(string id, int removeCascadeDepth = 0)
        {
            implCapsule.removeImplementedInterfaces(id, removeCascadeDepth);
        }

        public IDictionary<string, ISubject> getImplementedInterfaces()
        {
            return implCapsule.getImplementedInterfaces();
        }

        public void setExtendedElement(ISubject element)
        {
            extendsCapsule.setExtendedElement(element);
        }

        public void setExtendedElementID(string elementID)
        {
            extendsCapsule.setExtendedElementID(elementID);
        }

        public ISubject getExtendedElement()
        {
            return extendsCapsule.getExtendedElement();
        }

        public string getExtendedElementID()
        {
            return extendsCapsule.getExtendedElementID();
        }

        public bool isExtension()
        {
            return extendsCapsule.isExtension();
        }


    }
}