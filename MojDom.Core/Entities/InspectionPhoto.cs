using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojDom.Core.Entities
{
    public class InspectionPhoto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Description { get; set; }
        public string? Room { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public int PropertyInspectionId { get; set; }
        public PropertyInspection PropertyInspection { get; set; } = null!;
    }
}