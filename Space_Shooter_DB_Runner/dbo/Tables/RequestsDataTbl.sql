-- Table 7: RequestData (Holds the specific changes requested)
CREATE TABLE [dbo].[RequestsDataTbl] (
    idx INT IDENTITY(1,1) PRIMARY KEY,
    RequestIdx INT NOT NULL, -- Foreign Key to ProfileEditRequests table
    Field VARCHAR(50) NOT NULL,
    OldValue VARCHAR(256),
    NewValue VARCHAR(256),

    -- Define Foreign Key WITH ON DELETE CASCADE
    FOREIGN KEY (RequestIdx) 
    REFERENCES [dbo].[ProfileEditRequestsTbl](idx) 
    ON DELETE CASCADE -- <-- This is the required addition for cascading deletion
);