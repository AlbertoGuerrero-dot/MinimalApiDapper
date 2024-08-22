-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE Generos_ObtenerVariosPorId 
	-- Add the parameters for the stored procedure here
	@GenerosIds ListadoEntero READONLY 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT id FROM Generos
	WHERE id in (SELECT Id from @GenerosIds);
END
