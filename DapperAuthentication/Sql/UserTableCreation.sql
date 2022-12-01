CREATE TABLE [dbo].[User]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [UserName] NVARCHAR(256) NOT NULL,
    [NormalizedUserName] NVARCHAR(256) NOT NULL,
    [Email] NVARCHAR(256) NULL,
    [NormalizedEmail] NVARCHAR(256) NULL,
    [PasswordHash] NVARCHAR(MAX) NULL
)
 
GO
 
CREATE INDEX [IX_User_NormalizedUserName] ON [dbo].[User] ([NormalizedUserName])
 
GO
 
CREATE INDEX [IX_User_NormalizedEmail] ON [dbo].[User] ([NormalizedEmail])