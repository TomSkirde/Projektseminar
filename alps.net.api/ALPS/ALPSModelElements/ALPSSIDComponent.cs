using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// Class that represents an ALPS SID component
    /// From abstract pass ont:<br></br>
    /// An abstract SID Component is a model Component only used on model layers defined as abstract.
    /// It is used to specify a possible process or process elements that later can be used to implement others.
    /// </summary>
    public class ALPSSIDComponent : InteractionDescribingComponent, IALPSSIDComponent
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "ALPSSIDComponent";

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
            return new ALPSSIDComponent();
        }

        protected ALPSSIDComponent() { }

        /// <summary>
        /// Constructor that creates a new fully specified instance of the ALPSSIDComponent class
        /// </summary>
        /// <param name="additionalAttribute"></param>
        /// <param name="modelComponentID"></param>
        /// <param name="modelComponentLabel"></param>
        /// <param name="comment"></param>
        public ALPSSIDComponent(IModelLayer layer, string label = null, string comment = null,
            string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, label, comment, additionalLabel, additionalAttribute)
        { }

    }
}
