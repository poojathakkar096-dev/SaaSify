-- ============================================================
-- File: 02_Users.sql
-- Description: Users table + all User-related stored procedures
-- Author: Pooja Thakkar
-- ============================================================

USE SaaSifyDb;
GO


-- ============================================================
-- STORED PROCEDURE: sp_CreateUser
-- Creates a new user under a tenant
-- ============================================================
CREATE PROCEDURE sp_CreateUser
    @Id UNIQUEIDENTIFIER,
    @TenantId UNIQUEIDENTIFIER,
    @Email NVARCHAR(200),
    @PasswordHash NVARCHAR(MAX),
    @FullName NVARCHAR(200),
    @Role NVARCHAR(20) = 'User'
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Users (Id, TenantId, Email, PasswordHash, FullName, Role, CreatedAt)
    VALUES (@Id, @TenantId, @Email, @PasswordHash, @FullName, @Role, GETUTCDATE());

    SELECT Id, TenantId, Email, FullName, Role, CreatedAt, UpdatedAt
    FROM Users
    WHERE Id = @Id;
END
GO

-- ============================================================
-- STORED PROCEDURE: sp_GetUserByEmail
-- Retrieves user by email for login authentication
-- ============================================================

CREATE PROCEDURE sp_GetUserByEmail
    @Email NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.Id,
        u.TenantId,
        u.Email,
        u.PasswordHash,
        u.FullName,
        u.Role,
        u.CreatedAt,
        u.UpdatedAt
    FROM Users u
    INNER JOIN Tenants t ON u.TenantId = t.Id
    WHERE u.Email = @Email AND t.IsActive = 1;
END
GO
