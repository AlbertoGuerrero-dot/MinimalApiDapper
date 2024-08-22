-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Peliculas_Cantidad]
	-- Add the parameters for the stored procedure here
	@titulo  nvarchar(150) = '',
	@generoId int = 0,
	@proximoEstrenos bit = 'False' ,
	@enCines bit = 'False'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*)
	FROM Peliculas
	WHERE (Titulo LIKE '%' + @titulo +'%' OR @titulo = '')
	AND (FechaLanzamiento > GETDATE() OR @proximoEstrenos = 'False')
	AND (EnCines = 'True' OR @enCines = 'False')
	AND (id in (SELECT PeliculaId FROM GenerosPeliculas WHERE GeneroId = @generoId) OR @generoId = 0)
END
