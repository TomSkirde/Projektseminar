using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{

    /// <summary>
    /// Class that represents a payload data object definition
    /// </summary>
    public class PayloadDataObjectDefinition : DataObjectDefinition, IPayloadDataObjectDefinition
    {
        private const string className = "PayloadDataObjectDefinition";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new PayloadDataObjectDefinition();
        }

        protected PayloadDataObjectDefinition() { }

        public PayloadDataObjectDefinition(IPASSProcessModel model, string labelForID = null,
            IDataTypeDefinition dataTypeDefintion = null, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, dataTypeDefintion, comment, additionalLabel, additionalAttribute)
        { }


    }

}
