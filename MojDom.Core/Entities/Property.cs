using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MojDom.Core.Entities.Identity;
using MojDom.Core.Enums;

namespace MojDom.Core.Entities
{
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public PropertyType Type { get; set; }
        public PropertyStatus Status { get; set; } = PropertyStatus.Active;
        public string? Description { get; set; }
        public int SizeM2 { get; set; }
        public bool HasSecuritySystem { get; set; } = false; // V2
        public string OwnerId { get; set; } = null!;
        public ApplicationUser Owner { get; set; } = null!;
        public int CityId { get; set; }
        public City City { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
        public ICollection<ManagementAgreement> Agreements { get; set; } = new List<ManagementAgreement>();
    }
}