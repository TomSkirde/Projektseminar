using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a Data Type Definition
    /// </summary>
    public class DataTypeDefinition : DataDescribingComponent, IDataTypeDefinition
    {
        protected ICompDict<string, IDataObjectDefinition> dataObjectDefinitons = new CompDict<string, IDataObjectDefinition>();

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private static string className = "DataTypeDefinition";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new DataTypeDefinition();
        }

        protected DataTypeDefinition() { }
        public DataTypeDefinition(IPASSProcessModel model, string labelForID = null,
            ISet<IDataObjectDefinition> dataObjectDefiniton = null, string comment = null,
            string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setContainsDataObjectDefintions(dataObjectDefiniton);
        }



        public void setContainsDataObjectDefintions(ISet<IDataObjectDefinition> dataObjectDefinitons, int removeCascadeDepth = 0)
        {
            foreach (IDataObjectDefinition dataObjectDefinition in getDataObjectDefinitons().Values)
            {
                removeDataObjectDefiniton(dataObjectDefinition.getModelComponentID(), removeCascadeDepth);
            }
            if (dataObjectDefinitons is null) return;
            foreach (IDataObjectDefinition dataObjectDefinition in dataObjectDefinitons)
            {
                addContainsDataObjectDefintion(dataObjectDefinition);
            }
        }

        public void addContainsDataObjectDefintion(IDataObjectDefinition dataObjectDefiniton)
        {
            if (dataObjectDefiniton is null) { return; }
            if (dataObjectDefinitons.TryAdd(dataObjectDefiniton.getModelComponentID(), dataObjectDefiniton))
            {
                publishElementAdded(dataObjectDefiniton);
                dataObjectDefiniton.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, dataObjectDefiniton.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IDataObjectDefinition> getDataObjectDefinitons()
        {
            return new Dictionary<string, IDataObjectDefinition>(dataObjectDefinitons);
        }


        public void removeDataObjectDefiniton(string modelComponentID, int removeCascadeDepth = 0)
        {
            if (modelComponentID is null) return;
            if (dataObjectDefinitons.TryGetValue(modelComponentID, out IDataObjectDefinition definition))
            {
                dataObjectDefinitons.Remove(modelComponentID);
                definition.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContains, definition.getUriModelComponentID()));
            }
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.contains) && element is IDataObjectDefinition definition)
                {
                    addContainsDataObjectDefintion(definition);
                    return true;
                }

            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IDataObjectDefinition definition in getDataObjectDefinitons().Values)
                baseElements.Add(definition);
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IDataObjectDefinition definition)
                    removeDataObjectDefiniton(definition.getModelComponentID(), removeCascadeDepth);
            }
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (dataObjectDefinitons.ContainsKey(oldID))
            {
                IDataObjectDefinition element = dataObjectDefinitons[oldID];
                dataObjectDefinitons.Remove(oldID);
                dataObjectDefinitons.Add(element.getModelComponentID(), element);
            }
            base.notifyModelComponentIDChanged(oldID, newID);
        }

    }
}