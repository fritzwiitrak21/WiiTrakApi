using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WiiTrakApi.Models;

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
            modelBuilder.Entity<AssetModel>()
             .HasOne(a => a.TrackingDevice)
             .WithOne(b => b.Asset)
             .HasForeignKey<TrackingDeviceModel>(b => b.AssetId);

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
            modelBuilder.Entity<DriverStore>().HasKey(x => new { x.DriverId, x.StoreId });

            base.OnModelCreating(modelBuilder);
        }

        // public DbSet<AppUser> AppUsers { get; set; } = default!;
        public DbSet<SystemOwnerModel> SystemOwners { get; set; } = default!;
        public DbSet<CompanyModel> Companies { get; set; } = default!;
        public DbSet<CorporationModel> Corporations { get; set; } = default!;
        public DbSet<AssetModel> Assets { get; set; } = default!;
        public DbSet<TrackingDeviceModel> TrackingDevices { get; set; } = default!;
        public DbSet<ServiceProviderModel> ServiceProviders { get; set; } = default!;
        public DbSet<StoreModel> Stores { get; set; } = default!;
        public DbSet<TechnicianModel> Technicians { get; set; } = default!;
        public DbSet<DriverModel> Drivers { get; set; } = default!;
        public DbSet<DriverStore> DriverStores { get; set; } = default!;
        public DbSet<PickupModel> Pickups { get; set; } = default!;
        public DbSet<ProvisionModel> Provisions { get; set; } = default!;
        public DbSet<WorkOrderModel> WorkOrders { get; set; } = default!;
        public DbSet<RepairIssueModel> RepairIssues { get; set; } = default!;

    }
}
