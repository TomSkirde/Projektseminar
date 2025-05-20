using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a single subject
    /// </summary>
    public class SingleSubject : Subject, ISingleSubject
    {

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SingleSubject";


        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SingleSubject();
        }

        protected SingleSubject() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="incomingMessageExchange"></param>
        /// <param name="outgoingMessageExchange"></param>
        /// <param name="maxSubjectInstanceRestriction"></param>
        /// <param name="additionalAttribute"></param>
        public SingleSubject(IModelLayer layer, string labelForID = null, ISet<IMessageExchange> incomingMessageExchange = null, ISet<IMessageExchange> outgoingMessageExchange = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null) : base(layer, labelForID, incomingMessageExchange,
                outgoingMessageExchange, 1, comment, additionalLabel, additionalAttribute)
        { }

        public new void setInstanceRestriction(int restriction)
        {
            base.setInstanceRestriction(1);
        }


    }
}