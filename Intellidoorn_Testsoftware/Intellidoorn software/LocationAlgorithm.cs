using Intellidoorn_software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intellidoorn_Testsoftware
{
    public class LocationAlgorithm 
    {
        Stand closestStand;
        String finalLocation = "";
        List<TagInfo> strongestTags;

        internal Stand ClosestStand { get => closestStand; set => closestStand = value; }

        public String getLocation()
        {
            strongestTags = ReaderConnection.tags.OrderByDescending(t => t.signalStrength).ToList();
            if (strongestTags.Count > 5)
                {
                    strongestTags.RemoveRange(5, strongestTags.Count - 5);
                }
            int maxCount = 0;
            int maxCount2 = 0;
            string maxStand = "";

            int filteredCount = 0;
            int filteredCount2 = 0;
            string filteredStand = "";

            int average = 0;
            string averageStand = "";

            foreach (Stand s in ReaderConnection.stands)
            {
                string standCode = s.tagId.Substring(0, 6);

                // FIND HIGHEST OCCURRENCE AMONGST ALL TAGS
                int standOccurrence = ReaderConnection.tags.FindAll(t => t.itemCode.Contains(standCode)).Count();
                if (standOccurrence > maxCount)
                {
                    maxCount = standOccurrence;
                    maxStand = s.locationDescription;
                }
                else if (standOccurrence == maxCount)
                    maxCount2 = standOccurrence;

                // FIND HIGHEST OCCURRENCE AMONGST THE TOP 5 TAGS
                int filteredOccurrence = strongestTags.FindAll(t => t.itemCode.Contains(standCode)).Count();
                if (filteredOccurrence > filteredCount)
                {
                    filteredCount = filteredOccurrence;
                    filteredStand = s.locationDescription;
                }
                else if (filteredOccurrence == filteredCount)
                    filteredCount2 = filteredOccurrence;

                //CALCULATE AVERAGE VALUES OF THE TOP 5 TAGS
                int tagAvg = ReaderConnection.tags.FindAll(t => t.itemCode.Contains(standCode)).Sum(t => t.signalStrength) / standOccurrence;
                if (tagAvg > average)
                {
                    average = tagAvg;
                    averageStand = s.locationDescription;
                }
            }

            

            if (maxCount != maxCount2)
                finalLocation = maxStand;
            else if (filteredCount != filteredCount2)
                finalLocation = filteredStand;
            else
                finalLocation = averageStand;
            return finalLocation;
            
        }
}
}
