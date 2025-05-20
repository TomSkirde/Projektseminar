using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class MacroExtension : SubjectExtension, IMacroExtension
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "MacroExtension";

        public override string getClassName()
        {
            return className;
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new MacroExtension();
        }

        protected MacroExtension() { }
        /// <summary>
        /// Constructor that creates a new fully specified instance of the subject extension class
        /// </summary>
        public MacroExtension(IModelLayer layer, string labelForID = null, ISubject extendedSubject = null, ISet<ISubjectBehavior> extensionBehavior = null,
             ISet<IMessageExchange> incomingMessageExchange = null, ISet<IMessageExchange> outgoingMessageExchange = null,
            int maxSubjectInstanceRestriction = 1, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, extendedSubject, extensionBehavior, incomingMessageExchange, outgoingMessageExchange, maxSubjectInstanceRestriction, comment, additionalLabel, additionalAttribute)
        {
        }
    }
}
