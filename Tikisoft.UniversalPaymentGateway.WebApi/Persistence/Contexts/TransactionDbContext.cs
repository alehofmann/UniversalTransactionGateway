using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TikiSoft.UniversalPaymentGateway.Domain.Model;
namespace TikiSoft.UniversalPaymentGateway.Persistence.Contexts
{
    public class TransactionDbContext : DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TransactionRequest>().ToTable("TransDef");
            builder.Entity<TransactionRequest>().HasKey(p => p.TransDefId);
            builder.Entity<TransactionRequest>().Property(p => p.TransDefId).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<TransactionRequest>().Property(p => p.Amount);
            builder.Entity<TransactionRequest>().Property(p => p.TransType).IsRequired();
            builder.Entity<TransactionRequest>()
               .HasMany(c => c.Items)
               .WithOne(e => e.TransDef);


            builder.Entity<TransItem>()
                .Property<long>("TransDefId");

            builder.Entity<TransItem>()
                .HasOne(p => p.TransDef)
                .WithMany(b => b.Items)
                .HasForeignKey("TransDefId");


            builder.Entity<TransactionRequest>().HasData
                (
                    new TransactionRequest(TransactionType.Sell)
                    {
                        TransDefId = 100,
                        Amount = 15,
                    }
                );
            builder.Entity<TransItem>().HasData
                (
                    new TransItem
                    {
                        TransDefId=100,
                        TransItemId=1,
                        Quantity = 1,
                        Description = "Item 1",
                        UnitPrice = 10,                        
                    },
                    new TransItem
                    {
                        TransDefId=100,
                        TransItemId=2,
                        Quantity = 2,
                        Description = "Item 2",
                        UnitPrice = 20
                    }
                );

        }

        public DbSet<TransactionRequest> TransDefs { get; set; }
        public DbSet<TransItem> TransItems { get; set; }

    }
}
