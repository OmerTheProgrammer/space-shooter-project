----------------------------------------------------
-- 3. CREATE LINKING (JUNCTION) TABLE
----------------------------------------------------

-- Table 4: PlayersAndGroups (Many-to-Many link between Players and Groups)
CREATE TABLE PlayersAndGroups (
    idx INT IDENTITY(1,1) PRIMARY KEY, -- Surrogate Key
    PlayerIdx INT NOT NULL,
    GroupIdx INT NOT NULL,

    -- Define Foreign Keys
    FOREIGN KEY (PlayerIdx) REFERENCES Players(idx),
    FOREIGN KEY (GroupIdx) REFERENCES Groups(idx),

    -- Enforce the rule that a player can only be in a group once (Crucial for linking tables)
    UNIQUE (PlayerIdx, GroupIdx)
);