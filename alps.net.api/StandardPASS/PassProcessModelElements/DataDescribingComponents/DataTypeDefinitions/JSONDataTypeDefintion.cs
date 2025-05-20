using System.Collections.Generic;
using alps.net.api.parsing;
using alps.net.api.util;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an JSONDataTypeDefinition
    /// </summary>

    public class JSONDataTypeDefinition : CustomOrExternalDataTypeDefinition, IJSONDataTypeDefinition
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "JSONDataTypeDefinition";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new JSONDataTypeDefinition();
        }

        protected JSONDataTypeDefinition() { }
        public JSONDataTypeDefinition(IPASSProcessModel model, string labelForID = null, ISet<IDataObjectDefinition> dataObjectDefiniton = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, dataObjectDefiniton, comment, additionalLabel, additionalAttribute)
        { }

    }
}
