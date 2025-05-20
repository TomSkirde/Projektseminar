using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a receive type
    /// </summary>
    public class ReceiveType : BehaviorDescribingComponent, IReceiveType
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "ReceiveType";

        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ReceiveType();
        }

        protected ReceiveType() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="additionalAttribute"></param>
        public ReceiveType(ISubjectBehavior behavior, string label, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, label, comment, additionalLabel, additionalAttribute) { }


    }
}