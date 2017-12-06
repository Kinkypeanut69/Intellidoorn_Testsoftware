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

            int highestSignalStrength = -65535;
            Stand averageStand = null;

            string location = "";

            foreach (Stand s in ReaderConnection.stands)
            {
                foreach (String tag in s.tags)
                {
                    
                    // FIND HIGHEST OCCURRENCE AMONGST ALL STANDS
                    int standOccurrence = ReaderConnection.tags.FindAll(t => t.itemCode.Contains(tag)).Count();
                    if (standOccurrence > maxCount)
                    {
                        maxCount = standOccurrence;
                        maxStand = s;
                    }
                    else if (standOccurrence == maxCount)
                        maxCount2 = standOccurrence;

                    //CALCULATE AVERAGE VALUES OF ALL TAGS

                    if (standOccurrence > 0)
                    {
                        int signalStrength = ReaderConnection.tags.FindAll(t => t.itemCode.Contains(tag)).Sum(t => t.signalStrength);
                        //Console.WriteLine(tagAvg);
                        if (signalStrength > highestSignalStrength)
                        {
                            highestSignalStrength = signalStrength;
                            averageStand = s;
                        }
                    }
                }
            }

            if (maxCount != maxCount2)
                finalLocation = maxStand;
           else 
                finalLocation = averageStand;
            if (finalLocation != null)
                location = finalLocation.locationDescription;
            return location;



        }
    }
}