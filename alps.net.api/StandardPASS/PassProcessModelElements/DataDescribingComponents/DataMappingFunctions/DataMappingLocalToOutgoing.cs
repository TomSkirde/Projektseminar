using alps.net.api.parsing;
using alps.net.api.StandardPASS.PassProcessModelElements.DataDescribingComponents;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a data mapping local to outgoing
    /// </summary>
    public class DataMappingLocalToOutgoing : DataMappingFunction, IDataMappingLocalToOutgoing
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "DataMappingLocalToOutgoing";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new DataMappingLocalToOutgoing();
        }

        protected DataMappingLocalToOutgoing() { }

        public DataMappingLocalToOutgoing(IPASSProcessModel model, string labelForID = null, string dataMappingString = null, string feelExpression = null,
            string toolSpecificDefinition = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, dataMappingString, feelExpression, toolSpecificDefinition, comment, additionalLabel, additionalAttribute)
        { }


    }
}
