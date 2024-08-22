-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE Peliculas_AsignarGeneros
	-- Add the parameters for the stored procedure here
	@PeliculaId int,
	@GenerosIds ListadoEntero READONLY 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE GenerosPeliculas WHERE PeliculaId = @PeliculaId;
	INSERT INTO GenerosPeliculas (GeneroId, PeliculaId)
	SELECT Id, @PeliculaId FROM @GenerosIds
END
