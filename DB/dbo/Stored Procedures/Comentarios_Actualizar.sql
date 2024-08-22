-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Comentarios_Actualizar]
	-- Add the parameters for the stored procedure here
	@Id INT,
	@PeliculaID INT,
	@Cuerpo NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Comentarios
	SET Cuerpo = @Cuerpo, PeliculaId = @PeliculaID
	WHERE id = @Id;
END
