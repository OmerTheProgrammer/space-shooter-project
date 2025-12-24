-- =============================================
-- DEEP RESET SCRIPT: FORCING PHYSICAL ALIGNMENT
-- =============================================

-- 1. CLEAR DATA
DELETE FROM [dbo].[RequestsDataTbl];

-- 2. RESET IDENTITY COUNTERS
DBCC CHECKIDENT ('[dbo].[RequestsDataTbl]', RESEED, 0);

-- 3. THE PHYSICAL FIX: REORGANIZE STORAGE
-- Rebuilding with a FillFactor of 100 forces SQL to pack rows 
-- perfectly at the end of the file, preventing the "13 after 1" physical jump.
ALTER INDEX ALL ON [dbo].[RequestsDataTbl] REBUILD WITH (FILLFACTOR = 100);

PRINT 'Seeding RequestsDataTbl (1-10)...';
INSERT INTO dbo.RequestsDataTbl (RequestIdx, Field, OldValue, NewValue) VALUES
(1, 'Username', 'StarPilot_Ace', 'AcePilot_77'),           
(2, 'Email', 'zerog@game.com', 'zero_gravity_main@game.com'), 
(3, 'Password', 'hashed_pass_P15', 'new_hashed_pass_P15'),    
(4, 'Birthday', '1998-08-08', '1997-08-08'),                  
(5, 'Username', 'GalaxyRunner', 'TheGalacticOne'),            
(6, 'Email', 'blastr.a@game.com', 'astro_blast@game.com'),    
(7, 'Email', 'hero.h@game.com', 'hyperhero@game.com'),        
(8, 'Password', 'hashed_pass_P20', 'another_new_hash'),       
(9, 'Username', 'NebulaNomad', 'NebulaNomad_X'),              
(10, 'Birthday', '2003-12-03', '2003-12-04');                 

-- 5. VERIFICATION
-- If the tool still shows 13 after 1, it is 100% a tool setting (Lexicographical view).
-- The CAST ensures we test if the DB sees them as numbers.
PRINT '--------------------------------------------------';
SELECT * FROM [dbo].[RequestsDataTbl] ORDER BY idx ASC;