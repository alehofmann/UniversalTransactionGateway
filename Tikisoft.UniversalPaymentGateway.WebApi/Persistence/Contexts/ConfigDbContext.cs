using Microsoft.EntityFrameworkCore;
using TikiSoft.UniversalPaymentGateway.Domain.Model;
using TikiSoft.UniversalPaymentGateway.Domain.Model.Merchants;

namespace TikiSoft.UniversalPaymentGateway.Persistence.Contexts
{
    public class ConfigDbContext : DbContext
    {
        public const int NullMerchantId = 0;
        public const string DefaultTerminalId = "default";

        public DbSet<ConfigItem> Config { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<MerchantConfigItem> MerchantConfig { get; set; }

        public ConfigDbContext (DbContextOptions<ConfigDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ConfigItem>()
                .ToTable("ProcessorConfig");

            builder.Entity<Merchant>()
                .ToTable("MerchantInfo")
                .HasKey(k => k.Id);

            builder.Entity<Merchant>()
                .Property(p => p.Id)
                .HasColumnName("MerchantID");

            builder.Entity<Merchant>()
                .HasMany(c => c.Config).WithOne(e => e.Merchant);

            builder.Entity<Merchant>()
                .Property(p => p.Name)
                .IsRequired();

            builder.Entity<MerchantConfigItem>()
                .ToTable("MerchantConfig");

            builder.Entity<MerchantConfigItem>()
                .HasOne(p => p.Merchant)
                .WithMany(b => b.Config)
                .HasForeignKey("MerchantId");

            builder.Entity<MerchantConfigItem>()
                .HasAlternateKey("MerchantId", "TerminalId", "Processor", "Key");
        }



    }
}
