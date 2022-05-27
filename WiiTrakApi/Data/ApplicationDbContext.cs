using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WiiTrakApi.Models;
using WiiTrakApi.SPModels;
using WiiTrakApi.DTOs;


namespace WiiTrakApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-one relationships
            modelBuilder.Entity<CartModel>()
             .HasOne(a => a.TrackingDevice)
             .WithOne(b => b.Cart)
             .HasForeignKey<TrackingDeviceModel>(b => b.CartId);

            // Configure one-to-many relationships
            modelBuilder.Entity<CompanyModel>()
                .HasOne(p => p.SystemOwner)
                .WithMany(b => b.Companies)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ServiceProviderModel>()
                .HasOne(p => p.Company)
                .WithMany(b => b.ServiceProviders)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<StoreModel>()
                .HasOne(p => p.Company)
                .WithMany(b => b.Stores)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StoreModel>()
                .HasOne(p => p.Corporate)
                .WithMany(b => b.Stores)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<StoreModel>()
                .HasOne(p => p.ServiceProvider)
                .WithMany(b => b.Stores)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<DriverModel>()
                .HasOne(p => p.Company)
                .WithMany(b => b.Drivers)
                .OnDelete(DeleteBehavior.ClientSetNull);

           

            modelBuilder.Entity<TechnicianModel>()
              .HasOne(p => p.SystemOwner)
              .WithMany(b => b.Technicians)
              .OnDelete(DeleteBehavior.ClientSetNull);

           

            // Configure many-to-many relationships
            modelBuilder.Entity<DriverStoreModel>().HasKey(x => new { x.DriverId, x.StoreId });
            modelBuilder.Entity<CompanyCorporateModel>().HasKey(x => new { x.CompanyId, x.CorporateId });
            
            modelBuilder.Entity<SpGetDriverAssignedStoresByCompany>().HasNoKey();
            modelBuilder.Entity<SpGetDriverAssignedStoresBySystemOwner>().HasNoKey();
            modelBuilder.Entity<SpGetDriverAssignedStores>().HasNoKey();
            modelBuilder.Entity<SpGetNotification>().HasNoKey();
            modelBuilder.Entity<SPGetStoresBySystemOwnerId>().HasNoKey();
            modelBuilder.Entity<SpGetDriverAssignedStores>().HasNoKey();
            modelBuilder.Entity<SPGetServiceBoardDetailsById>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }

        // public DbSet<AppUser> AppUsers { get; set; } = default!;
        public DbSet<SystemOwnerModel> SystemOwners { get; set; } = default!;
        public DbSet<CompanyModel> Companies { get; set; } = default!;
        public DbSet<CorporateModel> Corporates { get; set; } = default!;
        public DbSet<CompanyCorporateModel> CompanyCorporates { get; set; } = default!;
        public DbSet<CartModel> Carts { get; set; } = default!;
        public DbSet<CartHistoryModel> CartHistory { get; set; } = default!;
        public DbSet<TrackingDeviceModel> TrackingDevices { get; set; } = default!;
        public DbSet<ServiceProviderModel> ServiceProviders { get; set; } = default!;
        public DbSet<StoreModel> Stores { get; set; } = default!;
        public DbSet<TechnicianModel> Technicians { get; set; } = default!;
        public DbSet<DriverModel> Drivers { get; set; } = default!;
        public DbSet<DriverStoreModel> DriverStores { get; set; } = default!;
        public DbSet<DeliveryTicketModel> DeliveryTickets { get; set; } = default!;
        public DbSet<WorkOrderModel> WorkOrders { get; set; } = default!;
        public DbSet<RepairIssueModel> RepairIssues { get; set; } = default!;
        public DbSet<UsersModel> Users { get; set; } = default!;
        public DbSet<NotificationModel> Notification { get; set; } = default!;
        public DbSet<CountyCodeModel> CountyCode { get; set; } = default!;

        public DbSet<SpGetDriverAssignedStoresByCompany> SpGetDriverAssignedStoresByCompany { get; set; }
        public DbSet<SpGetDriverAssignedStoresBySystemOwner> SpGetDriverAssignedStoresBySystemOwner { get; set; }
        public DbSet<SpGetDriverAssignedStores> SpGetDriverAssignedStores { get; set; }
        public DbSet<SpGetNotification> SpGetNotifications { get; set; }
        public DbSet<SPGetStoresBySystemOwnerId> SPGetStoresBySystemOwnerId { get; set; }
        public DbSet<SPGetDeliveryTicketsById> SPGetDeliveryTicketsById { get; set; }

        public DbSet<SPGetServiceBoardDetailsById> SPGetServiceBoardDetailsById { get; set; }

    }
}
