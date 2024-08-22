-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actores_ObtenerVariosPorId]
	-- Add the parameters for the stored procedure here
	@actoresIds ListadoEntero READONLY 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id
	FROM Actores 
	WHERE Id in (SELECT Id FROM @actoresIds);
END
