-- ============================================================
-- File: 03_Customers.sql
-- Description: all Customer-related stored procedures
-- Author: Pooja Thakkar
-- ============================================================

USE SaaSifyDb;
GO


-- ============================================================
-- STORED PROCEDURE: sp_SaveCustomer
-- Upsert (Create or Update) customer with tenant isolation
-- ============================================================

CREATE PROCEDURE sp_SaveCustomer
    @Id UNIQUEIDENTIFIER,
    @TenantId UNIQUEIDENTIFIER,
    @Name NVARCHAR(200),
    @Phone NVARCHAR(20) = NULL,
    @Email NVARCHAR(200) = NULL,
    @Address NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Customers WHERE Id = @Id AND TenantId = @TenantId)
    BEGIN
        UPDATE Customers
        SET 
            Name = @Name,
            Phone = @Phone,
            Email = @Email,
            Address = @Address,
            UpdatedAt = GETUTCDATE()
        WHERE Id = @Id AND TenantId = @TenantId;
    END
    ELSE
    BEGIN
        INSERT INTO Customers (Id, TenantId, Name, Phone, Email, Address, CreatedAt)
        VALUES (@Id, @TenantId, @Name, @Phone, @Email, @Address, GETUTCDATE());
    END

    SELECT Id, TenantId, Name, Phone, Email, Address, CreatedAt, UpdatedAt
    FROM Customers
    WHERE Id = @Id AND TenantId = @TenantId;
END
GO


-- ============================================================
-- STORED PROCEDURE: sp_GetCustomersByTenant
-- Returns all customers for a given tenant
-- ============================================================

CREATE PROCEDURE sp_GetCustomersByTenant
    @TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, TenantId, Name, Phone, Email, Address, CreatedAt, UpdatedAt
    FROM Customers
    WHERE TenantId = @TenantId
    ORDER BY CreatedAt DESC;
END
GO

-- ============================================================
-- STORED PROCEDURE: sp_SaveCustomer
-- Get customer ByID
-- ============================================================

CREATE PROCEDURE sp_GetCustomerById
    @Id UNIQUEIDENTIFIER,
    @TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, TenantId, Name, Phone, Email, Address, CreatedAt, UpdatedAt
    FROM Customers
    WHERE Id = @Id AND TenantId = @TenantId;
END
GO

-- ============================================================
-- STORED PROCEDURE: sp_DeleteCustomer
-- Delete customer (only if belongs to current tenant)
-- ============================================================
CREATE PROCEDURE sp_DeleteCustomer
    @Id UNIQUEIDENTIFIER,
    @TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Customers
    WHERE Id = @Id AND TenantId = @TenantId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO