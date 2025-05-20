using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// Class that represents an ALPS SBD component
    /// </summary>
    public class ALPSSBDComponent : BehaviorDescribingComponent, IALPSSBDComponent
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "ALPSSBDComponent";
        public override string getClassName()
        {
            return className;
        }


        public ALPSSBDComponent(ISubjectBehavior subjectBehavior, string labelForID = null, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(subjectBehavior, labelForID, comment, additionalLabel, additionalAttribute)
        { }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ALPSSBDComponent();
        }

        protected ALPSSBDComponent() { }


        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }

    }
}
