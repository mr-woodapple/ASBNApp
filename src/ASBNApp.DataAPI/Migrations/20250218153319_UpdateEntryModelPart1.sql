-- Assign the matching location id based on the location name saved as a string
-- Only updated if the OwnerId's match!

UPDATE LogEntry
SET LocationId = WL.Id
FROM LogEntry E
	INNER JOIN WorkLocation WL ON E.OldLocation = WL.LocationName
WHERE E.OwnerId = WL.OwnerId
	AND E.LocationId IS NULL
	AND E.OldLocation IS NOT NULL;