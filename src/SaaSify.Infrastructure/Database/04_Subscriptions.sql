-- ============================================================
-- File: 04_Subscriptions.sql
-- Description: Subscriptions table + all related stored procedures
-- ============================================================

USE SaaSifyDb;
GO

-- ============================================================
-- TABLE: Subscriptions
-- ============================================================
IF OBJECT_ID('Subscriptions', 'U') IS NULL
BEGIN
    CREATE TABLE Subscriptions (
        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
        TenantId UNIQUEIDENTIFIER NOT NULL,
        Plan NVARCHAR(50) NOT NULL,
        Status NVARCHAR(20) NOT NULL DEFAULT 'Active',
        StartDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        EndDate DATETIME2 NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        CONSTRAINT FK_Subscriptions_Tenants FOREIGN KEY (TenantId) 
            REFERENCES Tenants(Id) ON DELETE CASCADE
    );

    CREATE INDEX IX_Subscriptions_TenantId ON Subscriptions(TenantId);
    CREATE INDEX IX_Subscriptions_TenantId_Status ON Subscriptions(TenantId, Status);
    
    PRINT 'Table Subscriptions created successfully';
END
ELSE
    PRINT 'Table Subscriptions already exists';
GO

-- ============================================================
-- SP: sp_SaveSubscription
-- Marks old active subscription as Inactive, creates new active one
-- ============================================================
IF OBJECT_ID('sp_SaveSubscription', 'P') IS NOT NULL
    DROP PROCEDURE sp_SaveSubscription;
GO

CREATE PROCEDURE sp_SaveSubscription
    @Id UNIQUEIDENTIFIER,
    @TenantId UNIQUEIDENTIFIER,
    @Plan NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Mark existing Active subscription as Inactive
    UPDATE Subscriptions
    SET Status = 'Inactive',
        EndDate = GETUTCDATE(),
        UpdatedAt = GETUTCDATE()
    WHERE TenantId = @TenantId AND Status = 'Active';

    -- Create new active subscription
    INSERT INTO Subscriptions (Id, TenantId, Plan, Status, StartDate, CreatedAt)
    VALUES (@Id, @TenantId, @Plan, 'Active', GETUTCDATE(), GETUTCDATE());

    -- Also update Tenant table's SubscriptionPlan column
    UPDATE Tenants
    SET SubscriptionPlan = @Plan,
        UpdatedAt = GETUTCDATE()
    WHERE Id = @TenantId;

    -- Return the new subscription
    SELECT Id, TenantId, Plan, Status, StartDate, EndDate, CreatedAt, UpdatedAt
    FROM Subscriptions
    WHERE Id = @Id;
END
GO

PRINT 'Stored Procedure sp_SaveSubscription created successfully!';
GO

-- ============================================================
-- SP: sp_GetCurrentSubscription
-- Returns the active subscription for a tenant
-- ============================================================
IF OBJECT_ID('sp_GetCurrentSubscription', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetCurrentSubscription;
GO

CREATE PROCEDURE sp_GetCurrentSubscription
    @TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, TenantId, Plan, Status, StartDate, EndDate, CreatedAt, UpdatedAt
    FROM Subscriptions
    WHERE TenantId = @TenantId AND Status = 'Active';
END
GO

PRINT 'Stored Procedure sp_GetCurrentSubscription created successfully!';
GO

-- ============================================================
-- SP: sp_GetSubscriptionHistory
-- Returns all subscriptions for a tenant (active + past)
-- ============================================================
IF OBJECT_ID('sp_GetSubscriptionHistory', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetSubscriptionHistory;
GO

CREATE PROCEDURE sp_GetSubscriptionHistory
    @TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, TenantId, Plan, Status, StartDate, EndDate, CreatedAt, UpdatedAt
    FROM Subscriptions
    WHERE TenantId = @TenantId
    ORDER BY CreatedAt DESC;
END
GO

PRINT 'Stored Procedure sp_GetSubscriptionHistory created successfully!';
GO

-- ============================================================
-- SP: sp_CancelSubscription
-- Cancels the active subscription
-- ============================================================
IF OBJECT_ID('sp_CancelSubscription', 'P') IS NOT NULL
    DROP PROCEDURE sp_CancelSubscription;
GO

CREATE PROCEDURE sp_CancelSubscription
    @TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Subscriptions
    SET Status = 'Cancelled',
        EndDate = GETUTCDATE(),
        UpdatedAt = GETUTCDATE()
    WHERE TenantId = @TenantId AND Status = 'Active';

    -- Reset tenant to Free plan
    UPDATE Tenants
    SET SubscriptionPlan = 'Free',
        UpdatedAt = GETUTCDATE()
    WHERE Id = @TenantId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

PRINT 'Stored Procedure sp_CancelSubscription created successfully!';
GO

PRINT '========================================';
PRINT '04_Subscriptions.sql executed successfully!';
PRINT '========================================';