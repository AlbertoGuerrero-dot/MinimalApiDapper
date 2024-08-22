CREATE TABLE [dbo].[Generos] (
    [id]     INT          IDENTITY (1, 1) NOT NULL,
    [Nombre] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Generos] PRIMARY KEY CLUSTERED ([id] ASC)
);

