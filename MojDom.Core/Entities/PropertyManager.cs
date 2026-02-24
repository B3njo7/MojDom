using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MojDom.Core.Entities.Identity;

namespace MojDom.Core.Entities
{
    public class PropertyManager
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public string? Bio { get; set; }
        public double Rating { get; set; } = 0;
        public int CompletedInspections { get; set; } = 0;
        public string CoverageZone { get; set; } = null!;
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ManagementAgreement> Agreements { get; set; } = new List<ManagementAgreement>();
    }
}
