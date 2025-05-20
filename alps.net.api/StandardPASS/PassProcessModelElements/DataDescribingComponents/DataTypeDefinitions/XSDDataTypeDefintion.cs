using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a XSD data type defintion
    /// </summary>
    public class XSDDataTypeDefintion : CustomOrExternalDataTypeDefinition, IXSDDataTypeDefintion
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "XSD-DataTypeDefinition";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new XSDDataTypeDefintion();
        }

        protected XSDDataTypeDefintion() { }
        public XSDDataTypeDefintion(IPASSProcessModel model, string labelForID = null, ISet<IDataObjectDefinition> dataObjectDefiniton = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, dataObjectDefiniton, comment, additionalLabel, additionalAttribute)
        { }


    }
}
