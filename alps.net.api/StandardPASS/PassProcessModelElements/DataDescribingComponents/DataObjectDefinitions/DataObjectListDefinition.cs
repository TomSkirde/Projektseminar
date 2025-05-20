using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a data object list definition
    /// </summary>
    public class DataObjectListDefinition : DataObjectDefinition, IDataObjectListDefiniton
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "DataObjectListDefintion";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new DataObjectListDefinition();
        }

        protected DataObjectListDefinition() { }
        public DataObjectListDefinition(IPASSProcessModel model, string labelForID = null,
            IDataTypeDefinition dataTypeDefintion = null, string comment = null,
            string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, dataTypeDefintion, comment, additionalLabel, additionalAttribute)
        { }
    }
}
