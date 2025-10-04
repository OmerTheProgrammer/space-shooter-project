----------------------------------------------------
-- 5. CREATE PROFILE/REQUEST TABLES
----------------------------------------------------

-- Table 6: ProfileEditRequestsTbl
CREATE TABLE ProfileEditRequestsTbl (
    idx INT IDENTITY(1,1) PRIMARY KEY,
    PlayerIdx INT NOT NULL, -- Changed from UserID to PlayerIdx (Foreign Key to Players table)
    RequestDateTime DATETIME NOT NULL,
    
    -- Status values: 0=Pending, 1=Approved, 2=Rejected, 3=Canceled
    [Status] INT NOT NULL CHECK ([Status] IN (0, 1, 2, 3)) DEFAULT 0, 
    ReviewDate DATETIME NULL, -- NULL if still Pending
    AdminIdx INT NULL,    -- New field based on your image (Foreign Key to Admins table)

    -- Define Foreign Key: PlayerIdx links to the Player's ID in the Players table
    FOREIGN KEY (PlayerIdx) REFERENCES PlayersTbl(idx),

    -- Define Foreign Key: AdminIdx links to the Admin's ID in the Admins table
    -- I've made this NULLable (NULL) since a request might not be assigned immediately.
    FOREIGN KEY (AdminIdx) REFERENCES AdminsTbl(idx)
);