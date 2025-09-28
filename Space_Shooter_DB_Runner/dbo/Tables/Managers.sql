----------------------------------------------------
-- 6. CREATE MANAGERS TABLE
----------------------------------------------------


-- Table 8: Managers (1:1 with Users)
CREATE TABLE Managers (
    idx INT PRIMARY KEY, -- Use the Users' idx as the Primary Key and Foreign Key
    StartDate DATETIME NOT NULL,

    -- Define Foreign Key (Enforcing 1:1 relationship)
    FOREIGN KEY (idx) REFERENCES Users(idx)
);