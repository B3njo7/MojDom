using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MojDom.Core.Entities.Identity;
using MojDom.Core.Enums;

namespace MojDom.Core.Entities
{
    public class ManagerInvitation
    {
        public int Id { get; set; }
        public string OwnerId { get; set; } = null!;
        public ApplicationUser Owner { get; set; } = null!;
        public int PropertyManagerId { get; set; }
        public PropertyManager PropertyManager { get; set; } = null!;
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
        public string? Message { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }
    }
}