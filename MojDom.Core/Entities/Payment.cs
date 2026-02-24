using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MojDom.Core.Entities.Identity;
using MojDom.Core.Enums;

namespace MojDom.Core.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string PayerId { get; set; } = null!;
        public ApplicationUser Payer { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "BAM";
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string? StripePaymentIntentId { get; set; }
        public string? Description { get; set; }
        public int? MonthlyReportId { get; set; }
        public MonthlyReport? MonthlyReport { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
    }
}