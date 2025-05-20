
using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class Simple2DVisualizationPathPoint : Simple2DVisualizationPoint, ISimple2DVisualizationPathPoint
    {
        protected ISimple2DVisualizationPathPoint nextPoint, previousPoint;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "Simple2DVisualizationPathPoint";

        public Simple2DVisualizationPathPoint(string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { }

        public override string getClassName()
        {
            return className;
        }

        public ISimple2DVisualizationPathPoint getNextPathPoint()
        {
            return nextPoint;
        }

        public ISimple2DVisualizationPathPoint getPreviousPathPoint()
        {
            return previousPoint;
        }

        /// <summary>
        /// Sets the next path point in this chain
        /// Automatically updates the <i>previousPathPoint</i>-parameter of the next point
        /// </summary>
        /// <param name="point">the new next point</param>
        public void setNextPathPoint(ISimple2DVisualizationPathPoint point)
        {
            if (point is null || point.Equals(getNextPathPoint())) return;
            this.nextPoint = point;
            point.setPreviousPathPoint(this);
        }

        /// <summary>
        /// Sets the previous path point in this chain
        /// Automatically updates the <i>nextPathPoint</i>-parameter of the previous point
        /// </summary>
        /// <param name="point">the new previous point</param>
        public void setPreviousPathPoint(ISimple2DVisualizationPathPoint point)
        {
            if (point is null || point.Equals(getPreviousPathPoint())) return;
            this.previousPoint = point;
            point.setNextPathPoint(this);
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new Simple2DVisualizationPathPoint();
        }

        protected Simple2DVisualizationPathPoint() { }
    }
}
