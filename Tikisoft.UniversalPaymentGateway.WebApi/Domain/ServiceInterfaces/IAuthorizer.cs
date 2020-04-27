using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.Model;
using Microsoft.Extensions.Configuration;

namespace TikiSoft.UniversalPaymentGateway.Domain.ServiceInterfaces
{
    public interface IAuthorizer : IDisposable

    {       
        Task<Domain.Model.TransactionResponse> ProcessTransactionAsync(TransactionRequest transDef);
        Task<AdminCommandResponse<IList<Domain.Model.CardType>>> GetAcceptedCardTypesAsync();        
    }
}
