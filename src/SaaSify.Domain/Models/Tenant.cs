using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSify.Domain.Models;

public class Tenant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SubscriptionPlan { get; set; } = "Free";
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
