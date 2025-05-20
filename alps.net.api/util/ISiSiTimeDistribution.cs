using System;
using System.Collections.Generic;
using System.Text;

namespace alps.net.api.util
{
    public interface ISiSiTimeDistribution
    {
        double meanValue { get; set; }
        double standardDeviation { get; set; }
        double maxValue { get; set; }
        double minValue { get; set; }
        bool wellKnownDuration { get; set; }

        void AddDistributionToMe(ref ISiSiTimeDistribution otherDistro);
        void SubtractDurationFromMe(ref ISiSiTimeDistribution otherDistro);
        ISiSiTimeDistribution SubtractDurationAndGiveResult(ref ISiSiTimeDistribution otherDistro);
        ISiSiTimeDistribution CombineDistributionWeighted(ref ISiSiTimeDistribution otherDistro, double otherDistroWeight);
        void AddDistributionToMeWeighted(ref ISiSiTimeDistribution otherDistro, double otherDistroWeight);
        //void ParseStateOrTransition(ref Visio.Shape inputShape);
        void AverageOutWith(ref ISiSiTimeDistribution otherDuration);
        void CopyValuesOf(ref ISiSiTimeDistribution otherDistribution);
        ISiSiTimeDistribution MakeCopyOfMe();
    }
}
