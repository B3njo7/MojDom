using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MojDom.Core.Enums;

namespace MojDom.Core.Entities
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public RequestPriority Priority { get; set; } = RequestPriority.Medium;
        public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.Open;
        public int ServiceCategoryId { get; set; }
        public ServiceCategory ServiceCategory { get; set; } = null!;
        public int? PropertyInspectionId { get; set; }
        public PropertyInspection? PropertyInspection { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }

        public ICollection<RequestPhoto> Photos { get; set; } = new List<RequestPhoto>();
        public ICollection<RequestAssignment> Assignments { get; set; } = new List<RequestAssignment>();
    }
}