using InterviewTask.Model;
using Microsoft.EntityFrameworkCore;

namespace InterviewTask.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<DrugLabelCache> DrugLabelCaches => Set<DrugLabelCache>();
        public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
        public DbSet<AvailabilityRequest> AvailabilityRequests => Set<AvailabilityRequest>();
        public DbSet<AllergyCheckLog> AllergyCheckLogs => Set<AllergyCheckLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DrugLabelCache>(e =>
            {
                e.ToTable("DrugLabelCache");

                e.HasKey(x => x.Id);

                e.Property(x => x.SetId).IsRequired().HasMaxLength(80);
                e.Property(x => x.QueryKey).IsRequired().HasMaxLength(200);

                e.Property(x => x.BrandName).HasMaxLength(200);
                e.Property(x => x.GenericName).HasMaxLength(200);
                e.Property(x => x.ManufacturerName).HasMaxLength(200);

                e.Property(x => x.JsonLabel).IsRequired();
                e.Property(x => x.CachedAtUtc).IsRequired();

                e.HasIndex(x => x.SetId).IsUnique();
                e.HasIndex(x => x.QueryKey);
            });

            modelBuilder.Entity<InventoryItem>(e =>
            {
                e.ToTable("InventoryItem");

                e.HasKey(x => x.Id);

                e.Property(x => x.DrugKey).IsRequired().HasMaxLength(120);
                e.Property(x => x.DisplayName).HasMaxLength(250);

                e.Property(x => x.Quantity).IsRequired();
                e.Property(x => x.UpdatedAtUtc).IsRequired();

                e.HasIndex(x => x.DrugKey).IsUnique();
            });

            modelBuilder.Entity<AvailabilityRequest>(e =>
            {
                e.ToTable("AvailabilityRequest");

                e.HasKey(x => x.Id);

                e.Property(x => x.DrugKey).IsRequired().HasMaxLength(120);
                e.Property(x => x.Email).IsRequired().HasMaxLength(320);

                e.Property(x => x.Status).IsRequired(); 
                e.Property(x => x.CreatedAtUtc).IsRequired();

                e.HasIndex(x => new { x.DrugKey, x.Status });
                e.HasIndex(x => x.Email);
            });

            modelBuilder.Entity<AllergyCheckLog>(e =>
            {
                e.ToTable("AllergyCheckLog");

                e.HasKey(x => x.Id);

                e.Property(x => x.DrugKey).IsRequired().HasMaxLength(120);
                e.Property(x => x.AllergensRaw).IsRequired().HasMaxLength(1000);

                e.Property(x => x.ResultJson).IsRequired();
                e.Property(x => x.CheckedAtUtc).IsRequired();

                e.HasIndex(x => x.DrugKey);
                e.HasIndex(x => x.CheckedAtUtc);
            });

        }
        }
}
