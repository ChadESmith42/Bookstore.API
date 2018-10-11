IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksReviews_Reviews]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksReviews]'))
ALTER TABLE [dbo].[BooksReviews] DROP CONSTRAINT [FK_BooksReviews_Reviews]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksReviews_Books]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksReviews]'))
ALTER TABLE [dbo].[BooksReviews] DROP CONSTRAINT [FK_BooksReviews_Books]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksReviews_Authors]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksReviews]'))
ALTER TABLE [dbo].[BooksReviews] DROP CONSTRAINT [FK_BooksReviews_Authors]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksAuthors_Books]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksAuthors]'))
ALTER TABLE [dbo].[BooksAuthors] DROP CONSTRAINT [FK_BooksAuthors_Books]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksAuthors_Authors]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksAuthors]'))
ALTER TABLE [dbo].[BooksAuthors] DROP CONSTRAINT [FK_BooksAuthors_Authors]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Books_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Books]'))
ALTER TABLE [dbo].[Books] DROP CONSTRAINT [FK_Books_Categories]
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 10/8/2018 1:36:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reviews]') AND type in (N'U'))
DROP TABLE [dbo].[Reviews]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 10/8/2018 1:36:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
DROP TABLE [dbo].[Categories]
GO
/****** Object:  Table [dbo].[BooksReviews]    Script Date: 10/8/2018 1:36:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BooksReviews]') AND type in (N'U'))
DROP TABLE [dbo].[BooksReviews]
GO
/****** Object:  Table [dbo].[BooksAuthors]    Script Date: 10/8/2018 1:36:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BooksAuthors]') AND type in (N'U'))
DROP TABLE [dbo].[BooksAuthors]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 10/8/2018 1:36:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Books]') AND type in (N'U'))
DROP TABLE [dbo].[Books]
GO
/****** Object:  Table [dbo].[Authors]    Script Date: 10/8/2018 1:36:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Authors]') AND type in (N'U'))
DROP TABLE [dbo].[Authors]
GO
/****** Object:  Table [dbo].[Authors]    Script Date: 10/8/2018 1:36:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Authors]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Authors](
	[AuthorId] [int] IDENTITY(11,2) NOT NULL,
	[FirstName] [nchar](80) NOT NULL,
	[LastName] [nchar](80) NOT NULL,
	[HeadshotImageUrl] [nchar](100) NULL,
 CONSTRAINT [PK_Authors_1] PRIMARY KEY CLUSTERED 
(
	[AuthorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [dbo].[Authors] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[Books]    Script Date: 10/8/2018 1:36:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Books]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Books](
	[BookId] [int] IDENTITY(10,2) NOT NULL,
	[Title] [nchar](80) NOT NULL,
	[Description] [nchar](500) NULL,
	[PublishDate] [date] NOT NULL,
	[CoverImageUrl] [nchar](50) NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_Books_1] PRIMARY KEY CLUSTERED 
(
	[BookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [dbo].[Books] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[BooksAuthors]    Script Date: 10/8/2018 1:36:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BooksAuthors]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BooksAuthors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookId] [int] NOT NULL,
	[AuthorId] [int] NOT NULL,
 CONSTRAINT [PK_BooksAuthors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [dbo].[BooksAuthors] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[BooksReviews]    Script Date: 10/8/2018 1:36:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BooksReviews]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BooksReviews](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookId] [int] NOT NULL,
	[ReviewId] [int] NOT NULL,
	[AuthorId] [int] NOT NULL,
 CONSTRAINT [PK_BooksReviews] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [dbo].[BooksReviews] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 10/8/2018 1:36:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](100) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [dbo].[Categories] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 10/8/2018 1:36:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reviews]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Reviews](
	[ReviewId] [int] IDENTITY(1,1) NOT NULL,
	[ReviewText] [nvarchar](max) NOT NULL,
	[Rating] [float] NOT NULL,
	[PublishDate] [date] NOT NULL,
 CONSTRAINT [PK_Reviews] PRIMARY KEY CLUSTERED 
(
	[ReviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
ALTER AUTHORIZATION ON [dbo].[Reviews] TO  SCHEMA OWNER 
GO
SET IDENTITY_INSERT [dbo].[Authors] ON 

INSERT [dbo].[Authors] ([AuthorId], [FirstName], [LastName], [HeadshotImageUrl]) VALUES (11, N'David                                                                           ', N'Matson                                                                          ', N'none                                                                                                ')
INSERT [dbo].[Authors] ([AuthorId], [FirstName], [LastName], [HeadshotImageUrl]) VALUES (13, N'Robert                                                                          ', N'Heinlein                                                                        ', N'none                                                                                                ')
INSERT [dbo].[Authors] ([AuthorId], [FirstName], [LastName], [HeadshotImageUrl]) VALUES (15, N'James                                                                           ', N'Patterson                                                                       ', N'none                                                                                                ')
INSERT [dbo].[Authors] ([AuthorId], [FirstName], [LastName], [HeadshotImageUrl]) VALUES (17, N'Chad                                                                            ', N'Smith                                                                           ', N'none                                                                                                ')
SET IDENTITY_INSERT [dbo].[Authors] OFF
SET IDENTITY_INSERT [dbo].[Books] ON 

INSERT [dbo].[Books] ([BookId], [Title], [Description], [PublishDate], [CoverImageUrl], [CategoryId]) VALUES (10, N'Stranger In A Strange Land                                                      ', N'Book about a man raised on Mars and brought back to Earth. He starts a cult and is murdered by an even weirder cult.                                                                                                                                                                                                                                                                                                                                                                                                ', CAST(N'1967-01-01' AS Date), N'none                                              ', 4)
INSERT [dbo].[Books] ([BookId], [Title], [Description], [PublishDate], [CoverImageUrl], [CategoryId]) VALUES (12, N'10 Reasons To Hire Chad: He''s Really Awesome                                    ', N'A detailed look at why you should hire Chad Smith.                                                                                                                                                                                                                                                                                                                                                                                                                                                                  ', CAST(N'2018-10-01' AS Date), N'none                                              ', 2)
INSERT [dbo].[Books] ([BookId], [Title], [Description], [PublishDate], [CoverImageUrl], [CategoryId]) VALUES (14, N'Underappreciated: Working Hard and Learning Fast                                ', N'How to make the most of every opportunity when you are the FNG.                                                                                                                                                                                                                                                                                                                                                                                                                                                     ', CAST(N'2018-10-01' AS Date), N'none                                              ', 3)
SET IDENTITY_INSERT [dbo].[Books] OFF
SET IDENTITY_INSERT [dbo].[BooksAuthors] ON 

INSERT [dbo].[BooksAuthors] ([Id], [BookId], [AuthorId]) VALUES (1, 10, 13)
INSERT [dbo].[BooksAuthors] ([Id], [BookId], [AuthorId]) VALUES (2, 12, 17)
INSERT [dbo].[BooksAuthors] ([Id], [BookId], [AuthorId]) VALUES (3, 14, 17)
SET IDENTITY_INSERT [dbo].[BooksAuthors] OFF
SET IDENTITY_INSERT [dbo].[BooksReviews] ON 

INSERT [dbo].[BooksReviews] ([Id], [BookId], [ReviewId], [AuthorId]) VALUES (1, 14, 2, 17)
INSERT [dbo].[BooksReviews] ([Id], [BookId], [ReviewId], [AuthorId]) VALUES (2, 14, 3, 17)
INSERT [dbo].[BooksReviews] ([Id], [BookId], [ReviewId], [AuthorId]) VALUES (3, 14, 4, 17)
INSERT [dbo].[BooksReviews] ([Id], [BookId], [ReviewId], [AuthorId]) VALUES (4, 14, 5, 17)
SET IDENTITY_INSERT [dbo].[BooksReviews] OFF
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (1, N'Childrens                                                                                           ')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (2, N'Computer                                                                                            ')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (3, N'Auto/Biographies                                                                                    ')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (4, N'SciFi                                                                                               ')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (5, N'Classic Literature                                                                                  ')
SET IDENTITY_INSERT [dbo].[Categories] OFF
SET IDENTITY_INSERT [dbo].[Reviews] ON 

INSERT [dbo].[Reviews] ([ReviewId], [ReviewText], [Rating], [PublishDate]) VALUES (2, N'Hiring Chad is not a great book, but a great business decision.', 5, CAST(N'2018-10-02' AS Date))
INSERT [dbo].[Reviews] ([ReviewId], [ReviewText], [Rating], [PublishDate]) VALUES (3, N'A real page turner. Well documented and thought out.', 4.8, CAST(N'2018-10-02' AS Date))
INSERT [dbo].[Reviews] ([ReviewId], [ReviewText], [Rating], [PublishDate]) VALUES (4, N'I couldn''t put it down. I spilled glue on my hands as I picked it up.', 4.6, CAST(N'2018-10-02' AS Date))
INSERT [dbo].[Reviews] ([ReviewId], [ReviewText], [Rating], [PublishDate]) VALUES (5, N'What else is there to say? HIRE CHAD!', 4.7, CAST(N'2018-10-02' AS Date))
SET IDENTITY_INSERT [dbo].[Reviews] OFF
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Books_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Books]'))
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_Books_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Books_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Books]'))
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_Books_Categories]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksAuthors_Authors]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksAuthors]'))
ALTER TABLE [dbo].[BooksAuthors]  WITH CHECK ADD  CONSTRAINT [FK_BooksAuthors_Authors] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([AuthorId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksAuthors_Authors]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksAuthors]'))
ALTER TABLE [dbo].[BooksAuthors] CHECK CONSTRAINT [FK_BooksAuthors_Authors]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksAuthors_Books]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksAuthors]'))
ALTER TABLE [dbo].[BooksAuthors]  WITH CHECK ADD  CONSTRAINT [FK_BooksAuthors_Books] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([BookId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksAuthors_Books]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksAuthors]'))
ALTER TABLE [dbo].[BooksAuthors] CHECK CONSTRAINT [FK_BooksAuthors_Books]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksReviews_Authors]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksReviews]'))
ALTER TABLE [dbo].[BooksReviews]  WITH CHECK ADD  CONSTRAINT [FK_BooksReviews_Authors] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([AuthorId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksReviews_Authors]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksReviews]'))
ALTER TABLE [dbo].[BooksReviews] CHECK CONSTRAINT [FK_BooksReviews_Authors]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksReviews_Books]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksReviews]'))
ALTER TABLE [dbo].[BooksReviews]  WITH CHECK ADD  CONSTRAINT [FK_BooksReviews_Books] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([BookId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksReviews_Books]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksReviews]'))
ALTER TABLE [dbo].[BooksReviews] CHECK CONSTRAINT [FK_BooksReviews_Books]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksReviews_Reviews]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksReviews]'))
ALTER TABLE [dbo].[BooksReviews]  WITH NOCHECK ADD  CONSTRAINT [FK_BooksReviews_Reviews] FOREIGN KEY([ReviewId])
REFERENCES [dbo].[Reviews] ([ReviewId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BooksReviews_Reviews]') AND parent_object_id = OBJECT_ID(N'[dbo].[BooksReviews]'))
ALTER TABLE [dbo].[BooksReviews] NOCHECK CONSTRAINT [FK_BooksReviews_Reviews]
GO
