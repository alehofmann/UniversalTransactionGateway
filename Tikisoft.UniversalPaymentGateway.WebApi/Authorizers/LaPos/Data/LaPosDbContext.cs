using Microsoft.EntityFrameworkCore;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms.Model;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Data
{
    public class LaPosDbContext : DbContext
    {

        public LaPosDbContext(DbContextOptions<LaPosDbContext> options)
        : base(options)
        { }

        public DbSet<CardType> CardTypes { get; set; }
    }


}
