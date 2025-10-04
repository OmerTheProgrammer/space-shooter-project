----------------------------------------------------
-- 2. CREATE CORE TABLES
----------------------------------------------------

-- Table 1: UsersTbl (Parent Table)

CREATE TABLE [dbo].[UsersTbl] (
    [idx]        INT           IDENTITY (1, 1) NOT NULL,
    [ID]         VARCHAR (50)  NOT NULL unique,
    [Password]   VARCHAR (256) NOT NULL unique,
    [Username]   VARCHAR (50)  NOT NULL unique,
    [Birthday]   DATE          NULL,
    [Email]      VARCHAR (100) NOT NULL,
    [IsLoggedIn] BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([idx] ASC),
    UNIQUE NONCLUSTERED ([Username] ASC),
    UNIQUE NONCLUSTERED ([ID] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);