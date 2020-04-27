using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.DataInterfaces;
using TikiSoft.UniversalPaymentGateway.Persistence.Contexts;

namespace TikiSoft.UniversalPaymentGateway.Persistence.Repositories
{
    public abstract class EfCoreRepository<TEntity, TContext, TIdType> : IRepository<TEntity,TIdType>
       where TEntity : class
       where TContext : DbContext
    {
        //private readonly TContext Context;
        public TContext Context { get;}
        public EfCoreRepository(TContext context)
        {
            this.Context = context;
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Delete(TIdType id)
        {
            var entity = await Context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Get(TIdType id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IList<TEntity>> GetAll()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return entity;
        }

    }
}
