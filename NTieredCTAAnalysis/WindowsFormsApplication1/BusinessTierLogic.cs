//
// BusinessTier:  business logic, acting as interface between UI and data store.
//

using System;
using System.Collections.Generic;
using System.Data;


namespace BusinessTier
{

    //
    // Business:
    //
    public class Business
    {
        //
        // Fields:
        //
        private string _DBFile;
        private DataAccessTier.Data dataTier;


        ///
        /// <summary>
        /// Constructs a new instance of the business tier.  The format
        /// of the filename should be either |DataDirectory|\filename.mdf,
        /// or a complete Windows pathname.
        /// </summary>
        /// <param name="DatabaseFilename">Name of database file</param>
        /// 
        public Business(string DatabaseFilename)
        {
            _DBFile = DatabaseFilename;

            dataTier = new DataAccessTier.Data(DatabaseFilename);
        }


        ///
        /// <summary>
        ///  Opens and closes a connection to the database, e.g. to
        ///  startup the server and make sure all is well.
        /// </summary>
        /// <returns>true if successful, false if not</returns>
        /// 
        public bool TestConnection()
        {
            return dataTier.OpenCloseConnection();
        }


        ///
        /// <summary>
        /// Returns all the CTA Stations, ordered by name.
        /// </summary>
        /// <returns>Read-only list of CTAStation objects</returns>
        /// 
        public IReadOnlyList<CTAStation> GetStations(string likeName = "")
        {
            List<CTAStation> stations = new List<CTAStation>();

            try
            {

                string query = "SELECT StationID, Name FROM Stations WHERE Name LIKE '%" + likeName + "%' GROUP BY StationID, Name ORDER BY Name ASC";
                DataSet ds = dataTier.ExecuteNonScalarQuery(query);
                DataTable dt = ds.Tables["TABLE"];

                foreach (DataRow dr in dt.Rows)
                {
                    CTAStation station = new CTAStation((int)dr["StationID"], (string)dr["Name"]);
                    stations.Add(station);
                }

                return stations;


            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetStations: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }
        }

        ///
        /// <summary>
        /// Returns the CTA Stops associated with a given station,
        /// ordered by name.
        /// </summary>
        /// <returns>Read-only list of CTAStop objects</returns>
        ///
        public IReadOnlyList<CTAStop> GetStops(int stationID)
        {
            List<CTAStop> stops = new List<CTAStop>();

            try
            {

                string query = "SELECT * FROM Stops WHERE StationID = '" + stationID + "' ORDER BY Name ASC";
                DataSet ds = dataTier.ExecuteNonScalarQuery(query);
                DataTable dt = ds.Tables["TABLE"];
                foreach (DataRow dr in dt.Rows)
                {
                    CTAStop stop = new CTAStop((int)dr["StopID"], (string)dr["Name"], (int)dr["StationID"],
                                              (string)dr["Direction"], (bool)dr["ADA"], (double)dr["Latitude"],
                                              (double)dr["Longitude"]);
                    stops.Add(stop);
                }
                return stops;
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetStops: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }
        }

        ///
        /// <summary>
        /// Returns the colors for the stop
        /// </summary>
        /// <returns>Read-only list of CTAStop objects</returns>
        ///
        public IReadOnlyList<CTAStop> GetStopColorInfo(string stopName)
        {
            List<CTAStop> stops = new List<CTAStop>();

            try
            {


                string query = String.Format("SELECT Color " +
                                                            "FROM StopDetails as t1 " +
                                                            "LEFT JOIN Lines t2 ON t1.LineID = t2.LineID " +
                                                            "WHERE t1.StopID = " +
                                                            "( " +
                                                            "SELECT StopID " +
                                                            "FROM Stops " +
                                                            "WHERE Name = '{0}')", stopName);

                DataSet ds = dataTier.ExecuteNonScalarQuery(query);
                DataTable dt = ds.Tables["TABLE"];
                foreach (DataRow dr in dt.Rows)
                {
                    CTAStop stop = new CTAStop((string)dr["Color"]);
                    stops.Add(stop);
                }
                return stops;
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetStops: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }
        }


        ///
        /// <summary>
        /// Updates the database here. Changes the ADA to opposite of what it currently is.
        /// </summary>
        /// <returns>Nothing. We are updating the DB here.</returns>
        ///
        public void UpdateStopADA(string stopName)
        {
            List<CTAStop> stops = new List<CTAStop>();

            try
            {


                string query = " " +
                            "UPDATE Stops " +
                            "SET ADA = " +
                            "(SELECT CAST( " +
                                    "CASE " +
                                        "WHEN ADA = 'True' " +
                                        "THEN 0 " +
                                        "ELSE 1 " +
                                        "END AS bit) " +
                                        "FROM Stops " +
                                        "WHERE Name = '"+stopName+"') " +
                            "WHERE Name = '" + stopName + "' ";


                dataTier.ExecuteNonScalarQuery(query);

            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetStops: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }
        }
    


        ///
        /// <summary>
        /// Returns the top N CTA Stations by ridership, 
        /// ordered by name.
        /// </summary>
        /// <returns>Read-only list of CTAStation objects</returns>
        /// 
        public IReadOnlyList<CTAStation> GetTopStations(int N, string typeOfDay = "")
        {
            if (N < 1)
                throw new ArgumentException("GetTopStations: N must be positive");

            List<CTAStation> stations = new List<CTAStation>();

            try
            {

                string query;
                if(!typeOfDay.Equals(""))


                {
                    query = "SELECT TOP " + N + "   t1.Name, SUM((DailyTotal)) as dailyTotal " +
                                       "FROM Stations as t1 " +
                                       "INNER JOIN Riderships as t2 ON t1.StationID = t2.StationID " +
                                       "WHERE t2.TypeOfDay = '" + typeOfDay + "' " +
                                       "GROUP BY t1.Name " +
                                       "ORDER BY dailyTotal DESC ";
                }
                else
                {
                    query = "SELECT TOP " + N + "   t1.Name, SUM((DailyTotal)) as dailyTotal " +
                                       "FROM Stations as t1 " +
                                       "INNER JOIN Riderships as t2 ON t1.StationID = t2.StationID " +
                                       "GROUP BY t1.Name " +
                                       "ORDER BY dailyTotal DESC ";
                }


                DataSet ds = dataTier.ExecuteNonScalarQuery(query);
                DataTable dt = ds.Tables["TABLE"];
                foreach (DataRow dr in dt.Rows)
                {
                    CTAStation station = new CTAStation((string)dr["Name"], (int)dr["dailyTotal"]);
                    stations.Add(station);
                }
                return stations;
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetTopStations: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }


        }

        ///
        /// <summary>
        /// Returns the Station DATA (Ridership details along with the day information), 
        /// </summary>
        /// <returns>Read-only list of CTAStation objects</returns>
        /// 
        public IReadOnlyList<CTAStation> GetStationData(int stationID)
        {
            List<CTAStation> stations = new List<CTAStation>();

            try
            {

                string query = "DECLARE @OverallDailyTotal as BIGINT " +                    // Holds the overall daily total
                               "SET     @OverallDailyTotal = (SELECT SUM(CAST(DailyTotal as BIGINT)) as OverallDailyTotal FROM Riderships) " +

                               "DECLARE @TotalRidership as BIGINT " +                       // Holds the total ridership for the selected station
                               "SET     @totalRidership = (SELECT SUM(CAST(DailyTotal as BIGINT)) as StationDailyTotal FROM Riderships WHERE StationID = '" + stationID + "' ) " +

                               "DECLARE @StationIDTotalDays as INT " +                      // Holds the total days of the selected station
                               "SET @StationIDTotalDays = (SELECT COUNT(*) " +
                               "FROM Riderships " +
                               "WHERE StationID = '" + stationID + "') " +

                               "SELECT @TotalRidership as TotalRidership, " +               // Holds the total ridership (for GUI)
                               "@TotalRidership / @StationIDTotalDays as AvgRidership, " +  // Holds the average ridership (for GUI)
                               "CAST(@TotalRidership as float) / CAST(@OverallDailyTotal as float) * 100.0 as PercentRidership, " + // Holds the percent ridership (for GUI)

                               //
                               //Extracts the info from database of the select stations day info (weekday, saturday, sun/holiday)
                               //
                               "SUM(DailyTotal) as Weekdays, " +
                               "(SELECT SUM(DailyTotal) FROM Riderships " +
                               "WHERE TypeOfDay = 'A' AND StationID = '" + stationID + "' " +
                               ") as Saturdays, " +

                               "( " +
                               "SELECT SUM(DailyTotal) " +
                               "FROM Riderships " +
                               "WHERE TypeOfDay = 'U' AND StationID = '" + stationID + "' " +
                               ") as Sunday_Holiday " +
                               "FROM Riderships " +
                               "WHERE TypeOfDay = 'W' AND StationID = '" + stationID + "'";


                DataSet ds = dataTier.ExecuteNonScalarQuery(query);
                DataTable dt = ds.Tables["TABLE"];
                foreach (DataRow dr in dt.Rows)
                {
                    CTAStation station = new CTAStation(stationID, (long)dr["TotalRidership"], (long)dr["AvgRidership"], (double)dr["PercentRidership"],
                                              (int)dr["Weekdays"], (int)dr["Saturday"], (int)dr["Sunday_Holiday"]);
                    stations.Add(station);
                }
                return stations;
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetStationData: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }
        }

    }//class
}//namespace
