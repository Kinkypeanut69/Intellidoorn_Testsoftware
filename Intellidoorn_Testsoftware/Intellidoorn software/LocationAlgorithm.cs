using Intellidoorn_software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intellidoorn_software
{
    public class LocationAlgorithm
    {
        Stand closestStand;
        Stand finalLocation = null;
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
            Stand maxStand = null;

            int filteredCount = 0;
            int filteredCount2 = 0;
            Stand filteredStand = null;

            int average = 0;
            Stand averageStand = null;

            string location = "";

            foreach (Stand s in ReaderConnection.stands)
            {
                foreach (String tag in s.tags)
                {
                    string standCode = tag;

                    // FIND HIGHEST OCCURRENCE AMONGST ALL TAGS
                    int standOccurrence = ReaderConnection.tags.FindAll(t => t.itemCode.Contains(standCode)).Count();
                    if (standOccurrence > maxCount)
                    {
                        maxCount = standOccurrence;
                        maxStand = s;
                    }
                    else if (standOccurrence == maxCount)
                        maxCount2 = standOccurrence;

                    // FIND HIGHEST OCCURRENCE AMONGST THE TOP 5 TAGS
                    int filteredOccurrence = strongestTags.FindAll(t => t.itemCode.Contains(standCode)).Count();
                    if (filteredOccurrence > filteredCount)
                    {
                        filteredCount = filteredOccurrence;
                        filteredStand = s;
                    }
                    else if (filteredOccurrence == filteredCount)
                        filteredCount2 = filteredOccurrence;

                    //CALCULATE AVERAGE VALUES OF THE TOP 5 TAGS

                    if (standOccurrence > 0)
                    {
                        int tagAvg = ReaderConnection.tags.FindAll(t => t.itemCode.Contains(standCode)).Sum(t => t.signalStrength) / standOccurrence;
                        if (tagAvg > average)
                        {
                            average = tagAvg;
                            averageStand = s;
                        }
                    }
                }
                
            }

            if (maxCount != maxCount2)
                finalLocation = maxStand;
            else if (filteredCount != filteredCount2)
                finalLocation = filteredStand;
            else
                finalLocation = averageStand;
            if (finalLocation != null)
                location = finalLocation.locationDescription;
            return location;

        }
    }
}