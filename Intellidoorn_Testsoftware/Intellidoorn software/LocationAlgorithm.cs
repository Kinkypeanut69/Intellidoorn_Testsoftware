using Intellidoorn_software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Intellidoorn_software
{
    public class LocationAlgorithm
    {
        Stand closestStand;
        Stand finalLocation = null;
        List<TagInfo> strongestTags;
        int finalHeight = -65535;
        bool goodReading = false;


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
                int totalStandStrength = 0;
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

                    //CALCULATE WHAT TAG HAS THE HIGHEST SIGNAL STRENGTH

                    if (standOccurrence > 0)
                    {
                        int signalStrength = ReaderConnection.tags.FindAll(t => t.itemCode.Contains(tag)).Sum(t => t.signalStrength);
                        signalStrength = signalStrength + 2000;
                        totalStandStrength = totalStandStrength + signalStrength;
                        if(totalStandStrength > highestSignalStrength)
                        {
                            highestSignalStrength = totalStandStrength;
                            Console.WriteLine(totalStandStrength);
                            averageStand = s;
                        }
                    }
                }
                if (ReaderConnection.laserHeight >= s.baseHeight && ReaderConnection.laserHeight <= (s.baseHeight + s.height))
                {
                    finalHeight = s.row;
                    //Console.WriteLine(finalHeight);
                    goodReading = true;
                }
            }

            if (goodReading)
            {
                finalLocation = averageStand;
                if (finalLocation != null)
                    location = finalLocation.locationDescription + finalHeight;
                goodReading = false;
                return location;
            } else
                return "not valid";
        }

        public String getLocationOld()
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

                    //CALCULATE WHAT TAG HAS THE HIGHEST SIGNAL STRENGTH

                    if (standOccurrence > 0)
                    {
                        int signalStrength = ReaderConnection.tags.FindAll(t => t.itemCode.Contains(tag)).Sum(t => t.signalStrength);
                        signalStrength = signalStrength;
                        if (signalStrength > highestSignalStrength)
                        {
                            highestSignalStrength = signalStrength;
                            averageStand = s;
                        }
                    }
                }
                if (ReaderConnection.laserHeight >= s.baseHeight && ReaderConnection.laserHeight <= (s.baseHeight + s.height))
                {
                    finalHeight = s.row;
                    goodReading = true;
                }
            }

            if (goodReading)
            {

                if (maxCount != maxCount2)
                    finalLocation = maxStand;
                else
                    finalLocation = averageStand;
                
                if (finalLocation != null)
                    location = finalLocation.locationDescription + finalHeight;
                goodReading = false;
                return location;
            }
            else
                return "not valid";
        }

    }
}