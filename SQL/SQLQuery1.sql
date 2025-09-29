-- ====================================================================
-- FINAL DIAGNOSTIC & CLEANUP SCRIPT
-- ====================================================================

-- Step 1: Switch to the database your application is using.
USE GroomMateDB_New;
GO

-- Step 2: Confirm we are in the right place.
PRINT 'SUCCESS: Now operating on database [GroomMateDB_New].';
GO

-- Step 3: Count the problem records before we do anything.
PRINT '--------------------------------------------------';
PRINT 'Checking for orphaned appointments BEFORE cleanup...';
DECLARE @OrphanCountBefore INT;
SELECT @OrphanCountBefore = COUNT(*)
FROM Appointments
WHERE UserID NOT IN (SELECT UserID FROM Users);

PRINT CONCAT('RESULT: Found ', @OrphanCountBefore, ' orphaned appointment(s).');
PRINT '--------------------------------------------------';
GO

-- Step 4: If we found any problem records, delete them.
IF (SELECT COUNT(*) FROM Appointments WHERE UserID NOT IN (SELECT UserID FROM Users)) > 0
BEGIN
    PRINT 'Attempting to delete the orphaned appointments...';
    
    DELETE FROM Appointments
    WHERE UserID NOT IN (SELECT UserID FROM Users);

    PRINT CONCAT('SUCCESS: ', @@ROWCOUNT, ' orphaned row(s) have been deleted.');
END
ELSE
BEGIN
    PRINT 'INFO: No orphaned appointments were found to delete.';
END
GO

-- Step 5: Verify that the problem records are now gone.
PRINT '--------------------------------------------------';
PRINT 'Verifying cleanup...';
DECLARE @OrphanCountAfter INT;
SELECT @OrphanCountAfter = COUNT(*)
FROM Appointments
WHERE UserID NOT IN (SELECT UserID FROM Users);

PRINT CONCAT('RESULT: Found ', @OrphanCountAfter, ' orphaned appointment(s) after cleanup.');
PRINT '--------------------------------------------------';

IF @OrphanCountAfter = 0
BEGIN
    PRINT 'VERIFICATION COMPLETE: All orphaned records are gone.';
END
ELSE
BEGIN
    PRINT 'ERROR: Cleanup did NOT complete successfully. The issue persists.';
END
GO
