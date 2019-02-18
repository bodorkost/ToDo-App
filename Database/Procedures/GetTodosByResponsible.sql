CREATE PROCEDURE [dbo].[GetTodosByResponsible]
	@Responsible nvarchar(50)
AS
BEGIN
	SELECT * FROM TodoItems WHERE Responsible = @Responsible;
END
