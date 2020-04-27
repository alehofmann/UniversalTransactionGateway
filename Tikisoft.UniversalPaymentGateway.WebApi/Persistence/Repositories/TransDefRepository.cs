using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.Models;
using TikiSoft.UniversalPaymentGateway.Persistence.Contexts;
using TikiSoft.UniversalPaymentGateway.Domain.Repositories;
using TikiSoft.UniversalPaymentGateway.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TikiSoft.UniversalPaymentGateway.Persistence.Repositories
{
    public class TransDefRepository : EfCoreRepository, ITransDefRepository
    {
        public TransDefRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TransDef>> ListAsync()
        {
            var transDefs = await _context.TransDefs.Include(td=>td.Items).ToListAsync();
            //var items = await _context.TransItems.ToListAsync();
            return transDefs;
        }

        public async Task AddAsync(TransDef transDef)
        {
            await _context.AddAsync(transDef);            
        }
    }
}
