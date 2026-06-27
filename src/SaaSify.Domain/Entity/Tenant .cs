using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSify.Domain.Entity
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SubscriptionPlan { get; set; } = "Free";
        public bool IsActive { get; set; } = true;

        // Navigation property — one tenant has many users
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
