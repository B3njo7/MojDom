using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MojDom.Core.Entities.Identity;

namespace MojDom.Core.Entities
{
    public class RequestAssignment
    {
        public int Id { get; set; }
        public int ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; } = null!;
        public int ServiceProviderId { get; set; }
        public ServiceProvider ServiceProvider { get; set; } = null!;
        public string AssignedById { get; set; } = null!;
        public ApplicationUser AssignedBy { get; set; } = null!;
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
    }
}