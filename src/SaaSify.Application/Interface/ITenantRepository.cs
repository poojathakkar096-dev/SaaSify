using SaaSify.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSify.Application.Interface
{
    public interface ITenantRepository
    {
        Task<Tenant> CreateAsync(Tenant tenant);

    }
}
