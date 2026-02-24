using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojDom.Core.Entities
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCover { get; set; } = false;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }
}