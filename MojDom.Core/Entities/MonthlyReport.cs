using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MojDom.Core.Entities
{
    public class MonthlyReport
    {
        public int Id { get; set; }
        public int ManagementAgreementId { get; set; }
        public ManagementAgreement ManagementAgreement { get; set; } = null!;
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalCosts { get; set; }
        public string? Notes { get; set; }
        public string? PdfUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}