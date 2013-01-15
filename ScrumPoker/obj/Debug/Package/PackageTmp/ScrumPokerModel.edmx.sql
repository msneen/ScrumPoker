
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/14/2013 23:08:54
-- Generated from EDMX file: D:\Sneen\ScrumPoker\ScrumPoker\ScrumPokerModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-ScrumPoker-20130104200736];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_FinalEstimate_Project]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FinalEstimate] DROP CONSTRAINT [FK_FinalEstimate_Project];
GO
IF OBJECT_ID(N'[dbo].[FK_Project_UserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Project] DROP CONSTRAINT [FK_Project_UserProfile];
GO
IF OBJECT_ID(N'[dbo].[fk_RoleId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [fk_RoleId];
GO
IF OBJECT_ID(N'[dbo].[FK_TeamMember_Project]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeamMember] DROP CONSTRAINT [FK_TeamMember_Project];
GO
IF OBJECT_ID(N'[dbo].[FK_TeamMember_UserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeamMember] DROP CONSTRAINT [FK_TeamMember_UserProfile];
GO
IF OBJECT_ID(N'[dbo].[fk_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [fk_UserId];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FinalEstimate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FinalEstimate];
GO
IF OBJECT_ID(N'[dbo].[Project]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Project];
GO
IF OBJECT_ID(N'[dbo].[TeamMember]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TeamMember];
GO
IF OBJECT_ID(N'[dbo].[UserProfile]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserProfile];
GO
IF OBJECT_ID(N'[dbo].[webpages_Membership]', 'U') IS NOT NULL
    DROP TABLE [dbo].[webpages_Membership];
GO
IF OBJECT_ID(N'[dbo].[webpages_OAuthMembership]', 'U') IS NOT NULL
    DROP TABLE [dbo].[webpages_OAuthMembership];
GO
IF OBJECT_ID(N'[dbo].[webpages_Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[webpages_Roles];
GO
IF OBJECT_ID(N'[dbo].[webpages_UsersInRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[webpages_UsersInRoles];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Memberships'
CREATE TABLE [dbo].[Memberships] (
    [UserId] int  NOT NULL,
    [CreateDate] datetime  NULL,
    [ConfirmationToken] nvarchar(128)  NULL,
    [IsConfirmed] bit  NULL,
    [LastPasswordFailureDate] datetime  NULL,
    [PasswordFailuresSinceLastSuccess] int  NOT NULL,
    [Password] nvarchar(128)  NOT NULL,
    [PasswordChangedDate] datetime  NULL,
    [PasswordSalt] nvarchar(128)  NOT NULL,
    [PasswordVerificationToken] nvarchar(128)  NULL,
    [PasswordVerificationTokenExpirationDate] datetime  NULL
);
GO

-- Creating table 'OAuthMemberships'
CREATE TABLE [dbo].[OAuthMemberships] (
    [Provider] nvarchar(30)  NOT NULL,
    [ProviderUserId] nvarchar(100)  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Roles1'
CREATE TABLE [dbo].[Roles1] (
    [RoleId] int IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'UsersInRoles'
CREATE TABLE [dbo].[UsersInRoles] (
    [UserId] int  NOT NULL,
    [RoleId] int  NOT NULL
);
GO

-- Creating table 'UserProfileScrums'
CREATE TABLE [dbo].[UserProfileScrums] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NULL
);
GO

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [id] int IDENTITY(1,1) NOT NULL,
    [ProjectName] varchar(50)  NOT NULL,
    [CreatedUserId] int  NOT NULL
);
GO

-- Creating table 'TeamMembers'
CREATE TABLE [dbo].[TeamMembers] (
    [UserId] int  NULL,
    [ProjectId] int  NOT NULL,
    [NickName] varchar(50)  NULL,
    [id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'FinalEstimates'
CREATE TABLE [dbo].[FinalEstimates] (
    [id] int IDENTITY(1,1) NOT NULL,
    [TaskId] varchar(100)  NOT NULL,
    [Estimate] decimal(10,2)  NULL,
    [ProjectId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [UserId] in table 'Memberships'
ALTER TABLE [dbo].[Memberships]
ADD CONSTRAINT [PK_Memberships]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [Provider], [ProviderUserId] in table 'OAuthMemberships'
ALTER TABLE [dbo].[OAuthMemberships]
ADD CONSTRAINT [PK_OAuthMemberships]
    PRIMARY KEY CLUSTERED ([Provider], [ProviderUserId] ASC);
GO

-- Creating primary key on [RoleId] in table 'Roles1'
ALTER TABLE [dbo].[Roles1]
ADD CONSTRAINT [PK_Roles1]
    PRIMARY KEY CLUSTERED ([RoleId] ASC);
GO

-- Creating primary key on [UserId], [RoleId] in table 'UsersInRoles'
ALTER TABLE [dbo].[UsersInRoles]
ADD CONSTRAINT [PK_UsersInRoles]
    PRIMARY KEY NONCLUSTERED ([UserId], [RoleId] ASC);
GO

-- Creating primary key on [UserId] in table 'UserProfileScrums'
ALTER TABLE [dbo].[UserProfileScrums]
ADD CONSTRAINT [PK_UserProfileScrums]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'TeamMembers'
ALTER TABLE [dbo].[TeamMembers]
ADD CONSTRAINT [PK_TeamMembers]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'FinalEstimates'
ALTER TABLE [dbo].[FinalEstimates]
ADD CONSTRAINT [PK_FinalEstimates]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RoleId] in table 'UsersInRoles'
ALTER TABLE [dbo].[UsersInRoles]
ADD CONSTRAINT [fk_RoleId]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles1]
        ([RoleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'fk_RoleId'
CREATE INDEX [IX_fk_RoleId]
ON [dbo].[UsersInRoles]
    ([RoleId]);
GO

-- Creating foreign key on [UserId] in table 'UsersInRoles'
ALTER TABLE [dbo].[UsersInRoles]
ADD CONSTRAINT [FK_UsersInRolesUserProfileScrum]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserProfileScrums]
        ([UserId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CreatedUserId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_Project_UserProfile]
    FOREIGN KEY ([CreatedUserId])
    REFERENCES [dbo].[UserProfileScrums]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Project_UserProfile'
CREATE INDEX [IX_FK_Project_UserProfile]
ON [dbo].[Projects]
    ([CreatedUserId]);
GO

-- Creating foreign key on [ProjectId] in table 'TeamMembers'
ALTER TABLE [dbo].[TeamMembers]
ADD CONSTRAINT [FK_TeamMember_Project]
    FOREIGN KEY ([ProjectId])
    REFERENCES [dbo].[Projects]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamMember_Project'
CREATE INDEX [IX_FK_TeamMember_Project]
ON [dbo].[TeamMembers]
    ([ProjectId]);
GO

-- Creating foreign key on [UserId] in table 'TeamMembers'
ALTER TABLE [dbo].[TeamMembers]
ADD CONSTRAINT [FK_TeamMember_UserProfile]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserProfileScrums]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamMember_UserProfile'
CREATE INDEX [IX_FK_TeamMember_UserProfile]
ON [dbo].[TeamMembers]
    ([UserId]);
GO

-- Creating foreign key on [ProjectId] in table 'FinalEstimates'
ALTER TABLE [dbo].[FinalEstimates]
ADD CONSTRAINT [FK_FinalEstimate_Project]
    FOREIGN KEY ([ProjectId])
    REFERENCES [dbo].[Projects]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FinalEstimate_Project'
CREATE INDEX [IX_FK_FinalEstimate_Project]
ON [dbo].[FinalEstimates]
    ([ProjectId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------