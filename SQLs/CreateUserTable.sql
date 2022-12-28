USE DeveloperTest;

CREATE TABLE [dbo].[AmirhosseinUser]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [UserName] NVARCHAR(256) NOT NULL,
    [NormalizedUserName] NVARCHAR(256) NOT NULL,
    [Email] NVARCHAR(256) NULL,
    [NormalizedEmail] NVARCHAR(256) NULL,
    [PasswordHash] NVARCHAR(MAX) NULL
);

CREATE INDEX [IX_User_NormalizedUserName] ON [dbo].[AmirhosseinUser] ([NormalizedUserName]);

CREATE INDEX [IX_User_NormalizedEmail] ON [dbo].[AmirhosseinUser] ([NormalizedEmail]);