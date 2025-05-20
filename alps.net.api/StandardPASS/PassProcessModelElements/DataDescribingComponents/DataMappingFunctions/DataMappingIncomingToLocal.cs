using alps.net.api.parsing;
using alps.net.api.StandardPASS.PassProcessModelElements.DataDescribingComponents;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a data mapping incoming to local
    /// </summary>
    public class DataMappingIncomingToLocal : DataMappingFunction, IDataMappingIncomingToLocal
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "DataMappingIncomingToLocal";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new DataMappingIncomingToLocal();
        }

        protected DataMappingIncomingToLocal() { }
        public DataMappingIncomingToLocal(IPASSProcessModel model, string labelForID = null, string dataMappingString = null, string feelExpression = null,
            string toolSpecificDefinition = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, dataMappingString, feelExpression, toolSpecificDefinition, comment, additionalLabel, additionalAttribute)
        { }

    }

}
