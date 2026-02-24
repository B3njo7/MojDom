using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MojDom.Core.Entities.Identity;

namespace MojDom.Core.Entities
{
    public class ServiceReview
    {
        public int Id { get; set; }
        public int ServiceProviderId { get; set; }
        public ServiceProvider ServiceProvider { get; set; } = null!;
        public string ReviewerId { get; set; } = null!;
        public ApplicationUser Reviewer { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}