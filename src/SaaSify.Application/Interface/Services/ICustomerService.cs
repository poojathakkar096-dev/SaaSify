using SaaSify.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSify.Application.Interface.Services
{
    public interface ICustomerService
    {
        Task<CustomerResponse> SaveAsync(CustomerRequest request);
        Task<IEnumerable<CustomerResponse>> GetAllAsync();
        Task<CustomerResponse?> GetByIdAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
    }
}
