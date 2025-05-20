using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a data object definition
    /// </summary>
    public class DataObjectDefinition : DataDescribingComponent, IDataObjectDefinition
    {
        protected IDataTypeDefinition dataTypeDefintion;
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "DataObjectDefinition";



        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new DataObjectDefinition();
        }

        protected DataObjectDefinition() { }
        public DataObjectDefinition(IPASSProcessModel model, string labelForID = null,
            IDataTypeDefinition dataTypeDefintion = null, string comment = null,
            string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setDataTypeDefinition(dataTypeDefintion);
        }


        public void setDataTypeDefinition(IDataTypeDefinition dataTypeDefintion, int removeCascadeDepth = 0)
        {
            IDataTypeDefinition oldDef = dataTypeDefintion;
            // Might set it to null
            this.dataTypeDefintion = dataTypeDefintion;

            if (oldDef != null)
            {
                if (oldDef.Equals(dataTypeDefintion)) return;
                oldDef.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataType, oldDef.getUriModelComponentID()));
            }

            if (!(dataTypeDefintion is null))
            {
                publishElementAdded(dataTypeDefintion);
                dataTypeDefintion.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasDataType, dataTypeDefintion.getUriModelComponentID()));
            }
        }


        public IDataTypeDefinition getDataTypeDefinition()
        {
            return dataTypeDefintion;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.hasDataType) && element is IDataTypeDefinition definition)
                {
                    setDataTypeDefinition(definition);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }



        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getDataTypeDefinition() != null)
                baseElements.Add(getDataTypeDefinition());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IDataTypeDefinition definition && definition.Equals(getDataTypeDefinition()))
                    setDataTypeDefinition(null, removeCascadeDepth);
            }
        }

    }
}