using alps.net.api.parsing;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    class LayeredPASSProcessModel : PASSProcessModel, IALPSModelElement, ILayeredPASSProcessModel
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "LayeredPASSProcessModel";

        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new LayeredPASSProcessModel();
        }

        protected LayeredPASSProcessModel() { }
        /// <summary>
        /// Constructor that creates a new fully specified instance of the layered pass process modell class
        /// </summary>
        /// <param name="additionalAttribute"></param>
        /// <param name="modelComponentID"></param>
        /// <param name="modelComponentLabel"></param>
        /// <param name="comment"></param>
        /// <param name="messageExchange"></param>
        /// <param name="relationToModelComponent"></param>
        /// <param name="startSubject"></param>
        public LayeredPASSProcessModel(string baseURI, string labelForID = null, ISet<IMessageExchange> messageExchanges = null, ISet<ISubject> relationsToModelComponent = null,
            ISet<ISubject> startSubject = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(baseURI, labelForID, messageExchanges, relationsToModelComponent, startSubject, comment, additionalLabel, additionalAttribute)
        { }




    }

}

