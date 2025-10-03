----------------------------------------------------
-- 6. CREATE ADMINS TABLE
----------------------------------------------------


-- Table 8: Admins (1:1 with Users)
CREATE TABLE Admins (
    idx INT PRIMARY KEY, -- Use the Users' idx as the Primary Key and Foreign Key
    StartDate DATETIME NOT NULL,

    -- Define Foreign Key (Enforcing 1:1 relationship)
    FOREIGN KEY (idx) REFERENCES Users(idx)
);