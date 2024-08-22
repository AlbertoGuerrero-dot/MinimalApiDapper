CREATE TABLE [dbo].[Actores] (
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]         NVARCHAR (50)  NOT NULL,
    [Foto]           NVARCHAR (MAX) NULL,
    [FechaNacmiento] DATETIME2 (7)  NOT NULL
);

