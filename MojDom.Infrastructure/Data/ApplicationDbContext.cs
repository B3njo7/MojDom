using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MojDom.Core.Entities;
using MojDom.Core.Entities.Identity;

namespace MojDom.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<City> Cities { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<PropertyManager> PropertyManagers { get; set; }
        public DbSet<ManagerInvitation> ManagerInvitations { get; set; }
        public DbSet<ManagementAgreement> ManagementAgreements { get; set; }
        public DbSet<PropertyInspection> PropertyInspections { get; set; }
        public DbSet<InspectionPhoto> InspectionPhotos { get; set; }
        public DbSet<ScheduledMaintenanceTask> ScheduledMaintenanceTasks { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<RequestPhoto> RequestPhotos { get; set; }
        public DbSet<RequestAssignment> RequestAssignments { get; set; }
        public DbSet<Core.Entities.ServiceProvider> ServiceProviders { get; set; }
        public DbSet<ProviderCertificate> ProviderCertificates { get; set; }
        public DbSet<MaterialUsed> MaterialsUsed { get; set; }
        public DbSet<TaskCompletion> TaskCompletions { get; set; }
        public DbSet<MonthlyReport> MonthlyReports { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ServiceReview> ServiceReviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Decimal precision
            builder.Entity<ManagementAgreement>()
                .Property(x => x.MonthlyFee).HasPrecision(18, 2);
            builder.Entity<Payment>()
                .Property(x => x.Amount).HasPrecision(18, 2);
            builder.Entity<MaterialUsed>()
                .Property(x => x.UnitPrice).HasPrecision(18, 2);
            builder.Entity<TaskCompletion>()
                .Property(x => x.LaborCost).HasPrecision(18, 2);
            builder.Entity<MonthlyReport>()
                .Property(x => x.TotalCosts).HasPrecision(18, 2);

            // Computed column
            builder.Entity<MaterialUsed>()
                .Property(x => x.TotalPrice)
                .HasComputedColumnSql("[Quantity] * [UnitPrice]");

            // Globalno Restrict NA KRAJU - mora biti zadnje!
            foreach (var relationship in builder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

    }
}