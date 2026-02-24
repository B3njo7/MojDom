using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MojDom.Core.Enums;

namespace MojDom.Core.Entities
{
    public class ManagementAgreement
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
        public int PropertyManagerId { get; set; }
        public PropertyManager PropertyManager { get; set; } = null!;
        public decimal MonthlyFee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public AgreementStatus Status { get; set; } = AgreementStatus.Pending;
        public string? Terms { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PropertyInspection> Inspections { get; set; } = new List<PropertyInspection>();
        public ICollection<MonthlyReport> MonthlyReports { get; set; } = new List<MonthlyReport>();
    }
}