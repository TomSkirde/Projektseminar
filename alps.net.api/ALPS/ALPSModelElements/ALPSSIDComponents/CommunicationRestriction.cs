using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using Serilog;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// From abstract pass ont:<br></br>
    /// Communication Restriction is a concept on an abstract layer. 
    /// A Communication Restriction defines that no communication is allowed between the defined subjects
    /// </summary>
    public class CommunicationRestriction : ALPSSIDComponent, ICommunicationRestriction
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "CommunicationRestriction";
        private ISubject correspondentA;
        private ISubject correspondentB;

        private double has2DPageRatio = -1;
        private double hasRelative2D_BeginX = -1;
        private double hasRelative2D_BeginY = -1;
        private double hasRelative2D_EndX = -1;
        private double hasRelative2D_EndY = -1;
        private List<ISimple2DVisualizationPathPoint> pathPoints = new List<ISimple2DVisualizationPathPoint>();


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new CommunicationRestriction();
        }

        protected CommunicationRestriction() { }

        public CommunicationRestriction(IModelLayer layer, string labelForID = null,
            ISubject correspondentA = null, ISubject correspondentB = null,
            string comment = null, string additionalLabel = null,
            IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setCorrespondentA(correspondentA);
            setCorrespondentB(correspondentB);
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element is ISubject subj)
            {
                if (predicate.Contains(OWLTags.hasCorrespondent))
                {
                    if (correspondentA == null)
                    {
                        setCorrespondentA(subj);
                    }
                    else if (correspondentB == null)
                    {
                        setCorrespondentB(subj);
                    }
                }
            }
            else if (element is ISimple2DVisualizationPathPoint point)
            {
                //Console.WriteLine(this.getModelComponentID() + ": PathPoint:" + point.getModelComponentID());
                if (this.pathPoints == null) this.pathPoints = new List<ISimple2DVisualizationPathPoint>();

                this.pathPoints.Add(point);
            }

            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public void setCorrespondents(ISubject correspondentA, ISubject correspondentB, int removeCascadeDepth = 0)
        {
            setCorrespondentA(correspondentA, removeCascadeDepth);
            setCorrespondentB(correspondentB, removeCascadeDepth);
        }

        public void setCorrespondentA(ISubject correspondentA, int removeCascadeDepth = 0)
        {
            ISubject oldCorrespondentA = this.correspondentA;
            // Might set it to null
            this.correspondentA = correspondentA;

            if (oldCorrespondentA != null)
            {
                if (oldCorrespondentA.Equals(correspondentA)) return;
                oldCorrespondentA.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasCorrespondent, correspondentA.getUriModelComponentID()));
            }
            if (!(correspondentA is null))
            {
                publishElementAdded(correspondentA);
                correspondentA.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasCorrespondent, correspondentA.getUriModelComponentID()));
            }
        }

        public void setCorrespondentB(ISubject correspondentB, int removeCascadeDepth = 0)
        {
            ISubject oldCorrespondentB = this.correspondentB;
            // Might set it to null
            this.correspondentB = correspondentB;

            if (oldCorrespondentB != null)
            {
                if (oldCorrespondentB.Equals(correspondentB)) return;
                oldCorrespondentB.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasCorrespondent, correspondentB.getUriModelComponentID()));
            }
            if (!(correspondentB is null))
            {
                publishElementAdded(correspondentB);
                correspondentB.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.stdHasCorrespondent, correspondentB.getUriModelComponentID()));
            }
        }

        public ISubject getCorrespondentA()
        {
            return correspondentA;
        }

        public ISubject getCorrespondentB()
        {
            return correspondentB;
        }

        public Tuple<ISubject, ISubject> getCorrespondents()
        {
            return new Tuple<ISubject, ISubject>(correspondentA, correspondentB);
        }


        public double get2DPageRatio() { return has2DPageRatio; }
        public void set2DPageRatio(double has2DPageRatio)
        {
            if (has2DPageRatio > 0)
            {
                this.has2DPageRatio = has2DPageRatio;
            }
            if (has2DPageRatio == 0)
            {
                this.has2DPageRatio = 1;
                Log.Warning("found 2D page ratio of 0. This is impossible. changed it to 1");
            }
            else
            {
                this.has2DPageRatio = Math.Abs(has2DPageRatio);
                Log.Warning("found negative 2d page ratio. Changed it to positive value");
            }
        }

        public double getRelative2DBeginX()
        {
            return hasRelative2D_BeginX;
        }

        public void setRelative2DBeginX(double relative2DBeginX)
        {
            if (relative2DBeginX >= 0 && relative2DBeginX <= 1)
            {
                hasRelative2D_BeginX = relative2DBeginX;
            }
            else
            {
                if (relative2DBeginX < 0)
                {
                    hasRelative2D_BeginX = 0;
                    Log.Warning("Value for relative2DBeginX is smaller than 0. Setting it to 0.");
                }
                else if (relative2DBeginX > 1)
                {
                    hasRelative2D_BeginX = 1;
                    Log.Warning("Value for relative2DBeginX is larger than 1. Setting it to 1.");
                }
            }

        }

        public double getRelative2DBeginY()
        {
            return hasRelative2D_BeginY;
        }

        public void setRelative2DBeginY(double relative2DBeginY)
        {
            if (relative2DBeginY >= 0 && relative2DBeginY <= 1)
            {
                hasRelative2D_BeginY = relative2DBeginY;
            }
            else
            {
                if (relative2DBeginY < 0)
                {
                    hasRelative2D_BeginY = 0;
                    Log.Warning("Value for relative2DBeginY is smaller than 0. Setting it to 0.");
                }
                else if (relative2DBeginY > 1)
                {
                    hasRelative2D_BeginY = 1;
                    Log.Warning("Value for relative2DBeginY is larger than 1. Setting it to 1.");
                }
            }

        }

        public double getRelative2DEndX()
        {
            return hasRelative2D_EndX;
        }

        public void setRelative2DEndX(double relative2DEndX)
        {
            if (relative2DEndX >= 0 && relative2DEndX <= 1)
            {
                hasRelative2D_EndX = relative2DEndX;
            }
            else
            {
                if (relative2DEndX < 0)
                {
                    hasRelative2D_EndX = 0;
                    Log.Warning("Value for relative2DEndX is smaller than 0. Setting it to 0.");
                }
                else if (relative2DEndX > 1)
                {
                    hasRelative2D_EndX = 1;
                    Log.Warning("Value for relative2DEndX is larger than 1. Setting it to 1.");
                }
            }

        }

        public double getRelative2DEndY() { return hasRelative2D_EndY; }

        public void setRelative2DEndY(double relative2DEndY)
        {
            if (relative2DEndY >= 0 && relative2DEndY <= 1)
            {
                hasRelative2D_EndY = relative2DEndY;
            }
            else
            {
                if (relative2DEndY < 0)
                {
                    hasRelative2D_EndY = 0;
                    Log.Warning("Value for relative2DEndY is smaller than 0. Setting it to 0.");
                }
                else if (relative2DEndY > 1)
                {
                    hasRelative2D_EndY = 1;
                    Log.Warning("Value for relative2DEndY is larger than 1. Setting it to 1.");
                }
            }
        }

        public List<ISimple2DVisualizationPathPoint> getSimple2DPathPoints()
        {
            return this.pathPoints;
        }

        public void addSimple2DPathPoint(ISimple2DVisualizationPathPoint point)
        {
            this.pathPoints.Add(point);
        }
    }
}
