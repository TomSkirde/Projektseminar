using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class StatePlaceholder : State, IStatePlaceholder
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string CLASS_NAME = "StatePlaceholder";


        public override string getClassName()
        {
            return CLASS_NAME;
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new StatePlaceholder();
        }

        protected StatePlaceholder() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="behavior"></param>
        /// <param name="labelForId"></param>
        /// <param name="guardBehavior"></param>
        /// <param name="functionSpecification"></param>
        /// <param name="incomingTransition"></param>
        /// <param name="outgoingTransition"></param>
        /// <param name="comment"></param>
        /// <param name="additionalLabel"></param>
        /// <param name="additionalAttribute"></param>
        public StatePlaceholder(ISubjectBehavior behavior, string labelForId = null, IGuardBehavior guardBehavior = null,
            IFunctionSpecification functionSpecification = null, ISet<ITransition> incomingTransition = null, ISet<ITransition> outgoingTransition = null,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(behavior, labelForId, guardBehavior, functionSpecification, incomingTransition, outgoingTransition, comment, additionalLabel, additionalAttribute)
        { }


    }
}
