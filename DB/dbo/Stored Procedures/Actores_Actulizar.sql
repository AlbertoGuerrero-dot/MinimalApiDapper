-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE Actores_Actulizar
	-- Add the parameters for the stored procedure here
	@Id int,
	@Nombre nvarchar(50),
	@Foto nvarchar(MAX),
	@FechaNacmiento  datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Actores
	SET Nombre = @Nombre, Foto = @Foto, FechaNacmiento = @FechaNacmiento
	WHERE id = @Id;
END
