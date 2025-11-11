----------------------------------------------------
-- 2. CREATE CORE TABLES
----------------------------------------------------

-- Table 1: UsersTbl (Parent Table)

CREATE TABLE [dbo].[UsersTbl] (
    [idx]        INT           IDENTITY (1, 1) NOT NULL,
    [ID]         VARCHAR (50)  NOT NULL,
    [Password]   VARCHAR (256) NOT NULL,
    [Username]   VARCHAR (50)  NOT NULL,
    [Birthday]   DATE          NULL,
    [Email]      VARCHAR (100) NOT NULL,
    [IsLoggedIn] BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([idx] ASC),
	CONSTRAINT Unique_Users_Username UNIQUE NONCLUSTERED ([Username] ASC),
	CONSTRAINT Unique_Users_ID UNIQUE NONCLUSTERED ([ID] ASC),
	CONSTRAINT Unique_Users_Password UNIQUE NONCLUSTERED ([Password] ASC)
);

