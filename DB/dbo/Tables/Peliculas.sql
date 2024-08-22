CREATE TABLE [dbo].[Peliculas] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [Titulo]           NVARCHAR (50)  NOT NULL,
    [EnCines]          BIT            NOT NULL,
    [FechaLanzamiento] DATETIME2 (7)  NOT NULL,
    [Poster]           NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Peliculas] PRIMARY KEY CLUSTERED ([id] ASC)
);

