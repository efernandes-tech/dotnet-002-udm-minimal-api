USE MinimalTasksDB

CREATE TABLE [dbo].Tasks(
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](255),
	[IsComplete] [varchar](100)
);

INSERT INTO [dbo].Tasks (Name, IsComplete)
VALUES ('Task One', 'complete');

INSERT INTO [dbo].Tasks (Name, IsComplete)
VALUES ('Task Two', 'in progress');

SELECT * FROM [dbo].Tasks
