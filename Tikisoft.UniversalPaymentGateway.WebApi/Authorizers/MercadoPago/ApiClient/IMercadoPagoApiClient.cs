using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient.Dto;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient
{
    public interface IMercadoPagoApiClient
    {
        Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request);
        Task<SearchPaymentsResponseDto> SearchPaymentAsync(string externalReference);
    }
}
