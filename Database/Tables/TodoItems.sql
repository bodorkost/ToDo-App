CREATE TABLE [dbo].[TodoItems]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(500) NULL, 
    [Priority] INT NOT NULL, 
    [Responsible] NVARCHAR(50) NULL, 
    [Deadline] DATETIME NULL, 
    [Status] INT NULL, 
    [Category] INT NULL, 
    [ParentId] UNIQUEIDENTIFIER NULL, 
    [Created] DATETIME NOT NULL, 
    [Creator] NVARCHAR(50) NOT NULL, 
    [Modified] DATETIME NOT NULL, 
    [Modifier] NVARCHAR(50) NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0
)
