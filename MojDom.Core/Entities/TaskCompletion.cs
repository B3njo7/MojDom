using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojDom.Core.Entities
{
    public class TaskCompletion
    {
        public int Id { get; set; }
        public int ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; } = null!;
        public string WorkDescription { get; set; } = null!;
        public decimal LaborCost { get; set; }
        public string? ProofImageUrl { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MaterialUsed> MaterialsUsed { get; set; } = new List<MaterialUsed>();
    }
}