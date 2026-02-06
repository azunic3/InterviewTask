using InterviewTask.Model;
using InterviewTask.Enums;
using Microsoft.EntityFrameworkCore;

namespace InterviewTask.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
        public DbSet<AvailabilityRequest> AvailabilityRequests => Set<AvailabilityRequest>();
        public DbSet<AllergyCheckLog> AllergyCheckLogs => Set<AllergyCheckLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InventoryItem>(e =>
            {
                e.ToTable("InventoryItem");

                e.HasKey(x => x.Id);

                e.Property(x => x.DrugKey)
                    .IsRequired()
                    .HasMaxLength(120);

                e.Property(x => x.Status)
                    .IsRequired()
                    .HasConversion<int>(); 

                e.Property(x => x.Quantity);

                e.Property(x => x.LastUpdated)
                    .IsRequired();

                e.HasIndex(x => x.DrugKey).IsUnique();
            });

            modelBuilder.Entity<AvailabilityRequest>(e =>
            {
                e.ToTable("AvailabilityRequest");

                e.HasKey(x => x.Id);

                e.Property(x => x.DrugKey)
                    .IsRequired()
                    .HasMaxLength(120);

                e.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(320);

                e.Property(x => x.Status)
                    .IsRequired()
                    .HasConversion<int>(); 

                e.Property(x => x.CreatedAtUtc)
                    .IsRequired();

                e.HasIndex(x => new { x.Email, x.DrugKey }).IsUnique();

                e.HasIndex(x => x.Status);
            });

            modelBuilder.Entity<AllergyCheckLog>(e =>
            {
                e.ToTable("AllergyCheckLog");

                e.HasKey(x => x.Id);

                e.Property(x => x.Query)
                    .IsRequired()
                    .HasMaxLength(200);

                e.Property(x => x.DrugKey)
                    .IsRequired()
                    .HasMaxLength(120);

                e.Property(x => x.AllergensRaw)
                    .IsRequired()
                    .HasMaxLength(1000);

                e.Property(x => x.ResultJson)
                    .IsRequired();

                e.Property(x => x.CheckedAtUtc)
                    .IsRequired();

                e.HasIndex(x => x.DrugKey);
                e.HasIndex(x => x.CheckedAtUtc);
            });
        }
    }
}
