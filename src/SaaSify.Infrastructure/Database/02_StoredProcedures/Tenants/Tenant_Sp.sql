-- ============================================================
-- STORED PROCEDURE: sp_CreateTenant
-- Creates a new tenant
-- ============================================================
CREATE PROCEDURE sp_CreateTenant
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(200),
    @Email NVARCHAR(200),
    @SubscriptionPlan NVARCHAR(50) = 'Free'
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Tenants (Id, Name, Email, SubscriptionPlan, IsActive, CreatedAt)
    VALUES (@Id, @Name, @Email, @SubscriptionPlan, 1, GETUTCDATE());

    SELECT Id, Name, Email, SubscriptionPlan, IsActive, CreatedAt, UpdatedAt
    FROM Tenants
    WHERE Id = @Id;
END
GO