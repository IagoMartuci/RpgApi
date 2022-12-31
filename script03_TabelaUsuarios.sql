BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Personagens]') AND [c].[name] = N'Nome');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Personagens] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Personagens] ALTER COLUMN [Nome] nvarchar(max) NULL;
GO

ALTER TABLE [Personagens] ADD [FotoPersonagem] varbinary(max) NULL;
GO

ALTER TABLE [Personagens] ADD [UsuarioId] int NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Armas]') AND [c].[name] = N'Nome');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Armas] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Armas] ALTER COLUMN [Nome] nvarchar(max) NULL;
GO

CREATE TABLE [Usuarios] (
    [Id] int NOT NULL IDENTITY,
    [UserName] nvarchar(max) NULL,
    [PasswordHash] varbinary(max) NULL,
    [PasswordSalt] varbinary(max) NULL,
    [Foto] varbinary(max) NULL,
    [Latitude] float NULL,
    [Longitude] float NULL,
    [DataAcesso] datetime2 NULL,
    [Perfil] nvarchar(max) NULL DEFAULT N'Jogador',
    [Email] nvarchar(max) NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataAcesso', N'Email', N'Foto', N'Latitude', N'Longitude', N'PasswordHash', N'PasswordSalt', N'Perfil', N'UserName') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
    SET IDENTITY_INSERT [Usuarios] ON;
INSERT INTO [Usuarios] ([Id], [DataAcesso], [Email], [Foto], [Latitude], [Longitude], [PasswordHash], [PasswordSalt], [Perfil], [UserName])
VALUES (1, NULL, NULL, NULL, NULL, NULL, 0xABD3B874482FBAE9C940686E07230E7D845E34D6796056EA09DFC8399217D4C3C10837EE29C8B043A1AD64DA8452691F7CEF41E8B7D66D1B1553427368B76B70, 0xEBF5F0B961F859D5F651B89F42E8360A93419761EAB365E687695740990EF51688BE5ED825C2C4551F359B50262801259AB7F601381A390D7A5BD635BBA58344A5C7D727CAC66F12CEF72B36C4C605384336B56AC3C7ACA356DE9FAF76D90971AF011BA96781B60486B10730B20B4B54286B753B209BC85799909DFF7AAE5159, N'Admin', N'UsuarioAdmin');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataAcesso', N'Email', N'Foto', N'Latitude', N'Longitude', N'PasswordHash', N'PasswordSalt', N'Perfil', N'UserName') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
    SET IDENTITY_INSERT [Usuarios] OFF;
GO

CREATE INDEX [IX_Personagens_UsuarioId] ON [Personagens] ([UsuarioId]);
GO

ALTER TABLE [Personagens] ADD CONSTRAINT [FK_Personagens_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221004011722_MigracaoUsuario', N'6.0.9');
GO

COMMIT;
GO

