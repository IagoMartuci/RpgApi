BEGIN TRANSACTION;
GO

CREATE TABLE [Armas] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(max) NOT NULL,
    [Dano] int NOT NULL,
    CONSTRAINT [PK_Armas] PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Dano', N'Nome') AND [object_id] = OBJECT_ID(N'[Armas]'))
    SET IDENTITY_INSERT [Armas] ON;
INSERT INTO [Armas] ([Id], [Dano], [Nome])
VALUES (1, 25, N'Espada de Duas Mãos'),
(2, 25, N'Machado de Duas Mãos'),
(3, 25, N'Clava de Duas Mãos'),
(4, 20, N'Espada e Escudo'),
(5, 20, N'Machado e Escudo'),
(6, 20, N'Clava e Escudo'),
(7, 15, N'Cajado Elemental');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Dano', N'Nome') AND [object_id] = OBJECT_ID(N'[Armas]'))
    SET IDENTITY_INSERT [Armas] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220927044212_MigrationArmas', N'6.0.9');
GO

COMMIT;
GO

