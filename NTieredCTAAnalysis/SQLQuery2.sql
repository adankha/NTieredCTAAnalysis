DECLARE @stationID as INT
set @stationID = 40010

DECLARE @OverallDailyTotal as BIGINT
SET		@overallDailyTotal = (SELECT SUM(CAST(DailyTOtal as BIGINT)) as OverallDailyTotal 
							  FROM Riderships)

DECLARE @TotalRidership BIGINT
SET		@totalRidership = (SELECT SUM(CAST(DailyTotal as BIGINT)) as StationDailyTotal 
						   FROM Riderships 
						   WHERE StationID = stationID )

DECLARE @StationIDTotalDays as INT
SET		@StationIDTotalDays = (SELECT COUNT(*)
							   FROM Riderships 
							   WHERE StationID = stationID)

SELECT  @TotalRidership as TotalRidership, 
		@TotalRidership / @StationIDTotalDays as AvgRidership, 
		CAST(@TotalRidership as float) / CAST(@OverallDailyTotal as float) * 100.0 as PercentRidership,

		SUM(DailyTotal) as Weekdays, 
		(SELECT SUM(DailyTotal) FROM Riderships 
        WHERE TypeOfDay = 'A' AND StationID = stationID
        ) as Saturdays,
		 
        ( 
        SELECT SUM(DailyTotal) 
        FROM Riderships 
        WHERE TypeOfDay = 'U' AND StationID = stationID
        ) as Sunday_Holiday

        FROM Riderships
        WHERE TypeOfDay = 'W' AND StationID = stationID


		SELECT ADA, Direction, Latitude, Longitude 
        FROM Stops
        WHERE StopID = 30010
       ( 
       SELECT StopID
       FROM Stops
       WHERE Name = '18th (Pink Line)'
	   )

    SELECT Color
    FROM StopDetails as t1 
    LEFT JOIN Lines t2 ON t1.LineID = t2.LineID 
    WHERE t1.StopID = 
    ( 
    SELECT StopID 
    FROM Stops 
    WHERE Name = 'Clark/Lake (Inner Loop)'
	)


	SELECT StationID, Name
	FROM Stations
	WHERE Name LIKE '%Chicago%'
	ORDER BY Name ASC


SELECT TOP 10  t1.Name, SUM((DailyTotal)) as dailyTotal 
FROM Stations as t1 
INNER JOIN Riderships as t2 ON t1.StationID = t2.StationID 
WHERE t2.TypeOfDay = 'U'
GROUP BY t1.Name
ORDER BY dailyTotal DESC


SELECT ADA
FROM Stops
WHERE Name = 'Austin (O''Hare-bound)'


SELECT ADA
FROM Stops
WHERE Name = 'Harlem (63rd-bound)'



UPDATE Stops
SET ADA = 
(SELECT CAST(
		CASE
			WHEN ADA = 'True'
			THEN 0
			ELSE 1
			END AS bit)
			FROM Stops 
			WHERE Name = 'Austin (O''Hare-bound)')

WHERE Name = 'Austin (O''Hare-bound)'
			

