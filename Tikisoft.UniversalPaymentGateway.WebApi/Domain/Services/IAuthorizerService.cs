using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.Models;
using TikiSoft.UniversalPaymentGateway.Domain.Services.Communication;

namespace TikiSoft.UniversalPaymentGateway.Domain.Services
{
    public interface IAuthorizerService
    {
        Task<ProcessTransactionResponse> ProcessTransactionAsync(TransDef transDef);
        Task<IEnumerable<TransDef>> ListTransactionsAsync();

    }
}
