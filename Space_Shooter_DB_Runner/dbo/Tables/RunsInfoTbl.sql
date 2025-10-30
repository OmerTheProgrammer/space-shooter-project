----------------------------------------------------
-- 4. CREATE GAME/RUNS INFO TABLES
----------------------------------------------------

-- Table 5: RunsInfoTbl
CREATE TABLE RunsInfoTbl (
    idx INT IDENTITY(1,1) PRIMARY KEY,
    PlayerIdx INT NOT NULL, -- Foreign Key to Players table
    CurrentScore INT NOT NULL DEFAULT 0,
    CurrentLevel INT NOT NULL DEFAULT 1,
    RunStopDate DATE NOT NULL,
    CurrentShieldLevel INT NOT NULL CHECK (CurrentShieldLevel >= 0) DEFAULT 0,
    CurrentBlasterCount INT NOT NULL CHECK (CurrentBlasterCount >= 1 AND CurrentBlasterCount <= 9) DEFAULT 1,
    CurrentHP INT NOT NULL CHECK (CurrentHP >= 0) DEFAULT 100,
    IsRunOver BIT NOT NULL DEFAULT 1
    
    -- Define Foreign Key
    FOREIGN KEY (PlayerIdx) REFERENCES PlayersTbl(idx) ON DELETE CASCADE
);