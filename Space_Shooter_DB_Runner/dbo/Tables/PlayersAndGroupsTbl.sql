----------------------------------------------------
-- 3. CREATE LINKING (JUNCTION) TABLE
----------------------------------------------------

-- Table 4: PlayersAndGroupsTbl (Many-to-Many link between Players and Groups)
CREATE TABLE [dbo].[PlayersAndGroupsTbl] (
    [idx]       INT IDENTITY (1, 1) NOT NULL,
    [PlayerIdx] INT NOT NULL,
    [GroupIdx]  INT NOT NULL,

    -- Primary Key: Defines the physical order of the data
    PRIMARY KEY CLUSTERED ([idx] ASC) WITH (FILLFACTOR = 100),

    -- Unique Constraint: Prevents a player from joining the same group multiple times
    CONSTRAINT [UQ_PlayerGroup_PlayerIdxAndGroupIdx] 
        UNIQUE NONCLUSTERED ([PlayerIdx] ASC, [GroupIdx] ASC) WITH (FILLFACTOR = 100),

    -- Foreign Keys: Maintain referential integrity with source tables
    FOREIGN KEY ([PlayerIdx]) REFERENCES [dbo].[PlayersTbl] ([idx]),
    FOREIGN KEY ([GroupIdx]) REFERENCES [dbo].[GroupsTbl] ([idx])
);