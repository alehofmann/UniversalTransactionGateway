using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.Models;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.Persistance

{
    public class NotifierDbContext : DbContext
    {
        public NotifierDbContext(DbContextOptions<NotifierDbContext> options) : base(options) {}

    

        public DbSet<TransactionRecord> Transactions { get; set; }
        public DbSet<NotificationRecord> Notifications { get; set; }
    }
}
