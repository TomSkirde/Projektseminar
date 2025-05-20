using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    public class PayloadPhysicalObjectDescription : PayloadDescription, IPayloadPhysicalObjectDescription
    {
        private const string className = "PayloadPhysicalObjectDescription";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new PayloadPhysicalObjectDescription();
        }

        protected PayloadPhysicalObjectDescription() { }

        public PayloadPhysicalObjectDescription(IPASSProcessModel model, string labelForID = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(model, labelForID, comment, additionalLabel, additionalAttribute)
        { }
    }
}
