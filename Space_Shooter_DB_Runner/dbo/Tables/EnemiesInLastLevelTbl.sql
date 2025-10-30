-- Defines the structure to track enemies encountered in a specific run.
-- This table allows for multiple rows (N) to link back to a single Run (1).

CREATE TABLE [dbo].[EnemiesInLastLevelTbl] (
    -- Primary Key: Identity column for this specific record.
    [idx] INT IDENTITY (1, 1) NOT NULL,

    -- Foreign Key: Links this enemy record back to a specific player run.
    -- (RunIdx in the image corresponds to the idx in RunsInfo)
    [RunInfoIdx] INT            NOT NULL,

    -- Data Fields
    [Name]   INT NOT NULL CHECK ([Name] IN (0, 1, 2, 3)) DEFAULT 0,
    [Amount] INT NOT NULL, -- The number of this enemy type encountered.

    -- Constraints
    CONSTRAINT [PK_EnemiesInLastLevel] PRIMARY KEY CLUSTERED ([idx] ASC),

    -- Foreign Key Constraint to RunsInfo
    CONSTRAINT [FK_EnemiesInLastLevel_RunsInfo] FOREIGN KEY ([RunIdx]) REFERENCES [dbo].[RunsInfoTbl] ([idx]),
);