using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.API.Dto;

namespace TikiSoft.UniversalPaymentGateway.API.Dto.Examples
{
    public class TransactionDtoExample : IExamplesProvider<TransactionDto>
    {
        public TransactionDto GetExamples()
        {
            return new TransactionDto()
            {
                Amount = (decimal)12.50,
                Gratuity = 0,
                PosId = "CAJA0001",
                //TargetAuthorizer = Domain.Models.AuthorizersEnum.MercadoPago,
                TransactionReference = "FAC-0099",
                TransType = Domain.Model.TransactionType.Sell,
                Items = new List<TransItemDto>()
                {
                    new TransItemDto()
                    {
                        Description="Hamburguesa c/queso",
                        Quantity=1,
                        UnitPrice=6
                    },
                    new TransItemDto()
                    {
                        Description="Papas fritas",
                        Quantity=1,
                        UnitPrice=4
                    },
                    new TransItemDto()
                    {
                        Description="Gaseosa",
                        Quantity=1,
                        UnitPrice=(decimal)2.50
                    }
                }

            };
        }
    }
}
