using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.API.Dto;

namespace TikiSoft.UniversalPaymentGateway.API.Dto.Examples
{
    public class BadRequestResponseExample : IExamplesProvider<ErrorResponseDto>
    {
        public ErrorResponseDto GetExamples()
        {
            return new ErrorResponseDto()
            {
                ErrorCode = "ProcessorError",
                ErrorSource = "processor",
                ErrorList = null,
                ErrorMessage = "Error creando la orden: Status 400 (unit_price invalid)"
            };
        }
    }
}
