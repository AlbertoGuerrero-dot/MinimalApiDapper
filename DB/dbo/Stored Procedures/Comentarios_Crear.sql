-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Comentarios_Crear]
	-- Add the parameters for the stored procedure here
	@Cuerpo NVARCHAR(MAX),
	@PeliculaId INT,
	@UserId nvarchar(450)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Comentarios (Cuerpo, PeliculaId, UserId) 
	VALUES (@Cuerpo, @PeliculaId, @UserId)

	SELECT SCOPE_IDENTITY();
END
