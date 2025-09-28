----------------------------------------------------
-- 2. CREATE CORE TABLES
----------------------------------------------------

-- Table 1: Users (Parent Table)

CREATE TABLE Users (
    idx INT IDENTITY(1,1) PRIMARY KEY,
    ID VARCHAR(50) UNIQUE NOT NULL, -- The unique user identifier (e.g., UUID or other system ID)
    Password VARCHAR(256) NOT NULL, -- Store as a Hashed string (recommended)
    Username VARCHAR(50) UNIQUE NOT NULL,
    Birthday DATE, -- Use DATE or DATETIME for Birthday
    Email VARCHAR(100) UNIQUE NOT NULL,
    IsLoggedIn BIT NOT NULL DEFAULT 0 -- BIT is used for boolean (1=True, 0=False)
);