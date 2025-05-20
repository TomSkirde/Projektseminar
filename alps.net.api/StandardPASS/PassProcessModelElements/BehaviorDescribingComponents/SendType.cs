using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a send type
    /// </summary>
    public class SendType : BehaviorDescribingComponent, ISendType
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SendType";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SendType();
        }

        protected SendType() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="additionalAttribute"></param>
        public SendType(ISubjectBehavior behavior, string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForID, comment, additionalLabel, additionalAttribute)
        { }


    }
}
