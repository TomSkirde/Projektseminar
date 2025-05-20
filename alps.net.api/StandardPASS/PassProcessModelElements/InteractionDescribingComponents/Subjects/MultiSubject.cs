using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a multi subject
    /// </summary>
    public class MultiSubject : Subject, IMultiSubject
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "MultiSubject";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new MultiSubject();
        }

        protected MultiSubject() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="incomingMessageExchange"></param>
        /// <param name="outgoingMessageExchange"></param>
        /// <param name="maxSubjectInstanceRestriction"></param>
        /// <param name="additionalAttribute"></param>
        public MultiSubject(IModelLayer layer, string labelForID = null, ISet<IMessageExchange> incomingMessageExchange = null, ISet<IMessageExchange> outgoingMessageExchange = null,
            int maxSubjectInstanceRestriction = 2, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, incomingMessageExchange, outgoingMessageExchange, -1, comment, additionalLabel, additionalAttribute)
        {
            setInstanceRestriction(maxSubjectInstanceRestriction);
        }


        public new void setInstanceRestriction(int instanceRestriction)
        {
            if (instanceRestriction >= 2)
            {
                base.setInstanceRestriction(instanceRestriction);
            }
            else
            {
                base.setInstanceRestriction(2);
            }
        }


    }
}
