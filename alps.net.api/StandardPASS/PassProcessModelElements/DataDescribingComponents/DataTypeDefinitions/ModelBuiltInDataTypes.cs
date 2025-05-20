using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a model built in data type
    /// </summary>
    public class ModelBuiltInDataTypes : DataTypeDefinition, IModelBuiltInDataTypes
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "ModelBuiltInDataTypes";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ModelBuiltInDataTypes();
        }

        protected ModelBuiltInDataTypes() { }
        public ModelBuiltInDataTypes(IPASSProcessModel model, string labelForID = null, ISet<IDataObjectDefinition> dataObjectDefiniton = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, dataObjectDefiniton, comment, additionalLabel, additionalAttribute)
        { }

    }
}
