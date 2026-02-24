using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojDom.Core.Entities
{
    public class ProviderCertificate
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public DateTime? ExpiryDate { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public int ServiceProviderId { get; set; }
        public ServiceProvider ServiceProvider { get; set; } = null!;
    }
}