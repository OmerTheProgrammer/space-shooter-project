-- Table 7: RequestData (Holds the specific changes requested)
CREATE TABLE RequestsDataTbl (
    idx INT IDENTITY(1,1) PRIMARY KEY,
    RequestIdx INT NOT NULL, -- Foreign Key to ProfileEditRequests table
    Field VARCHAR(50) NOT NULL,
    OldValue VARCHAR(256),
    NewValue VARCHAR(256),

    -- Define Foreign Key
    FOREIGN KEY (RequestIdx) REFERENCES ProfileEditRequestsTbl(idx)
);