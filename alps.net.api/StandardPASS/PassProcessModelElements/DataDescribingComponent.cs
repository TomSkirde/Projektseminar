using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a data describing component
    /// </summary>
    public class DataDescribingComponent : PASSProcessModelElement, IDataDescribingComponent
    {
        protected IPASSProcessModel model;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "DataDescribingComponent";

        public override string getClassName()
        {
            return className;
        }

        public void setContainedBy(IPASSProcessModel model)
        {
            if (model != null && model.Equals(this.model)) return;
            this.model = model;
            if (model != null)
            {
                model.addElement(this);
            }
        }


        public bool getContainedBy(out IPASSProcessModel model)
        {
            model = this.model;
            return this.model != null;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new DataDescribingComponent();
        }

        protected DataDescribingComponent() { }
        /// <summary>
        /// Constructor that creates a new fully specified instance of the data describing component class
        /// </summary>
        /// <param name="additionalAttribute"></param>
        /// <param name="modelComponentID"></param>
        /// <param name="modelComponentLabel"></param>
        /// <param name="comment"></param>
        public DataDescribingComponent(IPASSProcessModel model, string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute)
        { setContainedBy(model); }


        protected override IDictionary<string, IParseablePASSProcessModelElement> getDictionaryOfAllAvailableElements()
        {
            if (model is null) return null;
            IDictionary<string, IPASSProcessModelElement> allElements = model.getAllElements();
            IDictionary<string, IParseablePASSProcessModelElement> allParseableElements = new Dictionary<string, IParseablePASSProcessModelElement>();
            foreach (KeyValuePair<string, IPASSProcessModelElement> pair in allElements)
                if (pair.Value is IParseablePASSProcessModelElement parseable) allParseableElements.Add(pair.Key, parseable);
            return allParseableElements;
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (specification == ConnectedElementsSetSpecification.ALL)
                if (getContainedBy(out IPASSProcessModel model)) baseElements.Add(model);
            return baseElements;
        }

        public void removeFromContainer()
        {
            if (model != null)
                model.removeElement(getModelComponentID());
            model = null;
        }
    }
}
