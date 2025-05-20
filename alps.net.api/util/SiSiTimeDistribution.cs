using alps.net.api.StandardPASS;
using Serilog;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace alps.net.api.util
{
    public class SisiTimeDistribution: ISiSiTimeDistribution
    {

        public double meanValue{ get; set; }
        
        public double standardDeviation{get; set;}

        public double maxValue { get; set;}

        public double minValue{ get; set; }

        public bool wellKnownDuration
        {
            get { return wellKnownDuration; }
            set { wellKnownDuration = value; }
        }

        public void AddDistributionToMe(ref ISiSiTimeDistribution otherDistro)
        {
            meanValue = meanValue + otherDistro.meanValue;
            standardDeviation = Math.Sqrt((standardDeviation * standardDeviation) + (otherDistro.standardDeviation * otherDistro.standardDeviation));
            maxValue = maxValue + otherDistro.maxValue;
            minValue = minValue + otherDistro.minValue;

            if (wellKnownDuration)
            {
                wellKnownDuration = otherDistro.wellKnownDuration;
            }
        }

        public void SubtractDurationFromMe(ref ISiSiTimeDistribution otherDistro)
        {
            meanValue = meanValue - otherDistro.meanValue;
            if (meanValue < 0)
            {
                meanValue = 0;
            }

            minValue = minValue - otherDistro.minValue;
            if (minValue < 0)
            {
                minValue = 0;
            }

            maxValue = maxValue - otherDistro.maxValue;
            if (maxValue < 0)
            {
                maxValue = 0;
            }
        }

        public ISiSiTimeDistribution SubtractDurationAndGiveResult(ref ISiSiTimeDistribution otherDistro)
        {
            SisiTimeDistribution result = new SisiTimeDistribution();
            result.meanValue = meanValue - otherDistro.meanValue;
            if (result.meanValue < 0)
            {
                result.meanValue = 0;
            }

            result.minValue = minValue - otherDistro.minValue;
            if (result.minValue < 0)
            {
                result.minValue = 0;
            }

            result.maxValue = maxValue - otherDistro.maxValue;
            if (result.maxValue < 0)
            {
                result.maxValue = 0;
            }
            return result;
        }

        public ISiSiTimeDistribution CombineDistributionWeighted(ref ISiSiTimeDistribution otherDistro, double otherDistroWeight)
        {
            SisiTimeDistribution result = new SisiTimeDistribution();
            result.meanValue = meanValue + (otherDistro.meanValue * otherDistroWeight);
            result.standardDeviation = Math.Sqrt((standardDeviation * standardDeviation) + (otherDistro.standardDeviation * otherDistroWeight * otherDistro.standardDeviation * otherDistroWeight));
            result.maxValue = maxValue + (otherDistro.maxValue * otherDistroWeight);
            result.minValue = minValue + (otherDistro.minValue * otherDistroWeight);
            result.wellKnownDuration = otherDistro.wellKnownDuration && wellKnownDuration;
            return result;
        }

        public void AddDistributionToMeWeighted(ref ISiSiTimeDistribution otherDistro, double otherDistroWeight)
        {
            meanValue = meanValue + (otherDistro.meanValue * otherDistroWeight);
            standardDeviation = Math.Sqrt((standardDeviation * standardDeviation) + (otherDistro.standardDeviation * otherDistroWeight * otherDistro.standardDeviation * otherDistroWeight));
            maxValue = maxValue + (otherDistro.maxValue * otherDistroWeight);
            minValue = minValue + (otherDistro.minValue * otherDistroWeight);

            if (wellKnownDuration)
            {
                wellKnownDuration = otherDistro.wellKnownDuration;
            }
        }

       

        public override string ToString()
        {
            string result = "SiSi Time Distro with - Mean Value: " + ConvertFractionsOfDayToHourFormat(meanValue);

            if (!(standardDeviation == 0))
            {
                result = result + " - Standard Deviation: " + ConvertFractionsOfDayToHourFormat(standardDeviation);
            }

            if (!(minValue == 0))
            { 
                result = result + " - Minimum time : " + ConvertFractionsOfDayToHourFormat(minValue);
            }

            if (!(maxValue == 0))
            {   
                result = result + " - Maximum time : " + ConvertFractionsOfDayToHourFormat(maxValue);
            }

            return result;
        }



        public void AverageOutWith(ref ISiSiTimeDistribution otherDuration)
        {
            meanValue = (meanValue + otherDuration.meanValue) / 2;
            standardDeviation = (standardDeviation + otherDuration.standardDeviation) / 2;

            if (!(maxValue > otherDuration.maxValue))
            {
                maxValue = otherDuration.maxValue;
            }

            if (!(minValue < otherDuration.minValue))
            {
                minValue = otherDuration.minValue;
            }
        }

        public void CopyValuesOf(ref ISiSiTimeDistribution otherDistribution)
        {
            //distributionType = otherDistribution.distributionType;
            meanValue = otherDistribution.meanValue;
            standardDeviation = otherDistribution.standardDeviation;
            minValue = otherDistribution.minValue;
            maxValue = otherDistribution.maxValue;
        }

        public ISiSiTimeDistribution MakeCopyOfMe()
        {
            SisiTimeDistribution result = new SisiTimeDistribution();
            result.meanValue = meanValue;
            result.standardDeviation = standardDeviation;
            result.minValue = minValue;
            result.maxValue = maxValue;

            return result;
        }

        private static string ConvertFractionsOfDayToHourFormat(double fractionsOfDay)
        {
            string result = "";

            long days = (long)Math.Floor(fractionsOfDay);
            if (days > 0)
            {
                result = result + days + " days, ";
            }

            int hours;
            fractionsOfDay = (fractionsOfDay - days) * 24;
            hours = (int)Math.Floor(fractionsOfDay);
            if (hours > 0)
            {
                result = result + hours + " hours, ";
            }

            int minutes;
            fractionsOfDay = (fractionsOfDay - hours) * 60;
            minutes = (int)Math.Floor(fractionsOfDay);
            if (minutes > 0)
            {
                result = result + minutes + " minutes, ";
            }

            int seconds;
            fractionsOfDay = (fractionsOfDay - minutes) * 60;
            seconds = (int)Math.Floor(fractionsOfDay);
            if (seconds > 0)
            {
                result = result + seconds + " seconds, ";
            }

            int millis;
            fractionsOfDay = (fractionsOfDay - seconds) * 1000;
            millis = (int)Math.Floor(fractionsOfDay);
            if (millis > 0)
            {
                result = result + millis + " millis, ";
            }

            if ((days + hours + minutes + seconds + millis) <= 0)
            {
                result = "0";
            }

            return result;
        }

        public static string ConvertFractionsOfDayToXSDDurationString(double fractionsOfDay)
        {
            string result = "P";
            int days = (int)fractionsOfDay;

            if (days > 0)
            {
                result = result + days + "D";
            }

            result = result + "T";

            int hours = (int)((fractionsOfDay - days) * 24);
            if (hours > 0)
            {
                result = result + hours + "H";
            }

            int minutes = (int)((fractionsOfDay - days - (hours / 24.0)) * 1440);
            if (minutes > 0)
            {
                result = result + minutes + "M";
            }

            double seconds = (fractionsOfDay - days - (hours / 24.0) - (minutes / 1440.0)) * 86400;
            if (seconds > 0)
            {
                result = result + seconds + "S";
            }

            if (days + hours + minutes + seconds <= 0)
            {
                result = "P0DT0H0M0S";
            }

            return result;
        }

        public static double ConvertXSDDurationStringToFractionsOfDay(string xsdDurationString)
        {

            //CultureInfo customCulture = new CultureInfo("en-US");
            //customCulture.NumberFormat.NumberDecimalSeparator = ".";

            // Check if the string starts with 'P' and ends with 'S'
            if (!xsdDurationString.StartsWith("P"))
            {
                Log.Warning("could not Parse the value " + xsdDurationString + " as valid XSD Duration");
                return 0.0;
            }

            // Remove the 'P' and 'S' from the string to extract the duration components
            string duration = xsdDurationString.Substring(1, xsdDurationString.Length - 1);

            double fractionsOfDay = 0;

            int daysIndex = duration.IndexOf("D");
            if (daysIndex >= 0)
            {
                int days = int.Parse(duration.Substring(0, daysIndex));
                fractionsOfDay = fractionsOfDay + days;
                duration = duration.Substring(daysIndex + 1);
            }
            //remove the T for Time that
            int indexOfT = duration.IndexOf("T");
            if (indexOfT >= 0)
            {
                duration = duration.Substring(indexOfT + 1);
            }

                int hoursIndex = duration.IndexOf("H");
            if (hoursIndex >= 0)
            {
                int hours = int.Parse(duration.Substring(0, hoursIndex));
                fractionsOfDay = fractionsOfDay + (hours / 24.0);
                duration = duration.Substring(hoursIndex + 1);
            }

            int minutesIndex = duration.IndexOf("M");
            if (minutesIndex >= 0)
            {
                int minutes = int.Parse(duration.Substring(0, minutesIndex));
                fractionsOfDay = fractionsOfDay + (minutes / 1440.0);
                duration = duration.Substring(minutesIndex + 1);
            }

            int secondsIndex = duration.IndexOf("S");
            if (secondsIndex > 0)
            {
                double seconds = double.Parse(duration.Substring(0, secondsIndex),PASSProcessModelElement.customCulture);
                fractionsOfDay = fractionsOfDay + (seconds / 86400.0);
            }

            return fractionsOfDay;
        }




    }
}
