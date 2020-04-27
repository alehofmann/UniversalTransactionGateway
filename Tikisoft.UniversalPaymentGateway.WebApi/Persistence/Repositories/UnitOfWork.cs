using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.DataInterfaces;
using TikiSoft.UniversalPaymentGateway.Persistence.Contexts;

namespace TikiSoft.UniversalPaymentGateway.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TransactionDbContext _dbContext;

        public UnitOfWork(TransactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
