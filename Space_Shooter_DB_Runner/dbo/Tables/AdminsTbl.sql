----------------------------------------------------
-- 6. CREATE ADMINSTbl TABLE
----------------------------------------------------


-- Table 8: AdminsTbl (1:1 with Users)
CREATE TABLE AdminsTbl (
    idx INT PRIMARY KEY, -- Use the Users' idx as the Primary Key and Foreign Key
    StartDate DATETIME NOT NULL,

    -- Define Foreign Key (Enforcing 1:1 relationship)
    FOREIGN KEY (idx) REFERENCES UsersTbl(idx)
);