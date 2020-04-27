using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.Infrastructure;
using TikiSoft.UniversalPaymentGateway.Domain.Models;
using TikiSoft.UniversalPaymentGateway.Domain.Repositories;
using TikiSoft.UniversalPaymentGateway.Domain.Services.Communication;

namespace TikiSoft.UniversalPaymentGateway.Domain.Services
{
    public class AuthorizerService: IAuthorizerService 
    {
        private readonly ITransDefRepository _transactionRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorizer _internalAuthorizer;
        
        public AuthorizerService(IAuthorizer internalAuthorizer, ITransDefRepository transactionRepo, IUnitOfWork unitOfWork)
        {
            _transactionRepo = transactionRepo;
            _unitOfWork = unitOfWork;
            _internalAuthorizer = internalAuthorizer;
        }
        

        async Task<ProcessTransactionResponse> IAuthorizerService.ProcessTransactionAsync(TransDef transaction)
        {
          
            try
            {
                var transResponse = await _internalAuthorizer.ProcessTransactionAsync(transaction);
                await _transactionRepo.AddAsync(transaction);
                await _unitOfWork.CompleteAsync();

                return (new ProcessTransactionResponse(transResponse));                
            }
            catch (Exception ex)
            {
                //do some logging stuff
                return new ProcessTransactionResponse($"An error ocurred processing transaction: {ex.Message}");
            }

            
        }

        public async Task<IEnumerable<TransDef>> ListTransactionsAsync()
        {
            return await _transactionRepo.ListAsync();
        }
    }
}
