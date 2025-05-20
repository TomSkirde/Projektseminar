using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a custom or external data type definition
    /// </summary>
    public class CustomOrExternalDataTypeDefinition : DataTypeDefinition, ICustomOrExternalDataTypeDefinition
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "CustomOrExternalDataTypeDefinition";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new CustomOrExternalDataTypeDefinition();
        }

        protected CustomOrExternalDataTypeDefinition() { }
        public CustomOrExternalDataTypeDefinition(IPASSProcessModel model, string labelForID = null, ISet<IDataObjectDefinition> dataObjectDefiniton = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, dataObjectDefiniton, comment, additionalLabel, additionalAttribute)
        { }

    }
}