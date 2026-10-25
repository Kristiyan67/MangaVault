IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Authors] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Authors] PRIMARY KEY ([Id])
);

CREATE TABLE [Genres] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Genres] PRIMARY KEY ([Id])
);

CREATE TABLE [Series] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(150) NOT NULL,
    [Description] nvarchar(2000) NULL,
    [CoverUrl] nvarchar(500) NULL,
    [PublicationStatus] int NOT NULL,
    [TotalVolumes] int NULL,
    [ReadingStatus] int NOT NULL,
    [Rating] int NULL,
    [Notes] nvarchar(1000) NULL,
    [AuthorId] int NOT NULL,
    CONSTRAINT [PK_Series] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Series_Authors_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Authors] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [SeriesGenres] (
    [SeriesId] int NOT NULL,
    [GenreId] int NOT NULL,
    CONSTRAINT [PK_SeriesGenres] PRIMARY KEY ([SeriesId], [GenreId]),
    CONSTRAINT [FK_SeriesGenres_Genres_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [Genres] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SeriesGenres_Series_SeriesId] FOREIGN KEY ([SeriesId]) REFERENCES [Series] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Volumes] (
    [Id] int NOT NULL IDENTITY,
    [Number] int NOT NULL,
    [ReleaseDate] datetime2 NULL,
    [IsOwned] bit NOT NULL,
    [PurchasePrice] decimal(8,2 NULL,
    [SeriesId] int NOT NULL,
    CONSTRAINT [PK_Volumes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Volumes_Series_SeriesId] FOREIGN KEY ([SeriesId]) REFERENCES [Series] ([Id]) ON DELETE CASCADE
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Genres]'))
    SET IDENTITY_INSERT [Genres] ON;
INSERT INTO [Genres] ([Id], [Name])
VALUES (1, N'Shonen'),
(2, N'Seinen'),
(3, N'Shojo'),
(4, N'Isekai');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Genres]'))
    SET IDENTITY_INSERT [Genres] OFF;

CREATE UNIQUE INDEX [IX_Genres_Name] ON [Genres] ([Name]);

CREATE INDEX [IX_Series_AuthorId] ON [Series] ([AuthorId]);

CREATE INDEX [IX_SeriesGenres_GenreId] ON [SeriesGenres] ([GenreId]);

CREATE UNIQUE INDEX [IX_Volumes_SeriesId_Number] ON [Volumes] ([SeriesId], [Number]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261025160227_InitialCreate', N'10.0.13');

COMMIT;
GO

