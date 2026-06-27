using Dapper;
using SaaSify.Application.Interface.Repositories;
using SaaSify.Domain.Entity;
using System.Data;

namespace SaaSify.Infrastructure.Persistences.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly DapperContext _context;

    public SubscriptionRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<Subscription> SaveAsync(Subscription subscription)
    {
        using var connection = _context.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@Id", subscription.Id);
        parameters.Add("@TenantId", subscription.TenantId);
        parameters.Add("@PlanId", subscription.PlanId);

        return await connection.QuerySingleAsync<Subscription>(
            "sp_SaveSubscription",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<Subscription?> GetCurrentAsync(Guid tenantId)
    {
        using var connection = _context.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);

        return await connection.QuerySingleOrDefaultAsync<Subscription>(
            "sp_GetCurrentSubscription",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<IEnumerable<Subscription>> GetHistoryAsync(Guid tenantId)
    {
        using var connection = _context.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);

        return await connection.QueryAsync<Subscription>(
            "sp_GetSubscriptionHistory",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<bool> CancelAsync(Guid tenantId)
    {
        using var connection = _context.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);

        var rowsAffected = await connection.QuerySingleAsync<int>(
            "sp_CancelSubscription",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return rowsAffected > 0;
    }
}