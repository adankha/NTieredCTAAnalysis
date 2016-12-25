//
// BusinessTier objects:  these classes define the objects serving as data 
// transfer between UI and business tier.  These objects carry the data that
// is normally displayed in the presentation tier.  The classes defined here:
//
//    CTAStation
//    CTAStop
//
// NOTE: the presentation tier should not be creating instances of these objects,
// but instead calling the BusinessTier logic to obtain these objects.  You can 
// create instances of these objects if you want, but doing so has no impact on
// the underlying data store --- to change the data store, you have to call the
// BusinessTier logic.
//

using System;
using System.Collections.Generic;


namespace BusinessTier
{

    ///
    /// <summary>
    /// Info about one CTA station.
    /// </summary>
    /// 
    public class CTAStation
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public int DailyTotal { get; private set; }

        public long TotalRidership { get; private set; }
        public long AvgRidership { get; private set; }
        public double PercentRidership { get; private set; }

        public int Weekdays { get; private set; }
        public int Saturdays { get; private set; }
        public int Sunday_Holidays { get; private set; }




        public CTAStation(int stationID, string stationName)
        {
            ID = stationID;
            Name = stationName;
        }
        public CTAStation(string stationName, int dailyTotal)
        {
            Name = stationName;
            DailyTotal = dailyTotal;
        }

        public CTAStation(int stationID, long totalRidership, long avgRidership, double percentRidership, int weekdays, int saturday, int sun_holiday)
        {
            ID = stationID;
            TotalRidership = totalRidership;
            AvgRidership = avgRidership;
            PercentRidership = percentRidership;
            Weekdays = weekdays;
            Saturdays = saturday;
            Sunday_Holidays = sun_holiday;
        }



    }

    ///
    /// <summary>
    /// Info about one CTA stop.
    /// </summary>
    /// 
    public class CTAStop
    {
        public int ID { get; private set; }

        public string Name { get; private set; }

        public int StationID { get; private set; }

        public string Direction { get; private set; }

        public bool ADA { get; private set; }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public string ColorDetail { get; private set; }

        public CTAStop(int stopID, string stopName, int stationID, string direction, bool ada, double latitude, double longitude)
        {
            ID = stopID;
            Name = stopName;
            StationID = stationID;
            Direction = direction;
            ADA = ada;
            Latitude = latitude;
            Longitude = longitude;
        }

        public CTAStop(string colorDetail)
        {
            ColorDetail = colorDetail;
        }

    }

}//namespace
