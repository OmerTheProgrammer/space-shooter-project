----------------------------------------------------
-- 6. CREATE REQUEST DETAIL TABLES
----------------------------------------------------

-- Table 7: RequestsDataTbl (Holds the specific changes for each request)
CREATE TABLE [dbo].[RequestsDataTbl] (
    [idx]        INT           IDENTITY (1, 1) NOT NULL,
    [RequestIdx] INT           NOT NULL,
    [Field]      VARCHAR (50)  NOT NULL,
    [OldValue]   VARCHAR (256) NULL,
    [NewValue]   VARCHAR (256) NULL,

    -- Primary Key: Optimized for high-speed sequential inserts
    PRIMARY KEY CLUSTERED ([idx] ASC) WITH (FILLFACTOR = 100),

    -- Foreign Key: Links to the main request header
    -- ON DELETE CASCADE ensures that if a request is deleted, its data is wiped too
    CONSTRAINT [FK_RequestsData_RequestHeader] 
        FOREIGN KEY ([RequestIdx]) 
        REFERENCES [dbo].[ProfileEditRequestsTbl] ([idx]) 
        ON DELETE CASCADE
);