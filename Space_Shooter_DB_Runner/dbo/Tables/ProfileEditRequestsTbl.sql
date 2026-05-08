-- Table 6: ProfileEditRequestsTbl
CREATE TABLE [dbo].[ProfileEditRequestsTbl] (
    [idx]            INT      IDENTITY (1, 1) NOT NULL,
    [PlayerIdx]      INT      NOT NULL,
    [RequestingDate] DATETIME NOT NULL,
    
    -- Status values: 0=Pending, 1=Approved, 2=Rejected, 3=Canceled
    [Status]         INT      CONSTRAINT [DF_ProfileEdit_Status] DEFAULT ((0)) NOT NULL,
    [ReviewingDate]  DATETIME NULL, 
    [AdminIdx]       INT      NULL, 

    -- Primary Key: Optimized for sequential inserts
    PRIMARY KEY CLUSTERED ([idx] ASC) WITH (FILLFACTOR = 100),

    -- Foreign Keys: Linking to Players and Admins
    CONSTRAINT [FK_ProfileEdit_Player] FOREIGN KEY ([PlayerIdx]) REFERENCES [dbo].[PlayersTbl] ([idx]),
    CONSTRAINT [FK_ProfileEdit_Admin]  FOREIGN KEY ([AdminIdx])  REFERENCES [dbo].[AdminsTbl] ([idx]),

    -- Check Constraint: Ensures data integrity for the request status
    CONSTRAINT [CK_ProfileEdit_Status] CHECK ([Status] IN (0, 1, 2, 3))
);