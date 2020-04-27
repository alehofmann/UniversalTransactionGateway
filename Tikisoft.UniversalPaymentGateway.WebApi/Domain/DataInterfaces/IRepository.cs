using System.Collections.Generic;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Domain.DataInterfaces
{
    public interface IRepository<T, TId> where T : class
    {
        Task<IList<T>> GetAll();
        Task<T> Get(TId id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(TId id);
    }
}
