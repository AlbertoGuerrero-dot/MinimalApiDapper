-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE Peliculas_ObtenerTodas
	-- Add the parameters for the stored procedure here
	@Page int,
	@RecordsPerPage int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Peliculas
	ORDER BY Titulo
	OFFSET ((@Page -1)*@RecordsPerPage) ROWS FETCH NEXT @RecordsPerPage ROWS ONLY;
END
