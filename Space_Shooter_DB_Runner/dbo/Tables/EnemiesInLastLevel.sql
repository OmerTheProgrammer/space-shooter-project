-- Defines the structure to track enemies encountered in a specific run.
-- This table allows for multiple rows (N) to link back to a single Run (1).

CREATE TABLE [dbo].[EnemiesInLastLevel] (
    -- Primary Key: Identity column for this specific record.
    [idx] INT IDENTITY (1, 1) NOT NULL,

    -- Foreign Key: Links this enemy record back to a specific player run.
    -- (RunIdx in the image corresponds to the idx in RunsInfo)
    [RunIdx]       INT            NOT NULL,

    -- Data Fields
    [Name]         NVARCHAR (50)  NOT NULL, -- e.g., 'Enemy(basic)', 'Enemy(fast)'
    [Amount]       INT            NOT NULL, -- The number of this enemy type encountered.

    -- Constraints
    CONSTRAINT [PK_EnemiesInLastLevel] PRIMARY KEY CLUSTERED ([idx] ASC),

    -- Foreign Key Constraint to RunsInfo
    CONSTRAINT [FK_EnemiesInLastLevel_RunsInfo] FOREIGN KEY ([RunIdx]) REFERENCES [dbo].[RunsInfo] ([idx]),

    CONSTRAINT [CK_ValidEnemyValue] CHECK ([Name] = (3)
                                               OR [Name] = (2)
                                               OR [Name] = (1)
                                               OR [Name] = (0))
);