using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;
using System.Diagnostics;

namespace alps.net.api.ALPS
{
    public class StandaloneMacroSubject : Subject, IStandaloneMacroSubject
    {
        private IMacroBehavior macroBehavior;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "StandAloneMacroSubject";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new StandaloneMacroSubject();
        }

        protected StandaloneMacroSubject() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="incomingMessageExchange"></param>
        /// <param name="outgoingMessageExchange"></param>
        /// <param name="maxSubjectInstanceRestriction"></param>
        /// <param name="additionalAttribute"></param>
        /// <param name="inputPoolConstraint"></param>
        /// <param name="subjectDataDefinition"></param>
        public StandaloneMacroSubject(IModelLayer layer, string labelForID = null, ISet<IMessageExchange> incomingMessageExchange = null,
            IMacroBehavior macroBehavior = null, ISet<IMessageExchange> outgoingMessageExchange = null, int maxSubjectInstanceRestriction = 1,
            string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, incomingMessageExchange, outgoingMessageExchange, maxSubjectInstanceRestriction,
                comment, additionalLabel, additionalAttribute)
        {
            setBehavior(macroBehavior);
        }

        public void setBehavior(IMacroBehavior behavior, int removeCascadeDepth = 0)
        {
            IMacroBehavior oldDef = this.macroBehavior;
            // Might set it to null
            this.macroBehavior = behavior;

            if (oldDef != null)
            {
                if (oldDef.Equals(behavior)) return;
                oldDef.unregister(this, removeCascadeDepth);
                behavior.setSubject(null);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContainsBehavior, oldDef.getUriModelComponentID()));
            }

            if (behavior is null) return;

            publishElementAdded(behavior);
            behavior.register(this);
            behavior.setSubject(this);
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdContainsBehavior, behavior.getUriModelComponentID()));
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            /*
            if (predicate.Contains(OWLTags.containsBehavior))
            {
                Debug.Print("Parsing Attribute of Stand laone macro: " + predicate.ToString());
                if (element != null)
                {
                    Debug.Print("Element Type: " + element.GetType().ToString() + " - is macroB: " + (element is IMacroBehavior));
                }
            }*/

            if (element is IMacroBehavior subjectBehavior && predicate.Contains(OWLTags.containsBehavior))
            {
                setBehavior(subjectBehavior);
                return true;
            }

            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public IMacroBehavior getBehavior()
        {
            return macroBehavior;
        }
    }
}
