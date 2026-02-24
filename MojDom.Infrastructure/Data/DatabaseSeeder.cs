using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MojDom.Core.Entities;
using MojDom.Core.Entities.Identity;
using MojDom.Core.Enums;

namespace MojDom.Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedCitiesAsync(context);
            await SeedUsersAsync(userManager, context);
            await SeedPropertyManagersAsync(context);
            await SeedServiceProvidersAsync(context);
            await SeedServiceCategoriesAsync(context);
            await SeedPropertiesAsync(context);
            await SeedAgreementsAndInspectionsAsync(context);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "PropertyOwner", "PropertyManager", "ServiceProvider" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private static async Task SeedCitiesAsync(ApplicationDbContext context)
        {
            if (await context.Cities.AnyAsync()) return;

            var cities = new List<City>
            {
                new() { Name = "Sarajevo", Country = "Bosnia and Herzegovina" },
                new() { Name = "Mostar", Country = "Bosnia and Herzegovina" },
                new() { Name = "Banja Luka", Country = "Bosnia and Herzegovina" },
                new() { Name = "Tuzla", Country = "Bosnia and Herzegovina" },
                new() { Name = "Zenica", Country = "Bosnia and Herzegovina" },
                new() { Name = "Bihać", Country = "Bosnia and Herzegovina" },
                new() { Name = "Trebinje", Country = "Bosnia and Herzegovina" },
                new() { Name = "Neum", Country = "Bosnia and Herzegovina" },
            };

            await context.Cities.AddRangeAsync(cities);
            await context.SaveChangesAsync();
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            if (await userManager.Users.AnyAsync()) return;

            var sarajevoId = (await context.Cities.FirstAsync(c => c.Name == "Sarajevo")).Id;
            var mostarId = (await context.Cities.FirstAsync(c => c.Name == "Mostar")).Id;
            var banjalukaId = (await context.Cities.FirstAsync(c => c.Name == "Banja Luka")).Id;
            var zenicaId = (await context.Cities.FirstAsync(c => c.Name == "Zenica")).Id;

            // Admin
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@mojdom.ba",
                FirstName = "Admin",
                LastName = "MojDom",
                CityId = sarajevoId,
                IsActive = true,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "test");
            await userManager.AddToRoleAsync(admin, "Admin");

            // Property Owners
            var owner1 = new ApplicationUser
            {
                UserName = "propertyowner",
                Email = "owner1@mojdom.ba",
                FirstName = "Hans",
                LastName = "Müller",
                CityId = sarajevoId,
                IsActive = true,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(owner1, "test");
            await userManager.AddToRoleAsync(owner1, "PropertyOwner");

            var owner2 = new ApplicationUser
            {
                UserName = "owner2",
                Email = "owner2@mojdom.ba",
                FirstName = "Maria",
                LastName = "Schmidt",
                CityId = mostarId,
                IsActive = true,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(owner2, "test");
            await userManager.AddToRoleAsync(owner2, "PropertyOwner");

            // Property Managers
            var manager1 = new ApplicationUser
            {
                UserName = "propertymanager",
                Email = "manager1@mojdom.ba",
                FirstName = "Amir",
                LastName = "Hodžić",
                CityId = sarajevoId,
                IsActive = true,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(manager1, "test");
            await userManager.AddToRoleAsync(manager1, "PropertyManager");

            var manager2 = new ApplicationUser
            {
                UserName = "manager2",
                Email = "manager2@mojdom.ba",
                FirstName = "Selma",
                LastName = "Kovač",
                CityId = mostarId,
                IsActive = true,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(manager2, "test");
            await userManager.AddToRoleAsync(manager2, "PropertyManager");

            // Service Providers
            var sp1 = new ApplicationUser
            {
                UserName = "serviceprovider",
                Email = "sp1@mojdom.ba",
                FirstName = "Emir",
                LastName = "Begić",
                CityId = sarajevoId,
                IsActive = true,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(sp1, "test");
            await userManager.AddToRoleAsync(sp1, "ServiceProvider");

            var sp2 = new ApplicationUser
            {
                UserName = "sp2",
                Email = "sp2@mojdom.ba",
                FirstName = "Nermin",
                LastName = "Šišić",
                CityId = banjalukaId,
                IsActive = true,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(sp2, "test");
            await userManager.AddToRoleAsync(sp2, "ServiceProvider");

            var sp3 = new ApplicationUser
            {
                UserName = "sp3",
                Email = "sp3@mojdom.ba",
                FirstName = "Damir",
                LastName = "Lučić",
                CityId = zenicaId,
                IsActive = true,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(sp3, "test");
            await userManager.AddToRoleAsync(sp3, "ServiceProvider");
        }

        private static async Task SeedPropertyManagersAsync(ApplicationDbContext context)
        {
            if (await context.PropertyManagers.AnyAsync()) return;

            var manager1 = await context.Users.FirstAsync(u => u.UserName == "propertymanager");
            var manager2 = await context.Users.FirstAsync(u => u.UserName == "manager2");

            var managers = new List<PropertyManager>
            {
                new()
                {
                    UserId = manager1.Id,
                    Bio = "Iskusan property manager sa 5 godina iskustva u Sarajevu.",
                    Rating = 4.8,
                    CompletedInspections = 42,
                    CoverageZone = "Sarajevo",
                    IsAvailable = true
                },
                new()
                {
                    UserId = manager2.Id,
                    Bio = "Specijalizovan za nekretnine u Mostaru i okolini.",
                    Rating = 4.5,
                    CompletedInspections = 28,
                    CoverageZone = "Mostar",
                    IsAvailable = true
                }
            };

            await context.PropertyManagers.AddRangeAsync(managers);
            await context.SaveChangesAsync();
        }

        private static async Task SeedServiceProvidersAsync(ApplicationDbContext context)
        {
            if (await context.ServiceProviders.AnyAsync()) return;

            var sp1 = await context.Users.FirstAsync(u => u.UserName == "serviceprovider");
            var sp2 = await context.Users.FirstAsync(u => u.UserName == "sp2");
            var sp3 = await context.Users.FirstAsync(u => u.UserName == "sp3");

            var providers = new List<Core.Entities.ServiceProvider>
            {
                new()
                {
                    UserId = sp1.Id,
                    CompanyName = "Begić Elektro d.o.o.",
                    Bio = "Licencirani električar sa 10 godina iskustva.",
                    Rating = 4.9,
                    CompletedJobs = 87,
                    CoverageZone = "Sarajevo",
                    IsAvailable = true
                },
                new()
                {
                    UserId = sp2.Id,
                    CompanyName = null,
                    Bio = "Vodoinstalater, brz i pouzdan.",
                    Rating = 4.6,
                    CompletedJobs = 54,
                    CoverageZone = "Banja Luka",
                    IsAvailable = true
                },
                new()
                {
                    UserId = sp3.Id,
                    CompanyName = "Lučić Gradnja",
                    Bio = "Generalni majstor — građevina, keramika, gips.",
                    Rating = 4.7,
                    CompletedJobs = 63,
                    CoverageZone = "Zenica",
                    IsAvailable = true
                }
            };

            await context.ServiceProviders.AddRangeAsync(providers);
            await context.SaveChangesAsync();
        }

        private static async Task SeedServiceCategoriesAsync(ApplicationDbContext context)
        {
            if (await context.ServiceCategories.AnyAsync()) return;

            var categories = new List<ServiceCategory>
            {
                new() { Name = "Elektrika" },
                new() { Name = "Vodoinstalacije" },
                new() { Name = "Građevina" },
                new() { Name = "Keramika" },
                new() { Name = "Stolarija" },
                new() { Name = "Grijanje i klimatizacija" },
                new() { Name = "Ličenje i fasada" },
                new() { Name = "Krovovi i izolacija" },
                new() { Name = "Brave i sigurnost" },
                new() { Name = "Opće održavanje" },
            };

            await context.ServiceCategories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedPropertiesAsync(ApplicationDbContext context)
        {
            if (await context.Properties.AnyAsync()) return;

            var owner1 = await context.Users.FirstAsync(u => u.UserName == "propertyowner");
            var owner2 = await context.Users.FirstAsync(u => u.UserName == "owner2");
            var sarajevo = await context.Cities.FirstAsync(c => c.Name == "Sarajevo");
            var mostar = await context.Cities.FirstAsync(c => c.Name == "Mostar");
            var neum = await context.Cities.FirstAsync(c => c.Name == "Neum");

            var properties = new List<Property>
            {
                new()
                {
                    Name = "Apartman Baščaršija",
                    Address = "Ulica Ferhadija 12, Sarajevo",
                    Latitude = 43.8563,
                    Longitude = 18.4131,
                    Type = PropertyType.Apartment,
                    Status = PropertyStatus.Active,
                    Description = "Lijep apartman u centru Sarajeva, blizu Baščaršije.",
                    SizeM2 = 65,
                    OwnerId = owner1.Id,
                    CityId = sarajevo.Id
                },
                new()
                {
                    Name = "Kuća Mostar",
                    Address = "Bulevar narodne revolucije 45, Mostar",
                    Latitude = 43.3438,
                    Longitude = 17.8078,
                    Type = PropertyType.House,
                    Status = PropertyStatus.Active,
                    Description = "Porodična kuća u Mostaru sa dvorištem.",
                    SizeM2 = 140,
                    OwnerId = owner1.Id,
                    CityId = mostar.Id
                },
                new()
                {
                    Name = "Vila Neum",
                    Address = "Obala bb, Neum",
                    Latitude = 42.9228,
                    Longitude = 17.6156,
                    Type = PropertyType.Villa,
                    Status = PropertyStatus.Active,
                    Description = "Ljetna vila sa pogledom na more u Neumu.",
                    SizeM2 = 200,
                    OwnerId = owner2.Id,
                    CityId = neum.Id
                },
            };

            await context.Properties.AddRangeAsync(properties);
            await context.SaveChangesAsync();
        }

        private static async Task SeedAgreementsAndInspectionsAsync(ApplicationDbContext context)
        {
            if (await context.ManagementAgreements.AnyAsync()) return;

            var property1 = await context.Properties.FirstAsync(p => p.Name == "Apartman Baščaršija");
            var property2 = await context.Properties.FirstAsync(p => p.Name == "Kuća Mostar");
            var manager1 = await context.PropertyManagers.Include(m => m.User).FirstAsync(m => m.User.UserName == "propertymanager");
            var manager2 = await context.PropertyManagers.Include(m => m.User).FirstAsync(m => m.User.UserName == "manager2");

            var agreement1 = new ManagementAgreement
            {
                PropertyId = property1.Id,
                PropertyManagerId = manager1.Id,
                MonthlyFee = 150,
                StartDate = new DateTime(2025, 1, 1),
                Status = AgreementStatus.Active,
                Terms = "Mjesečna inspekcija, hitne intervencije u roku 24h."
            };

            var agreement2 = new ManagementAgreement
            {
                PropertyId = property2.Id,
                PropertyManagerId = manager2.Id,
                MonthlyFee = 120,
                StartDate = new DateTime(2025, 3, 1),
                Status = AgreementStatus.Active,
                Terms = "Dvomjesečna inspekcija, redovno izvještavanje."
            };

            await context.ManagementAgreements.AddRangeAsync(agreement1, agreement2);
            await context.SaveChangesAsync();

            // Inspekcije
            var inspections = new List<PropertyInspection>
            {
                new()
                {
                    ManagementAgreementId = agreement1.Id,
                    ScheduledDate = new DateTime(2025, 1, 15),
                    CompletedDate = new DateTime(2025, 1, 15),
                    Status = InspectionStatus.Completed,
                    Notes = "Sve u redu, manje oštećenje na zidu u kupaonici."
                },
                new()
                {
                    ManagementAgreementId = agreement1.Id,
                    ScheduledDate = new DateTime(2025, 2, 15),
                    CompletedDate = new DateTime(2025, 2, 15),
                    Status = InspectionStatus.Completed,
                    Notes = "Popravljen zid, sve uredno."
                },
                new()
                {
                    ManagementAgreementId = agreement1.Id,
                    ScheduledDate = DateTime.UtcNow.AddDays(7),
                    Status = InspectionStatus.Scheduled,
                    Notes = null
                },
                new()
                {
                    ManagementAgreementId = agreement2.Id,
                    ScheduledDate = new DateTime(2025, 3, 20),
                    CompletedDate = new DateTime(2025, 3, 20),
                    Status = InspectionStatus.Completed,
                    Notes = "Potrebna popravka slavine u kuhinji."
                },
            };

            await context.PropertyInspections.AddRangeAsync(inspections);
            await context.SaveChangesAsync();

            // Service request
            var category = await context.ServiceCategories.FirstAsync(c => c.Name == "Vodoinstalacije");
            var serviceRequest = new ServiceRequest
            {
                Title = "Popravka slavine u kuhinji",
                Description = "Slavina u kuhinji curi, potrebna hitna popravka.",
                Priority = RequestPriority.High,
                Status = ServiceRequestStatus.Open,
                ServiceCategoryId = category.Id,
                PropertyInspectionId = inspections[3].Id,
                PropertyId = property2.Id
            };

            await context.ServiceRequests.AddAsync(serviceRequest);
            await context.SaveChangesAsync();
        }
    }
}