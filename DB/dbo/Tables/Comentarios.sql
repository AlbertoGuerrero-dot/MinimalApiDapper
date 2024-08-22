CREATE TABLE [dbo].[Comentarios] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Cuerpo]     NVARCHAR (MAX) NOT NULL,
    [PeliculaId] INT            NOT NULL,
    [UserId]     NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_Comentarios] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Comentarios_Peliculas] FOREIGN KEY ([PeliculaId]) REFERENCES [dbo].[Peliculas] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Comentarios_Usuarios] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Usuarios] ([Id]) ON DELETE CASCADE
);

