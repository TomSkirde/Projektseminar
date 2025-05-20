using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an InputConstraintHandlingStrategy
    /// </summary>

    public class InputPoolConstraintHandlingStrategy : InteractionDescribingComponent, IInputPoolConstraintHandlingStrategy
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "InputPoolContstraintHandlingStrategy";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new InputPoolConstraintHandlingStrategy();
        }

        protected InputPoolConstraintHandlingStrategy() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="labelForID">a string describing this element which is used to generate the unique model component id</param>
        /// <param name="comment"></param>
        /// <param name="additionalLabel"></param>
        /// <param name="additionalAttribute"></param>
        public InputPoolConstraintHandlingStrategy(IModelLayer layer, string labelForID = null, string comment = null,
            string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        { }


    }
}
