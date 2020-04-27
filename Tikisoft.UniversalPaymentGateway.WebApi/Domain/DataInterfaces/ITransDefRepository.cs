using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.Model;

namespace TikiSoft.UniversalPaymentGateway.Domain.DataInterfaces
{
    public interface ITransDefRepository
    {
        Task<IEnumerable<TransactionRequest>> ListAsync();
        Task AddAsync(TransactionRequest transDef);
    }
}
