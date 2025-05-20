using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// From abstract pass ont: <br></br>
    /// An empty model component that may be the (empty) origin or target of a message exchange can later (in implementing layers) be substituted for any other subject 
    /// Idea if the place is supposed to be left empty
    /// </summary>
    public class ActorPlaceHolder : ALPSSIDComponent, IActorPlaceHolder
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "ActorPlaceHolder";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ActorPlaceHolder();
        }

        protected ActorPlaceHolder() { }
        public ActorPlaceHolder(IModelLayer layer, string labelForID = null, string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        { }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }

    }
}
