
using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace alps.net.api.ALPS
{
    public class Simple2DVisualizationPoint : ALPSModelElement, ISimple2DVisualizationPoint
    {
        private double has2DPageRatio = -1;
        private double hasRelative2D_PosX = -1;
        private double hasRelative2D_PosY = -1;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "Simple2DVisualizationPoint";

        public Simple2DVisualizationPoint(string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { }

        public override string getClassName()
        {
            return className;
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }

        public double getRelative2DPosX()
        {
            return this.hasRelative2D_PosX;
        }

        public double getRelative2DPosY()
        {
            return this.hasRelative2D_PosY;
        }

        public void setRelative2DPosX(double posx)
        {
            if (posx >= 0 && posx <= 1)
            {
                this.hasRelative2D_PosX = posx;
            }
            else
            {
                if (posx < 0)
                {
                    this.hasRelative2D_PosX = 0;
                    Log.Warning("Value for posx is smaller than 0. Setting it to 0.");
                }
                else if (posx > 1)
                {
                    this.hasRelative2D_PosX = 1;
                    Log.Warning("Value for posx is larger than 1. Setting it to 1.");
                }
            }

        }


        public void setRelative2DPosY(double posy)
        {
            if (posy >= 0 && posy <= 1)
            {
                this.hasRelative2D_PosY = posy;
            }
            else
            {
                if (posy < 0)
                {
                    this.hasRelative2D_PosY = 0;
                    Log.Warning("Value for posy is smaller than 0. Setting it to 0.");
                }
                else if (posy > 1)
                {
                    this.hasRelative2D_PosY = 1;
                    Log.Warning("Value for posy is larger than 1. Setting it to 1.");
                }
            }

        }


        public double getHas2DPageRatio() { return has2DPageRatio; }
        public void setHas2DPageRatio(double has2DPageRatio)
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

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new Simple2DVisualizationPoint();
        }

        protected Simple2DVisualizationPoint() { }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            //CultureInfo customCulture = new CultureInfo("en-US");
            //customCulture.NumberFormat.NumberDecimalSeparator = ".";


            if (predicate.Contains(OWLTags.abstrHas2DPageRatio))
            {
                setHas2DPageRatio(double.Parse(objectContent, customCulture));
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasRelative2D_PosX))
            {
                setRelative2DPosX(double.Parse(objectContent, customCulture));
                return true;
            }
            else if (predicate.Contains(OWLTags.abstrHasRelative2D_PosY))
            {
                setRelative2DPosY(double.Parse(objectContent, customCulture));
                return true;
            }

            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }
    }
}
