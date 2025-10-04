/*
Post-Deployment Script Template
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.
 Use SQLCMD syntax to include a file in the post-deployment script.
 Example:    :r .\myfile.sql
 Use SQLCMD syntax to reference a variable in the post-deployment script.
 Example:    :setvar TableName MyTable
             SELECT * FROM [$(TableName)]
--------------------------------------------------------------------------------------
*/

-- IMPORTANT: The order matters due to foreign key constraints.
-- Users must be inserted before Admins, Players, or RequestData.
-- Players must be inserted before PlayersAndGroups or RunsInfo.

---------------------------------------------------------------------------------
-- USERS TABLE BLOCK (Parents for Admins and Players)
---------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.Users)
BEGIN
    PRINT 'Inserting initial data into dbo.Users...'

    -- Insert 10 new Users (who will also be Admins) and 10 more (who will be players)
    INSERT INTO dbo.Users (ID, Password, Username, Birthday, Email) VALUES
    -- Admin 1: idx will be 1
    ('111222333', '063b4991196144e54a01c801e85579f1807d93425f187707e152003c267793d5', 'AdminAlpha', '1985-06-15', 'Admin.alpha@spaceshooter.com'),
    -- Admin 2: idx will be 2
    ('111222444', '24043b3512a382c7104f98553641151df2ce1648a37f5978a632822c7a40b991', 'AdminBeta', '1990-01-20', 'Admin.beta@spaceshooter.com'),
    -- Admin 3: idx will be 3
    ('111222555', 'a33d455d3e07089ff528c7c9e1c25ff968222a7f5a9e32e85084930be6287c7e', 'AdminGamma', '1988-11-05', 'Admin.gamma@spaceshooter.com'),
    -- Admin 4: idx will be 4
    ('111222666', '6d506d34b419430c7cc89988a82c61099df3d85429a1b6422d1b70d4722513c0', 'AdminDelta', '1995-03-25', 'Admin.delta@spaceshooter.com'),
    -- Admin 5: idx will be 5
    ('111222777', '6100908b8d9c57843472be68e4f16a9a08a46b08ccf865f17a943a41113c242a', 'AdminEpsilon', '1992-09-30', 'Admin.epsilon@spaceshooter.com'),
    -- Admin 6: idx will be 6
    ('111333111', '652309f19379659b9a6704b281f6d33878b209d81d295982e01b44b92b679a83', 'AdminZeta', '1983-04-12', 'Admin.zeta@spaceshooter.com'),
    -- Admin 7: idx will be 7
    ('111333222', 'f5463768160021b36952e46b0e8b26120e2cc5f3b7d188331168532f507b973a', 'AdminEta', '1998-07-01', 'Admin.eta@spaceshooter.com'),
    -- Admin 8: idx will be 8
    ('111333333', '91f1816f1c713b6329e46a59992f15309d4367f08b77684df6d5024213791244', 'AdminTheta', '1981-12-19', 'Admin.theta@spaceshooter.com'),
    -- Admin 9: idx will be 9
    ('111333444', '66f81e330536c4b9d0344d5a1538356f91722881a7051412b1897e930f73b885', 'AdminIota', '1996-02-28', 'Admin.iota@spaceshooter.com'),
    -- Admin 10: idx will be 10
    ('111333555', '0e7225c567e7223b20468e64c3c3cc79c882141528659b85c345377f0c11f71a', 'AdminKappa', '1987-10-10', 'Admin.kappa@spaceshooter.com'),

    -- Player 1: idx will be 11
    ('111375655', '948f98c92a95c46b5d233869279a024c08e5e7f1e78453535efd24d9c750e3ce', 'StarPilot_Ace', '2000-05-10', 'pilot.ace@game.com'),
    -- Player 2: idx will be 12
    ('516937402', 'e2d7f8a7062402927289b439c2c8f075d9c288b8595a822008f106880486c429', 'GalaxyRunner', '1999-01-25', 'g.runner@game.com'),
    -- Player 3: idx will be 13
    ('294158670', 'd4b5d625d9472e9a5c4e97669d51e891334c4f307371597f707f10b2e675086d', 'CosmicDrifter', '2003-12-03', 'drifter_c@game.com'),
    -- Player 4: idx will be 14
    ('763024851', '3a4915a31776953c898c6d1f0545f444837267ce03964250239b56f2e82b78b8', 'ZeroGravity', '1995-07-17', 'zerog@game.com'),
    -- Player 5: idx will be 15
    ('450782913', 'b7e600c3b016259021e8d90479d2cc955d7f1c1f728c73656c125d0c8397a8e7', 'CometChaser', '1997-04-01', 'chaser.c@game.com'),
    -- Player 6: idx will be 16
    ('821546390', '1793740e5509d94944d1808af2d1279a1af4d74a7d65b74681321d3f3f33664d', 'NebulaNomad', '2001-09-29', 'nomad_n@game.com'),
    -- Player 7: idx will be 17
    ('639401725', '3c4f74d9e29548b111559869680a6534575c345b597b41e3d360f27916a4f4e7', 'HyperDriveHero', '1993-11-11', 'hero.h@game.com'),
    -- Player 8: idx will be 18
    ('172859036', '4c0e6605a96323c28a8d166827a58b2921ef5edb210f96899583b28b7e80a049', 'AstroBlaster', '2005-02-14', 'blastr.a@game.com'),
    -- Player 9: idx will be 19
    ('905263184', '85a852026194b15091722d4f2913e164d1f2122650f92b7747e9b015e1979b90', 'VoidVagabond', '1998-08-08', 'v.vagabond@game.com'),
    -- Player 10: idx will be 20
    ('348197562', 'f626605d51e70e59a41981e4b8686e589df4641d406a4666f07b469502b48e42', 'OrbitalOutlaw', '1996-03-21', 'outlaw.o@game.com');
END
GO -- End of dbo.Users IF block

---------------------------------------------------------------------------------
-- ADMINS TABLE BLOCK (Depends on Users)
---------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.Admins)
BEGIN
    PRINT 'Inserting initial data into dbo.Admins...'
    INSERT INTO dbo.Admins (idx, StartDate) VALUES
    (1, '2024-01-01 09:00:00'),
    (2, '2024-01-15 10:30:00'),
    (3, '2024-02-20 11:00:00'),
    (4, '2024-03-05 14:00:00'),
    (5, '2024-04-10 08:30:00'),
    (6, '2024-05-25 12:00:00'),
    (7, '2024-06-01 15:30:00'),
    (8, '2024-07-18 09:45:00'),
    (9, '2024-08-22 13:00:00'),
    (10, '2024-09-05 11:15:00');
END
GO -- End of dbo.Admins IF block

---------------------------------------------------------------------------------
-- PLAYERS TABLE BLOCK (Depends on Users)
---------------------------------------------------------------------------------
-- Note: User inserts for players (idx 11-20) are done in the dbo.Users block above.
IF NOT EXISTS (SELECT 1 FROM dbo.Players)
BEGIN
    PRINT 'Inserting initial data into dbo.Players...'
    INSERT INTO dbo.Players (idx, MaxLevel, TotalScore, IsSoundOn, IsMusicOn) VALUES
    (11, 5, 12500, 1, 1),
    (12, 3, 5800, 1, 0),
    (13, 1, 1500, 0, 0),
    (14,8, 45000, 1, 1),
    (15,4, 9200, 1, 1),
    (16,6, 21000, 0, 1),
    (17,7, 35500, 1, 0),
    (18,2, 3100, 1, 1),
    (19,9, 60000, 1, 1),
    (20,5, 14500, 0, 0);
END
GO -- End of dbo.Players IF block

---------------------------------------------------------------------------------
-- PROFILE EDIT REQUESTS TABLE BLOCK (Depends on Players and Admins)
---------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.ProfileEditRequests)
BEGIN
    PRINT 'Inserting initial data into dbo.ProfileEditRequests...'
    INSERT INTO dbo.ProfileEditRequests
        (PlayerIdx, RequestDateTime, Status, ReviewDate, AdminIdx)
    VALUES
    -- Request 1: Player 11 (Pending), assigned to Admin 1
    (11, '2025-09-29 10:00:00', 0, NULL, 1),

    -- Request 2: Player 14 (Approved), assigned to Admin 2
    (14, '2025-09-28 15:30:00', 1, '2025-09-29 09:00:00', 2),

    -- Request 3: Player 15 (Rejected), assigned to Admin 3
    (15, '2025-09-29 11:45:00', 2, '2025-09-29 14:00:00', 3),

    -- Request 4: Player 19 (Pending), assigned to Admin 4
    (19, '2025-09-30 09:10:00', 0, NULL, 4),

    -- Request 5: Player 12 (Canceled), assigned to Admin 5
    (12, '2025-09-25 08:00:00', 3, NULL, 5),

    -- Request 6: Player 18 (Approved), assigned to Admin 6
    (18, '2025-09-26 13:00:00', 1, '2025-09-27 15:00:00', 6),

    -- Request 7: Player 17 (Pending), assigned to Admin 7
    (17, '2025-09-30 11:30:00', 0, NULL, 7),

    -- Request 8: Player 20 (Rejected), assigned to Admin 8
    (20, '2025-09-28 16:00:00', 2, '2025-09-29 10:00:00', 8),

    -- Request 9: Player 16 (Approved), assigned to Admin 9
    (16, '2025-09-27 09:40:00', 1, '2025-09-27 10:05:00', 9),

    -- Request 10: Player 13 (Pending), assigned to Admin 10
    (13, '2025-09-29 14:15:00', 0, NULL, 10);
END
GO -- End of dbo.ProfileEditRequests IF block

---------------------------------------------------------------------------------
-- REQUEST DATA TABLE BLOCK (Depends on ProfileEditRequests)
---------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.RequestData)
BEGIN
    PRINT 'Inserting initial data into dbo.RequestData...'
    INSERT INTO dbo.RequestData
        (RequestIdx, Field, OldValue, NewValue)
    VALUES
    -- Request 1 (Player 11, Pending): Changing Username
    (1, 'Username', 'StarPilot_Ace', 'AcePilot_77'),

    -- Request 2 (Player 14, Approved): Changing Email
    (2, 'Email', 'zerog@game.com', 'zero_gravity_main@game.com'),

    -- Request 3 (Player 15, Rejected): Changing Password (Hash)
    (3, 'Password', 'hashed_pass_P15', 'new_hashed_pass_P15'),

    -- Request 4 (Player 19, Pending): Changing Birthday
    (4, 'Birthday', '1998-08-08', '1997-08-08'),

    -- Request 5 (Player 12, Canceled): Changing Username
    (5, 'Username', 'GalaxyRunner', 'TheGalacticOne'),

    -- Request 6 (Player 18, Approved): Changing Email
    (6, 'Email', 'blastr.a@game.com', 'astro_blast@game.com'),

    -- Request 7 (Player 17, Pending): Changing Email
    (7, 'Email', 'hero.h@game.com', 'hyperhero@game.com'),

    -- Request 8 (Player 20, Rejected): Changing Password (Hash)
    (8, 'Password', 'hashed_pass_P20', 'another_new_hash'),

    -- Request 9 (Player 16, Approved): Changing Username
    (9, 'Username', 'NebulaNomad', 'NebulaNomad_X'),

    -- Request 10 (Player 13, Pending): Changing Birthday
    (10, 'Birthday', '2003-12-03', '2003-12-04');
END
GO -- End of dbo.RequestData IF block

---------------------------------------------------------------------------------
-- GROUPS TABLE BLOCK (Independent)
---------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.Groups)
BEGIN
    PRINT 'Inserting initial data into dbo.Groups...'
    INSERT INTO dbo.Groups (GroupScore) VALUES
    -- Group 1: idx will be 1
    (550000),
    -- Group 2: idx will be 2
    ( 920000),
    -- Group 3: idx will be 3
    ( 120000),
    -- Group 4: idx will be 4
    ( 380000),
    -- Group 5: idx will be 5
    ( 1050000);
END
GO -- End of dbo.Groups IF block

---------------------------------------------------------------------------------
-- PLAYERS AND GROUPS TABLE BLOCK (Depends on Players and Groups)
---------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.PlayersAndGroups)
BEGIN
    PRINT 'Inserting initial data into dbo.PlayersAndGroups...'
    INSERT INTO dbo.PlayersAndGroups (PlayerIdx, GroupIdx) VALUES
    -- Group 1 (Leaderboard Pos 5)
    (11, 1), -- StarPilot_Ace is in Group 1
    (12, 1), -- GalaxyRunner is in Group 1
    (13, 1), -- CosmicDrifter is in Group 1

    -- Group 2 (Leaderboard Pos 2)
    (14, 2), -- ZeroGravity is in Group 2
    (15, 2), -- CometChaser is in Group 2

    -- Group 3 (Leaderboard Pos 15)
    (16, 3), -- NebulaNomad is in Group 3
    (17, 3), -- HyperDriveHero is in Group 3

    -- Group 4 (Leaderboard Pos 8)
    (18, 4), -- AstroBlaster is in Group 4
    (19, 4), -- VoidVagabond is in Group 4

    -- Group 5 (Leaderboard Pos 1)
    (20, 5), -- OrbitalOutlaw is in Group 5

    -- Player with multiple groups (Showing the N:M relationship)
    (11, 5), -- StarPilot_Ace is ALSO in Group 5
    (14, 3); -- ZeroGravity is ALSO in Group 3
END
GO -- End of dbo.PlayersAndGroups IF block

---------------------------------------------------------------------------------
-- RUNS INFO TABLE BLOCK (Depends on Players)
---------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.RunsInfo)
BEGIN
    PRINT 'Inserting initial data into dbo.RunsInfo...'
    -- NOTE: Added IsRunOver to the column list.
    INSERT INTO dbo.RunsInfo (PlayerIdx, CurrentScore, CurrentLevel, RunStopDate, ShieldLevel, BlasterCount, HP, IsRunOver) VALUES
    -- Run 1 - Player 11 (StarPilot_Ace) -> True (1)
    (11, 15000, 5, '2025-09-27', 2, 4, 85, 1),
    -- Run 2 - Player 12 (GalaxyRunner) -> True (1)
    (12, 5500, 3, '2025-09-26', 1, 2, 50, 1),
    -- Run 3 - Player 13 (CosmicDrifter) -> True (1)
    (13, 1000, 1, '2025-09-25', 0, 1, 100, 1),
    -- Run 4 - Player 14 (ZeroGravity) -> True (1)
    (14, 45000, 8, '2025-09-28', 5, 9, 10, 1),
    -- Run 5 - Player 15 (CometChaser) -> False (0)
    (15, 8200, 4, '2025-09-27', 3, 3, 75, 0),
    -- Run 6 - Player 16 (NebulaNomad) -> True (1)
    (16, 22000, 6, '2025-09-28', 4, 5, 60, 1),
    -- Run 7 - Player 17 (HyperDriveHero) -> True (1)
    (17, 33000, 7, '2025-09-27', 6, 7, 95, 1),
    -- Run 8 - Player 18 (AstroBlaster) -> True (1)
    (18, 2500, 2, '2025-09-26', 1, 2, 30, 1),
    -- Run 9 - Player 19 (VoidVagabond) -> False (0)
    (19, 61000, 9, '2025-09-28', 7, 8, 20, 0),
    -- Run 10 - Player 20 (OrbitalOutlaw) -> True (1)
    (20, 12500, 5, '2025-09-25', 2, 4, 45, 1);
END
GO -- End of dbo.RunsInfo IF block

IF NOT EXISTS (SELECT 1 FROM dbo.EnemiesInLastLevel)
BEGIN
    PRINT 'Inserting initial data into dbo.EnemiesInLastLevel...'
    -- Inserting records to match the specific data shown in the picture:
    INSERT INTO dbo.EnemiesInLastLevel (RunIdx, Name, Amount) VALUES
    -- Run 5 had 2 'basic' enemies
    (5, 'basic', 2),
    -- Run 9 had 6 'basic' enemies
    (9, 'basic', 6);
END
GO -- End of dbo.EnemiesInLastLevel IF block