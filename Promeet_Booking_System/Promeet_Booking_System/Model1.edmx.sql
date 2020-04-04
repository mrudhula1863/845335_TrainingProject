
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/02/2020 01:10:40
-- Generated from EDMX file: F:\Training Project\Promeet_Booking_System\Promeet_Booking_System\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Promeet];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Bookings__RoomId__628FA481]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK__Bookings__RoomId__628FA481];
GO
IF OBJECT_ID(N'[dbo].[FK__Bookings__UserId__6383C8BA]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK__Bookings__UserId__6383C8BA];
GO
IF OBJECT_ID(N'[dbo].[FK__Rooms__UserId__5FB337D6]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Rooms] DROP CONSTRAINT [FK__Rooms__UserId__5FB337D6];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Bookings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bookings];
GO
IF OBJECT_ID(N'[dbo].[Rooms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rooms];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Bookings'
CREATE TABLE [dbo].[Bookings] (
    [BookingId] int IDENTITY(1,1) NOT NULL primary key,
    [RoomId] int  NOT NULL foreign key references Rooms(RoomId) ,
    [UserId] int  NOT NULL foreign key references Users(UserId),
    [Payment_Money] decimal(19,4)  NOT NULL,
    [Location] nvarchar(max)  NOT NULL,
    [Date_Time] datetime  NOT NULL,
    [Duration] int  NOT NULL
);
GO

-- Creating table 'Rooms'
CREATE TABLE [dbo].[Rooms] (
    [RoomId] int IDENTITY(1,1) NOT NULL primary key,
    [UserId] int  NOT NULL foreign key references Users(UserId),
    [AC] bit  NOT NULL,
    [Projector] bit  NOT NULL,
    [CoffeeFilter] bit  NOT NULL,
    [WaterBottles] bit  NOT NULL,
    [Mic] bit  NOT NULL,
    [Speaker] bit  NOT NULL,
    [System] bit  NOT NULL,
    [Podium] bit  NOT NULL,
    [WiFi] bit  NOT NULL,
    [WhiteBoard] bit  NOT NULL,
    [Price] decimal(19,4)  NOT NULL,
    [Location] nvarchar(max)  NOT NULL,
    [CapacityOfRoom] int  NOT NULL,
    [Availability] bit  NOT NULL,
    [RoomName] nvarchar(max)  NULL,
    [IsRoomBooked] bit  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserId] int IDENTITY(1,1) NOT NULL primary key,
    [Name] nvarchar(max)  NOT NULL,
    [EmailId] nvarchar(max)  NOT NULL,
    [PhoneNo] bigint  NOT NULL,
    [Office_Address] nvarchar(max)  NOT NULL,
    [Password] nvarchar(50)  NULL,
    [IsEmailVerified] bit  NOT NULL,
    [ActivationCode] uniqueidentifier  NOT NULL,
    [ResetPasswordCode] nvarchar(100)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [BookingId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [PK_Bookings]
    PRIMARY KEY CLUSTERED ([BookingId] ASC);
GO

-- Creating primary key on [RoomId] in table 'Rooms'
ALTER TABLE [dbo].[Rooms]
ADD CONSTRAINT [PK_Rooms]
    PRIMARY KEY CLUSTERED ([RoomId] ASC);
GO

-- Creating primary key on [UserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RoomId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK__Bookings__RoomId__628FA481]
    FOREIGN KEY ([RoomId])
    REFERENCES [dbo].[Rooms]
        ([RoomId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Bookings__RoomId__628FA481'
CREATE INDEX [IX_FK__Bookings__RoomId__628FA481]
ON [dbo].[Bookings]
    ([RoomId]);
GO

-- Creating foreign key on [UserId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK__Bookings__UserId__6383C8BA]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Bookings__UserId__6383C8BA'
CREATE INDEX [IX_FK__Bookings__UserId__6383C8BA]
ON [dbo].[Bookings]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'Rooms'
ALTER TABLE [dbo].[Rooms]
ADD CONSTRAINT [FK__Rooms__UserId__5FB337D6]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Rooms__UserId__5FB337D6'
CREATE INDEX [IX_FK__Rooms__UserId__5FB337D6]
ON [dbo].[Rooms]
    ([UserId]);
GO

alter table Bookings
drop  column Location
go

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------