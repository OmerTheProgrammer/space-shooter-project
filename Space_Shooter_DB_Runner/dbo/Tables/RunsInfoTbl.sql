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
    ShieldLevel INT NOT NULL CHECK (ShieldLevel >= 0) DEFAULT 0,
    BlasterCount INT NOT NULL CHECK (BlasterCount >= 1 AND BlasterCount <= 9) DEFAULT 1,
    HP INT NOT NULL CHECK (HP >= 0) DEFAULT 100,
    IsRunOver BIT NOT NULL DEFAULT 1
    
    -- Define Foreign Key
    FOREIGN KEY (PlayerIdx) REFERENCES PlayersTbl(idx)
);