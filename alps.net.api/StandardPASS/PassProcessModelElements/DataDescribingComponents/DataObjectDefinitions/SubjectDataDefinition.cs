using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a subject data definition class
    /// </summary>
    public class SubjectDataDefinition : DataObjectDefinition, ISubjectDataDefinition
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SubjectDataDefinition";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SubjectDataDefinition();
        }

        protected SubjectDataDefinition() { }
        public SubjectDataDefinition(IPASSProcessModel model, string labelForID = null,
            IDataTypeDefinition dataTypeDefintion = null, string comment = null,
            string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, dataTypeDefintion, comment, additionalLabel, additionalAttribute)
        { }


    }
}

