-- Table 2: Players
CREATE TABLE Players (
    idx INT PRIMARY KEY,
    MaxLevel INT NOT NULL DEFAULT 0,
    TotalScore INT NOT NULL DEFAULT 0,
    IsSoundOn BIT NOT NULL DEFAULT 1,
    IsMusicOn BIT NOT NULL DEFAULT 1,
    
    -- Define Foreign Key relationship
    FOREIGN KEY (idx) REFERENCES Users(idx)
);