-- Table 3: Groups
CREATE TABLE Groups (
    idx INT IDENTITY(1,1) PRIMARY KEY,
    GroupScore INT NOT NULL DEFAULT 0,
    LeadBoardPos INT NOT NULL -- Current position in the leaderboard
);