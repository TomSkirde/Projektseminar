using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// Class that represents an ALPS model element
    /// </summary>
    public class ALPSModelElement : PASSProcessModelElement, IALPSModelElement
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "ALPSModelElement";
        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ALPSModelElement();
        }

        protected ALPSModelElement() { }
        public ALPSModelElement(string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
    }
}
