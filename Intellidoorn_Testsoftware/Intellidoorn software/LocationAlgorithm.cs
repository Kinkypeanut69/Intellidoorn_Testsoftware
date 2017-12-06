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
        //Serial s1 = new Serial();
        Stand closestStand;
        Stand finalLocation = null;
        List<TagInfo> strongestTags;
        int finalHeight;

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
            //height = s1.ReadData();
            foreach (Stand s in ReaderConnection.stands)
            {
                foreach (String tag in s.tags)
                {
                    //height = s1.ReadData();
                    //Thread.Sleep(400);
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
                if(ReaderConnection.laserHeight >= s.baseHeight && ReaderConnection.laserHeight <= (s.baseHeight + s.height))
                {
                    finalHeight = s.row;
                }
            }

            if (finalHeight != null)
            {
                if (maxCount != maxCount2)
                    finalLocation = maxStand;
                else
                    finalLocation = averageStand;
                if (finalLocation != null)
                    location = finalLocation.locationDescription + finalHeight;
                return location;
            } else
                return "not valid";
        }
    }
}