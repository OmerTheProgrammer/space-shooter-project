CREATE TABLE [dbo].[RunsInfoTbl] (
    [idx]                 INT  IDENTITY (1, 1) NOT NULL,
    [PlayerIdx]           INT  NOT NULL,
    [CurrentScore]        INT  DEFAULT ((0)) NOT NULL,
    [CurrentLevel]        INT  DEFAULT ((1)) NOT NULL,
    [RunStopDate]         DATE NOT NULL,
    [CurrentShieldLevel]  INT  DEFAULT ((0)) NOT NULL,
    [CurrentBlasterCount] INT  DEFAULT ((1)) NOT NULL,
    [CurrentHP]           INT  DEFAULT ((100)) NOT NULL,
    [IsRunOver]           BIT  DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([idx] ASC),
    FOREIGN KEY ([PlayerIdx]) REFERENCES [dbo].[PlayersTbl] ([idx]) ON DELETE CASCADE,
    CONSTRAINT [CK_RunsInfoTbl_CurrentShieldLevel_Min] CHECK ([CurrentShieldLevel]>=(0)),
    CONSTRAINT [CK_RunsInfoTbl_CurrentBlasterCount_Range] CHECK ([CurrentBlasterCount]>=(1) AND [CurrentBlasterCount]<=(9)),
    CONSTRAINT [CK_RunsInfoTbl_CurrentHP_Min] CHECK ([CurrentHP]>=(0))
);

