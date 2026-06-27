-- ============================================================
-- Table: Tenants
-- Description: Master table for all SaaS tenants (businesses)
-- Author: Pooja Thakkar
-- Created: 2026-06-22
-- ============================================================
USE SaaSifyDb;
GO

CREATE TABLE Tenants (
        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Email NVARCHAR(200) NOT NULL,
        SubscriptionPlan NVARCHAR(50) NOT NULL DEFAULT 'Free',
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL
    );

    CREATE TABLE Users (
        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
        TenantId UNIQUEIDENTIFIER NOT NULL,
        Email NVARCHAR(200) NOT NULL,
        PasswordHash NVARCHAR(MAX) NOT NULL,
        FullName NVARCHAR(200) NOT NULL,
        Role NVARCHAR(20) NOT NULL DEFAULT 'User',
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        CONSTRAINT FK_Users_Tenants FOREIGN KEY (TenantId) 
            REFERENCES Tenants(Id) ON DELETE CASCADE
    );

     CREATE TABLE Customers (
        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
        TenantId UNIQUEIDENTIFIER NOT NULL,
        Name NVARCHAR(200) NOT NULL,
        Phone NVARCHAR(20) NULL,
        Email NVARCHAR(200) NULL,
        Address NVARCHAR(500) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        CONSTRAINT FK_Customers_Tenants FOREIGN KEY (TenantId) 
            REFERENCES Tenants(Id) ON DELETE CASCADE
    );

    CREATE INDEX IX_Customers_TenantId ON Customers(TenantId);
    