-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actores_Crear]
	-- Add the parameters for the stored procedure here
	@Nombre nvarchar(50), 
	@FechaNacmiento  datetime2,
	@Foto nvarchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Actores (Nombre, FechaNacmiento, Foto) 
	VALUES (@Nombre, @FechaNacmiento, @Foto);
	SELECT SCOPE_IDENTITY();
END
