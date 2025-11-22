----------------------------------------------------
-- 3. CREATE LINKING (JUNCTION) TABLE
----------------------------------------------------

-- Table 4: PlayersAndGroupsTbl (Many-to-Many link between Players and Groups)
CREATE TABLE PlayersAndGroupsTbl (
    idx INT IDENTITY(1,1) PRIMARY KEY, -- Surrogate Key
    PlayerIdx INT NOT NULL,
    GroupIdx INT NOT NULL,

    -- Define Foreign Keys
    FOREIGN KEY (PlayerIdx) REFERENCES PlayersTbl(idx),
    FOREIGN KEY (GroupIdx) REFERENCES GroupsTbl(idx),

    -- Enforce the rule that a player can only be in a group once (Crucial for linking tables)
CONSTRAINT UQ_PlayerGroup_PlayerIdxAndGroupIdx UNIQUE (PlayerIdx, GroupIdx));