CREATE TABLE [dbo].[User]
(
    [UserID]               INT IDENTITY (1000, 1) NOT NULL,
    [UserName]             NVARCHAR (256)         NOT NULL,
    [Email]                NVARCHAR (256)         NULL,
    [EmailConfirmed]       [dbo].[Flag]           NOT NULL,
    [PasswordHash]         NVARCHAR (MAX)         NULL,
    [SecurityStamp]        NVARCHAR (MAX)         NULL,
    [PhoneNumber]          NVARCHAR (MAX)         NULL,
    [PhoneNumberConfirmed] [dbo].[Flag]           NOT NULL,
    [TwoFactorEnabled]     [dbo].[Flag]           NOT NULL,
    [LockoutEndDateUtc]    DATETIME               NULL,
    [LockoutEnabled]       [dbo].[Flag]           NOT NULL,
    [AccessFailedCount]    INT                    NOT NULL,

    CONSTRAINT [PK_User_UserID] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [UK_User_UserName] UNIQUE NONCLUSTERED ([UserName] ASC)
);
