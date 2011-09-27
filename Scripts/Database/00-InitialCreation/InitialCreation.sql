CREATE TABLE [Family] (
[PkID] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
[FamilyName] VARCHAR(20)  UNIQUE NOT NULL,
[Description] TEXT  NULL
)

CREATE TABLE [UserFamily] (
[UserID] INTEGER  NULL,
[FamilyID] INTEGER  NULL
)

CREATE TABLE [Blog] (
[PkID] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
[AuthorID] INTEGER  NOT NULL,
[Title] VARCHAR(50) NOT NULL,
[Entry] TEXT  NULL,
[Tags] VARCHAR(50)  NULL,
[DatePublished] TIMESTAMP  NULL
)

CREATE TABLE [BlogComment] (
[PkID] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
[BlogID] INTEGER  NOT NULL,
[AuthorID] INTEGER  NOT NULL,
[Comment] TEXT  NOT NULL,
[DatePublished] TIMESTAMP  NULL
)