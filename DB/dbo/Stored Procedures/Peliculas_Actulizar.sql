-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE Peliculas_Actulizar
	-- Add the parameters for the stored procedure here
	@Id int,
	@Titulo nvarchar(50),
	@EnCines bit,
	@FechaLanzamiento datetime,
	@Poster nvarchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Peliculas
	SET Titulo = @Titulo, FechaLanzamiento = @FechaLanzamiento, Poster = @Poster,
	EnCines = @EnCines WHERE id = @Id; 
END
