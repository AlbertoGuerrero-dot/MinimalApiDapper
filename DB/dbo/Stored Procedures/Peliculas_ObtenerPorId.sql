-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Peliculas_ObtenerPorId]
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Peliculas
	WHERE id = @Id;

	SELECT * FROM Comentarios
	WHERE PeliculaId = @Id;

	SELECT id, Nombre fROM Generos
	INNER JOIN GenerosPeliculas 
	ON GenerosPeliculas.GeneroId = id
	WHERE PeliculaId = @Id

	SELECT id, Nombre, Personaje 
	FROM Actores 
	INNER JOIN ActoresPeliculas
	ON ActoresPeliculas.ActorId = id 
	WHERE PeliculaId = @Id 
	ORDER BY Orden;
END
