-- ===============================================================================
-- GroomMate ASP.NET MVC 5 Production Database Deployment & Seed Script
-- Target Environment: FreeASPHosting.net MS SQL Server
-- ===============================================================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 1. Create Roles Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Roles](
    [RoleID] [int] IDENTITY(1,1) NOT NULL,
    [RoleName] [nvarchar](max) NULL,
    CONSTRAINT [PK_dbo.Roles] PRIMARY KEY CLUSTERED ([RoleID] ASC)
)
END
GO

-- 2. Create Users Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Users](
    [UserID] [int] IDENTITY(1,1) NOT NULL,
    [Username] [nvarchar](max) NULL,
    [Password] [nvarchar](max) NULL,
    [FullName] [nvarchar](max) NULL,
    [Email] [nvarchar](max) NULL,
    [IsDeleted] [bit] NOT NULL,
    [RoleID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
)
END
GO

-- 3. Create Services Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Services]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Services](
    [ServiceID] [int] IDENTITY(1,1) NOT NULL,
    [ServiceName] [nvarchar](max) NULL,
    [Description] [nvarchar](max) NULL,
    [Price] [decimal](18, 2) NOT NULL,
    [IsActive] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Services] PRIMARY KEY CLUSTERED ([ServiceID] ASC)
)
END
GO

-- 4. Create Appointments Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Appointments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Appointments](
    [AppointmentID] [int] IDENTITY(1,1) NOT NULL,
    [UserID] [int] NOT NULL,
    [ServiceID] [int] NOT NULL,
    [StaffId] [int] NULL,
    [AppointmentDate] [datetime] NOT NULL,
    [Status] [nvarchar](max) NULL,
    CONSTRAINT [PK_dbo.Appointments] PRIMARY KEY CLUSTERED ([AppointmentID] ASC)
)
END
GO

-- 5. Create Feedbacks Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Feedbacks]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Feedbacks](
    [AppointmentID] [int] NOT NULL,
    [Rating] [int] NOT NULL,
    [Comments] [nvarchar](max) NULL,
    CONSTRAINT [PK_dbo.Feedbacks] PRIMARY KEY CLUSTERED ([AppointmentID] ASC)
)
END
GO

-- Foreign Key Constraints
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Users_dbo.Roles_RoleID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CONSTRAINT [FK_dbo.Users_dbo.Roles_RoleID] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
ON DELETE CASCADE
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Appointments_dbo.Services_ServiceID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD CONSTRAINT [FK_dbo.Appointments_dbo.Services_ServiceID] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
ON DELETE CASCADE
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Appointments_dbo.Users_StaffId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD CONSTRAINT [FK_dbo.Appointments_dbo.Users_StaffId] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Users] ([UserID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Appointments_dbo.Users_UserID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD CONSTRAINT [FK_dbo.Appointments_dbo.Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Feedbacks_dbo.Appointments_AppointmentID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Feedbacks]'))
ALTER TABLE [dbo].[Feedbacks]  WITH CHECK ADD CONSTRAINT [FK_dbo.Feedbacks_dbo.Appointments_AppointmentID] FOREIGN KEY([AppointmentID])
REFERENCES [dbo].[Appointments] ([AppointmentID])
GO

-- SEED DATA: Roles
IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleName] = 'Admin')
    INSERT INTO [dbo].[Roles] ([RoleName]) VALUES ('Admin');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleName] = 'Staff')
    INSERT INTO [dbo].[Roles] ([RoleName]) VALUES ('Staff');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleName] = 'Customer')
    INSERT INTO [dbo].[Roles] ([RoleName]) VALUES ('Customer');

-- SEED DATA: Users (Default Admin, Staff, and Customer Accounts)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'admin')
    INSERT INTO [dbo].[Users] ([Username], [Password], [FullName], [Email], [IsDeleted], [RoleID]) VALUES ('admin', 'password', 'Admin User', 'admin@groommate.com', 0, 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'alex')
    INSERT INTO [dbo].[Users] ([Username], [Password], [FullName], [Email], [IsDeleted], [RoleID]) VALUES ('alex', 'password', 'Alex Barber', 'alex@groommate.com', 0, 2);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'john')
    INSERT INTO [dbo].[Users] ([Username], [Password], [FullName], [Email], [IsDeleted], [RoleID]) VALUES ('john', 'password', 'John Stylist', 'john@groommate.com', 0, 2);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'staff')
    INSERT INTO [dbo].[Users] ([Username], [Password], [FullName], [Email], [IsDeleted], [RoleID]) VALUES ('staff', 'password', 'Staff Member', 'staff@groommate.com', 0, 2);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'balraj')
    INSERT INTO [dbo].[Users] ([Username], [Password], [FullName], [Email], [IsDeleted], [RoleID]) VALUES ('balraj', 'password', 'Balraj Gopi', 'balraj@example.com', 0, 3);

-- SEED DATA: 9 Active Grooming Services
IF NOT EXISTS (SELECT 1 FROM [dbo].[Services] WHERE [ServiceID] = 1)
BEGIN
    SET IDENTITY_INSERT [dbo].[Services] ON;
    INSERT INTO [dbo].[Services] ([ServiceID], [ServiceName], [Description], [Price], [IsActive]) VALUES 
    (1, N'Classic Haircut', N'Traditional scissor and clipper cut including wash and basic styling.', 150.00, 1),
    (2, N'Beard Trim & Styling', N'Beard shaping, razor line definition, and nourishing beard oil treatment.', 80.00, 1),
    (3, N'Royal Shave & Facial', N'Hot towel treatment, straight-razor shave, and refreshing facial massage.', 100.00, 1),
    (4, N'Hot Towel Shave', N'Traditional straight razor shave with essential oil hot towel treatment.', 120.00, 1),
    (5, N'Hair Spa Treatment', N'Deep conditioning scalp massage and revitalizing hair treatment.', 250.00, 1),
    (6, N'Face Massage & Cleanup', N'Exfoliating scrub, face massage, and blackhead removal for glowing skin.', 180.00, 1),
    (7, N'Hair Color & Style', N'Professional grey coverage or custom hair coloring with finish styling.', 350.00, 1),
    (8, N'Kids Haircut', N'Gentle haircut experience tailored for children under 12 years.', 100.00, 1),
    (9, N'Combo Package (Haircut + Beard)', N'Complete grooming package: Classic Haircut + Beard Trim & Oil treatment.', 200.00, 1);
    SET IDENTITY_INSERT [dbo].[Services] OFF;
END
GO

PRINT 'GroomMate production database deployment and seed data creation completed successfully!';
