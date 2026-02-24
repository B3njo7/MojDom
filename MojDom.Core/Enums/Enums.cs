using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojDom.Core.Enums
{
    public enum UserRole
    {
        Admin,
        PropertyOwner,
        PropertyManager,
        ServiceProvider
    }

    public enum PropertyType
    {
        Apartment,
        House,
        Villa,
        CommercialSpace,
        Land
    }

    public enum PropertyStatus
    {
        Active,
        Inactive,
        UnderMaintenance
    }

    public enum AgreementStatus
    {
        Pending,
        Active,
        Expired,
        Cancelled
    }

    public enum InspectionStatus
    {
        Scheduled,
        InProgress,
        Completed,
        Cancelled
    }

    public enum ServiceRequestStatus
    {
        Open,
        Assigned,
        InProgress,
        Completed,
        Cancelled
    }

    public enum RequestPriority
    {
        Low,
        Medium,
        High,
        Urgent
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum NotificationType
    {
        InspectionCreated,
        ServiceRequestCreated,
        ServiceRequestAssigned,
        TaskCompleted,
        PaymentProcessed,
        MonthlyReportReady,
        InvitationReceived
    }

    public enum InvitationStatus
    {
        Pending,
        Accepted,
        Declined,
        Expired
    }
}
