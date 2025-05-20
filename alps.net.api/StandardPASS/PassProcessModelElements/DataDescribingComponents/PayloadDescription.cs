using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a payload description
    /// </summary>
    public class PayloadDescription : DataDescribingComponent, IPayloadDescription
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "PayloadDescription";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new PayloadDescription();
        }

        protected PayloadDescription() { }

        /// <summary>
        /// Constructor that creates a new fully specified instance of the payload description class
        /// </summary>
        /// <param name="additionalAttribute"></param>
        /// <param name="modelComponentID"></param>
        /// <param name="modelComponentLabel"></param>
        /// <param name="comment"></param>
        public PayloadDescription(IPASSProcessModel model, string labelForID = null, string comment = null,
            string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, comment, additionalLabel, additionalAttribute)
        { }


    }
}