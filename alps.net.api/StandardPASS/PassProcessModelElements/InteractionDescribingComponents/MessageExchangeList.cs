using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an message exchange list
    /// </summary>

    public class MessageExchangeList : InteractionDescribingComponent, IMessageExchangeList
    {
        protected ICompDict<string, IMessageExchange> messageExchanges = new CompDict<string, IMessageExchange>();

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "MessageExchangeList";

        private double has2DPageRatio = -1;
        private double hasRelative2D_BeginX = -1;
        private double hasRelative2D_BeginY = -1;
        private double hasRelative2D_EndX = -1;
        private double hasRelative2D_EndY = -1;
        private List<ISimple2DVisualizationPathPoint> pathPoints = new List<ISimple2DVisualizationPathPoint>();


        public double get2DPageRatio() { return has2DPageRatio; }
        public void set2DPageRatio(double has2DPageRatio)
        {
            if (has2DPageRatio > 0)
            {
                this.has2DPageRatio = has2DPageRatio;
            }
            if (has2DPageRatio == 0)
            {
                this.has2DPageRatio = 1;
                Log.Warning("found 2D page ratio of 0. This is impossible. changed it to 1");
            }
            else
            {
                this.has2DPageRatio = Math.Abs(has2DPageRatio);
                Log.Warning("found negative 2d page ratio. Changed it to positive value");
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
                if (relative2DBeginX < 0)
                {
                    hasRelative2D_BeginX = 0;
                    Log.Warning("Value for relative2DBeginX is smaller than 0. Setting it to 0.");
                }
                else if (relative2DBeginX > 1)
                {
                    hasRelative2D_BeginX = 1;
                    Log.Warning("Value for relative2DBeginX is larger than 1. Setting it to 1.");
                }
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
                if (relative2DBeginY < 0)
                {
                    hasRelative2D_BeginY = 0;
                    Log.Warning("Value for relative2DBeginY is smaller than 0. Setting it to 0.");
                }
                else if (relative2DBeginY > 1)
                {
                    hasRelative2D_BeginY = 1;
                    Log.Warning("Value for relative2DBeginY is larger than 1. Setting it to 1.");
                }
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
                if (relative2DEndX < 0)
                {
                    hasRelative2D_EndX = 0;
                    Log.Warning("Value for relative2DEndX is smaller than 0. Setting it to 0.");
                }
                else if (relative2DEndX > 1)
                {
                    hasRelative2D_EndX = 1;
                    Log.Warning("Value for relative2DEndX is larger than 1. Setting it to 1.");
                }
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
                if (relative2DEndY < 0)
                {
                    hasRelative2D_EndY = 0;
                    Log.Warning("Value for relative2DEndY is smaller than 0. Setting it to 0.");
                }
                else if (relative2DEndY > 1)
                {
                    hasRelative2D_EndY = 1;
                    Log.Warning("Value for relative2DEndY is larger than 1. Setting it to 1.");
                }
            }
        }

        public List<ISimple2DVisualizationPathPoint> getSimple2DPathPoints()
        {
            return this.pathPoints;
        }

        public void addSimple2DPathPoint(ISimple2DVisualizationPathPoint point)
        {
            this.pathPoints.Add(point);
        }


        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new MessageExchangeList();
        }

        protected MessageExchangeList() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="messageExchanges"></param>
        /// <param name="additionalAttribute"></param>
        public MessageExchangeList(IModelLayer layer, string labelForID = null, ISet<IMessageExchange> messageExchanges = null, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null) : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setContainsMessageExchanges(messageExchanges);

        }


        public void addContainsMessageExchange(IMessageExchange messageExchange)
        {
            if (messageExchange is null) { return; }
            if (messageExchanges.TryAdd(messageExchange.getModelComponentID(), messageExchange))
            {
                publishElementAdded(messageExchange);
                messageExchange.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, messageExchange.getUriModelComponentID()));
            }
        }


        public void setContainsMessageExchanges(ISet<IMessageExchange> messageExchanges, int removeCascadeDepth = 0)
        {
            foreach (IMessageExchange messageExchange in getMessageExchanges().Values)
            {
                removeMessageExchange(messageExchange.getModelComponentID(), removeCascadeDepth);
            }
            if (messageExchanges is null) return;
            foreach (IMessageExchange messageExchange in messageExchanges)
            {
                addContainsMessageExchange(messageExchange);
            }
        }

        public void removeMessageExchange(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (messageExchanges.TryGetValue(id, out IMessageExchange exchange))
            {
                messageExchanges.Remove(id);
                exchange.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, exchange.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IMessageExchange> getMessageExchanges()
        {
            return new Dictionary<string, IMessageExchange>(messageExchanges);
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            //CultureInfo customCulture = new CultureInfo("en-US");
            //customCulture.NumberFormat.NumberDecimalSeparator = ".";

            if (element != null)
            {
                if (predicate.Contains(OWLTags.contains) && element is IMessageExchange exchange)
                {
                    addContainsMessageExchange(exchange);
                    return true;
                }
                else if (element is ISimple2DVisualizationPathPoint point)
                {
                    if (this.pathPoints == null) this.pathPoints = new List<ISimple2DVisualizationPathPoint>();

                    this.pathPoints.Add(point);
                }
            }
            else if (predicate.Contains(OWLTags.abstrHas2DPageRatio))
            {
                set2DPageRatio(double.Parse(objectContent, customCulture));
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasRelative2D_BeginX))
            {
                setRelative2DBeginX(double.Parse(objectContent, customCulture));
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasRelative2D_BeginY))
            {
                setRelative2DBeginY(double.Parse(objectContent, customCulture));
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasRelative2D_EndY))
            {
                setRelative2DEndY(double.Parse(objectContent, customCulture));
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasRelative2D_EndX))
            {
                setRelative2DEndX(double.Parse(objectContent, customCulture));
                return true;
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (messageExchanges.ContainsKey(oldID))
            {
                IMessageExchange element = messageExchanges[oldID];
                messageExchanges.Remove(oldID);
                messageExchanges.Add(element.getModelComponentID(), element);
            }
            base.notifyModelComponentIDChanged(oldID, newID);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IMessageExchange exchange in getMessageExchanges().Values) baseElements.Add(exchange);
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
                    removeMessageExchange(exchange.getModelComponentID(), removeCascadeDepth);
                }
            }
        }
    }
}