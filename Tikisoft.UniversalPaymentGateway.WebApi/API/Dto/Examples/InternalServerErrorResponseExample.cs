using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.API.Dto;

namespace TikiSoft.UniversalPaymentGateway.Dto.Examples
{
    public class InternalServerErrorResponseExample : IExamplesProvider<ErrorResponseDto>
    {
        public ErrorResponseDto GetExamples()
        {
            return new ErrorResponseDto()
            {
                ErrorCode = "CommunicationsError",
                ErrorSource = "utg",
                ErrorList = null,
                ErrorMessage = "Error de comunicación con el host de MercardoPago: No such host is known."
            };
        }
    }
}
