CREATE TABLE [dbo].[ActoresPeliculas] (
    [ActorId]    INT            NOT NULL,
    [PeliculaId] INT            NOT NULL,
    [Orden]      INT            NOT NULL,
    [Personaje]  NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_ActoresPeliculas] PRIMARY KEY CLUSTERED ([ActorId] ASC, [PeliculaId] ASC),
    CONSTRAINT [FK_ActoresPeliculas_Peliculas] FOREIGN KEY ([PeliculaId]) REFERENCES [dbo].[Peliculas] ([id]) ON DELETE CASCADE
);

