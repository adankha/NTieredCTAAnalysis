40380

DECLARE @overallDailyTotal float
SET @overallDailyTotal = (
SELECT SUM(CAST(DailyTotal as BIGINT))
FROM Riderships
);

DECLARE @TotalRidership float
SET @TotalRidership = (
SELECT SUM(CAST(DailyTotal as BIGINT)) 
FROM Riderships
WHERE StationID = 40380
);

DECLARE @totalDays float
SET @totalDays = (
SELECT COUNT(*)
FROM Riderships
WHERE StationID = 40380
);


DECLARE @AvgRidership int
SET @AvgRidership = @TotalRidership / @totalDays
print 'Avg Ridership: ' + CAST(@AvgRidership as varchar) + '/day'

DECLARE @percentRidership float
SET @percentRidership = @TotalRiderShip / @overallDailyTotal * 100.0

print '% Ridership: ' + CAST(@percentRidership as varchar) + '%'

DECLARE @ada int;
DECLARE @direction varchar(50);
DECLARE @latitude varchar(50);
DECLARE @longitude varchar(50);

SELECT @ada = ADA, @direction = Direction, @latitude = Latitude, @longitude = Longitude
FROM Stops
WHERE StopID = 
(
SELECT StopID
FROM Stops
WHERE Name = 'Clark/Lake (Inner Loop)'
)

print 'ADA: ' + CAST(@ada as varchar) + ' Direction: ' + CAST(@direction as varchar) + ' Coordinates (' + CAST(@latitude as varchar) + ', ' + CAST(@longitude as varchar) + ')'

SELECT Color
FROM StopDetails as t1
LEFT JOIN Lines t2 ON t1.LineID = t2.LineID
WHERE t1.StopID =
(
SELECT StopID
FROM Stops
WHERE Name = 'Clark/Lake (Inner Loop)'
)



SELECT SUM(DailyTotal) as 'Weekdays',
(
SELECT SUM(DailyTotal)
FROM Riderships
WHERE TypeOfDay = 'A' AND StationID = 40380
) as 'Weekends',
(
SELECT SUM(DailyTotal)
FROM Riderships
WHERE TypeOfDay = 'U' AND StationID = 40380
) as 'Sunday_Holiday'
FROM Riderships
WHERE TypeOfDay = 'W' AND StationID = 40380


SELECT TOP 10  t1.Name, SUM(CAST(DailyTotal AS BIGINT)) as dailyTotal
FROM Stations as t1
INNER JOIN Riderships as t2 ON t1.StationID = t2.StationID
GROUP BY t1.Name
ORDER BY dailyTotal DESC

Select Count(*) as countz
From Stops

Select Count(*) as counts
From Stations

