using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MojDom.Core.Entities.Identity;

namespace MojDom.Core.Entities
{
    public class ServiceProvider
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public string? CompanyName { get; set; }
        public string? Bio { get; set; }
        public double Rating { get; set; } = 0;
        public int CompletedJobs { get; set; } = 0;
        public string CoverageZone { get; set; } = null!;
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ProviderCertificate> Certificates { get; set; } = new List<ProviderCertificate>();
        public ICollection<RequestAssignment> Assignments { get; set; } = new List<RequestAssignment>();
        public ICollection<ServiceReview> Reviews { get; set; } = new List<ServiceReview>();
    }
}