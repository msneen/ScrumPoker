USE [aspnet-ScrumPoker-20130104200736]
GO
/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 01/21/2013 19:46:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[webpages_Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[webpages_Roles] ON
INSERT [dbo].[webpages_Roles] ([RoleId], [RoleName]) VALUES (1, N'Developer')
INSERT [dbo].[webpages_Roles] ([RoleId], [RoleName]) VALUES (2, N'ScrumMaster')
INSERT [dbo].[webpages_Roles] ([RoleId], [RoleName]) VALUES (3, N'SiteAdmin')
SET IDENTITY_INSERT [dbo].[webpages_Roles] OFF
/****** Object:  Table [dbo].[UserProfile]    Script Date: 01/21/2013 19:46:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserProfile]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserProfile](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[UserProfile] ON
INSERT [dbo].[UserProfile] ([UserId], [UserName]) VALUES (5, N'michael.sneen@gmail.com')
INSERT [dbo].[UserProfile] ([UserId], [UserName]) VALUES (6, N'JJohnston')
INSERT [dbo].[UserProfile] ([UserId], [UserName]) VALUES (7, N'msneen@practicegenius.com')
INSERT [dbo].[UserProfile] ([UserId], [UserName]) VALUES (8, N'msneen@practicegenius.com1')
INSERT [dbo].[UserProfile] ([UserId], [UserName]) VALUES (9, N'dang.h.le@gmail.com')
INSERT [dbo].[UserProfile] ([UserId], [UserName]) VALUES (10, N'JasonTolliver')
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
/****** Object:  Table [dbo].[webpages_OAuthMembership]    Script Date: 01/21/2013 19:46:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[webpages_OAuthMembership]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[webpages_OAuthMembership](
	[Provider] [nvarchar](30) NOT NULL,
	[ProviderUserId] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Provider] ASC,
	[ProviderUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [dbo].[webpages_OAuthMembership] ([Provider], [ProviderUserId], [UserId]) VALUES (N'google', N'https://www.google.com/accounts/o8/id?id=AItOawk41P5veGM9yDIip0mxkA-EuYERQ4fUx-U', 7)
INSERT [dbo].[webpages_OAuthMembership] ([Provider], [ProviderUserId], [UserId]) VALUES (N'google', N'https://www.google.com/accounts/o8/id?id=AItOawl4RjA1VQvFruxq0-_K__7KlglyIbXhrIc', 8)
INSERT [dbo].[webpages_OAuthMembership] ([Provider], [ProviderUserId], [UserId]) VALUES (N'google', N'https://www.google.com/accounts/o8/id?id=AItOawltiChEJ9jW-bZ8Rksql8aQlfoFgWrQT38', 5)
INSERT [dbo].[webpages_OAuthMembership] ([Provider], [ProviderUserId], [UserId]) VALUES (N'google', N'https://www.google.com/accounts/o8/id?id=AItOawmwm77F-jNsaxu5nBBgZEIrp0baLnDMNyk', 9)
INSERT [dbo].[webpages_OAuthMembership] ([Provider], [ProviderUserId], [UserId]) VALUES (N'google', N'https://www.google.com/accounts/o8/id?id=AItOawn5bLA62fUPbfBfAZ2qeMPY7wKrDpxcryA', 8)
/****** Object:  Table [dbo].[webpages_Membership]    Script Date: 01/21/2013 19:46:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[webpages_Membership]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[webpages_Membership](
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) NULL,
	[IsConfirmed] [bit] NULL,
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (6, CAST(0x0000A13F003BAB13 AS DateTime), NULL, 1, NULL, 0, N'AEx+LYOw7zicKcLq23MS40CAAndsGbVSur4u3phEENM6GgbZHkhyr8cqW8DUY1QU3w==', CAST(0x0000A13F003BAB13 AS DateTime), N'', NULL, NULL)
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (10, CAST(0x0000A14D01516B59 AS DateTime), NULL, 1, NULL, 0, N'APZH8Ah9XNcJpkmpGuaNjI7cU02KcGaGO4/zq8qd3o9CjHxdqDF+Yhn6FJ38dnYLGg==', CAST(0x0000A14D01516B59 AS DateTime), N'', NULL, NULL)
/****** Object:  Table [dbo].[webpages_UsersInRoles]    Script Date: 01/21/2013 19:46:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[webpages_UsersInRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[webpages_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (5, 1)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (5, 2)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (5, 3)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (6, 1)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (6, 2)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (6, 3)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (7, 1)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (7, 2)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (7, 3)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (8, 1)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (8, 2)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (9, 1)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (9, 2)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (9, 3)
/****** Object:  Table [dbo].[TeamMember]    Script Date: 01/21/2013 19:46:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TeamMember]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TeamMember](
	[UserId] [int] NULL,
	[ProjectId] [int] NOT NULL,
	[NickName] [varchar](50) NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_TeamMember_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[TeamMember] ON
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 7, N'Dang', 1)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 7, N'Henry', 2)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 7, N'Mark', 3)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 7, N'Mike', 4)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 7, N'Roxy', 5)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (9, 5, N'Dang', 6)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 5, N'Mike', 7)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 5, N'Roxy', 8)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 5, N'Vivi', 10)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 2, N'Freddie', 11)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 2, N'Jason', 12)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 2, N'Mike', 13)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 2, N'Ray', 14)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 1, N'Jason', 15)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 1, N'Freddie', 16)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 1, N'Ray', 17)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 1, N'Mike', 18)
INSERT [dbo].[TeamMember] ([UserId], [ProjectId], [NickName], [id]) VALUES (NULL, 5, N'Ryann', 19)
SET IDENTITY_INSERT [dbo].[TeamMember] OFF
/****** Object:  Table [dbo].[FinalEstimate]    Script Date: 01/21/2013 19:46:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FinalEstimate]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FinalEstimate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[TaskId] [varchar](100) NOT NULL,
	[Estimate] [decimal](10, 2) NULL,
	[ProjectId] [int] NOT NULL,
 CONSTRAINT [PK_FinalEstimate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[FinalEstimate] ON
INSERT [dbo].[FinalEstimate] ([id], [TaskId], [Estimate], [ProjectId]) VALUES (22, N'Hub-2', CAST(2.00 AS Decimal(10, 2)), 7)
SET IDENTITY_INSERT [dbo].[FinalEstimate] OFF
/****** Object:  Table [dbo].[Project]    Script Date: 01/21/2013 19:46:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Project]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Project](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectName] [varchar](50) NOT NULL,
	[CreatedUserId] [int] NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[Project] ON
INSERT [dbo].[Project] ([id], [ProjectName], [CreatedUserId]) VALUES (1, N'Syntility', 5)
INSERT [dbo].[Project] ([id], [ProjectName], [CreatedUserId]) VALUES (2, N'AGame', 5)
INSERT [dbo].[Project] ([id], [ProjectName], [CreatedUserId]) VALUES (3, N'F Game', 5)
INSERT [dbo].[Project] ([id], [ProjectName], [CreatedUserId]) VALUES (4, N'FRG', 5)
INSERT [dbo].[Project] ([id], [ProjectName], [CreatedUserId]) VALUES (5, N'Scrum Poker', 5)
INSERT [dbo].[Project] ([id], [ProjectName], [CreatedUserId]) VALUES (6, N'Dev1', 8)
INSERT [dbo].[Project] ([id], [ProjectName], [CreatedUserId]) VALUES (7, N'Dev2', 8)
INSERT [dbo].[Project] ([id], [ProjectName], [CreatedUserId]) VALUES (8, N'Dev3', 8)
SET IDENTITY_INSERT [dbo].[Project] OFF
/****** Object:  Default [DF__webpages___IsCon__014935CB]    Script Date: 01/21/2013 19:46:42 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__webpages___IsCon__014935CB]') AND parent_object_id = OBJECT_ID(N'[dbo].[webpages_Membership]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__webpages___IsCon__014935CB]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [IsConfirmed]
END


End
GO
/****** Object:  Default [DF__webpages___Passw__023D5A04]    Script Date: 01/21/2013 19:46:42 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__webpages___Passw__023D5A04]') AND parent_object_id = OBJECT_ID(N'[dbo].[webpages_Membership]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__webpages___Passw__023D5A04]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [PasswordFailuresSinceLastSuccess]
END


End
GO
/****** Object:  ForeignKey [FK_FinalEstimate_Project]    Script Date: 01/21/2013 19:46:42 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FinalEstimate_Project]') AND parent_object_id = OBJECT_ID(N'[dbo].[FinalEstimate]'))
ALTER TABLE [dbo].[FinalEstimate]  WITH CHECK ADD  CONSTRAINT [FK_FinalEstimate_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FinalEstimate_Project]') AND parent_object_id = OBJECT_ID(N'[dbo].[FinalEstimate]'))
ALTER TABLE [dbo].[FinalEstimate] CHECK CONSTRAINT [FK_FinalEstimate_Project]
GO
/****** Object:  ForeignKey [FK_Project_UserProfile]    Script Date: 01/21/2013 19:46:42 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Project_UserProfile]') AND parent_object_id = OBJECT_ID(N'[dbo].[Project]'))
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_UserProfile] FOREIGN KEY([CreatedUserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Project_UserProfile]') AND parent_object_id = OBJECT_ID(N'[dbo].[Project]'))
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_UserProfile]
GO
/****** Object:  ForeignKey [FK_TeamMember_Project]    Script Date: 01/21/2013 19:46:42 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TeamMember_Project]') AND parent_object_id = OBJECT_ID(N'[dbo].[TeamMember]'))
ALTER TABLE [dbo].[TeamMember]  WITH CHECK ADD  CONSTRAINT [FK_TeamMember_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TeamMember_Project]') AND parent_object_id = OBJECT_ID(N'[dbo].[TeamMember]'))
ALTER TABLE [dbo].[TeamMember] CHECK CONSTRAINT [FK_TeamMember_Project]
GO
/****** Object:  ForeignKey [FK_TeamMember_UserProfile]    Script Date: 01/21/2013 19:46:42 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TeamMember_UserProfile]') AND parent_object_id = OBJECT_ID(N'[dbo].[TeamMember]'))
ALTER TABLE [dbo].[TeamMember]  WITH CHECK ADD  CONSTRAINT [FK_TeamMember_UserProfile] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TeamMember_UserProfile]') AND parent_object_id = OBJECT_ID(N'[dbo].[TeamMember]'))
ALTER TABLE [dbo].[TeamMember] CHECK CONSTRAINT [FK_TeamMember_UserProfile]
GO
/****** Object:  ForeignKey [fk_RoleId]    Script Date: 01/21/2013 19:46:42 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[webpages_UsersInRoles]'))
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[webpages_UsersInRoles]'))
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_RoleId]
GO
/****** Object:  ForeignKey [fk_UserId]    Script Date: 01/21/2013 19:46:42 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[webpages_UsersInRoles]'))
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[webpages_UsersInRoles]'))
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_UserId]
GO
