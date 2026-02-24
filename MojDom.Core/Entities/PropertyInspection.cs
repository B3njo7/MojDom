using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MojDom.Core.Enums;

namespace MojDom.Core.Entities
{
    public class PropertyInspection
    {
        public int Id { get; set; }
        public int ManagementAgreementId { get; set; }
        public ManagementAgreement ManagementAgreement { get; set; } = null!;
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public InspectionStatus Status { get; set; } = InspectionStatus.Scheduled;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<InspectionPhoto> Photos { get; set; } = new List<InspectionPhoto>();
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}