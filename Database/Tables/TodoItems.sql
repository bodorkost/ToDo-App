CREATE TABLE [dbo].[TodoItems]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(500) NULL, 
    [Priority] INT NOT NULL, 
    [Responsible] NVARCHAR(50) NULL, 
    [Deadline] DATETIME2 NULL, 
    [Status] INT NULL, 
    [CategoryId] UNIQUEIDENTIFIER NULL, 
    [ParentId] UNIQUEIDENTIFIER NULL, 
    [Created] DATETIME2 NOT NULL, 
    [CreatedById] UNIQUEIDENTIFIER NULL, 
    [Modified] DATETIME2 NOT NULL, 
    [ModifiedById] UNIQUEIDENTIFIER NULL, 
    [Deleted] DATETIME2 NULL, 
    [DeletedById] UNIQUEIDENTIFIER NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_TodoItems_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [Categories]([Id])
)
