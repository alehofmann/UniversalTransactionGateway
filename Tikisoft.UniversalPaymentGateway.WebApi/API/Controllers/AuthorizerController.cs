using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TikiSoft.UniversalPaymentGateway.Domain.Model;
using Swashbuckle.AspNetCore.Filters;
using TikiSoft.UniversalPaymentGateway.Dto.Examples;
using Microsoft.AspNetCore.Authorization;
using TikiSoft.UniversalPaymentGateway.API.Dto;
using TikiSoft.UniversalPaymentGateway.API.Dto.Examples;
using TikiSoft.UniversalPaymentGateway.API.ApplicationServices;

namespace TikiSoft.UniversalPaymentGateway.API.Controllers
{       
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AuthorizerController : ControllerBase
    {
        private readonly IAuthorizerFactory _authorizerFactory;
        private readonly IMapper _mapper;

        public AuthorizerController(IAuthorizerFactory authorizerFactory, IMapper mapper)
        {
            _authorizerFactory = authorizerFactory;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene una lista de las tarjetas aceptadas por el processor.
        /// </summary>
        /// <param name="processor">Nombre del processor</param>      
        /// 
        /// <response code="400">Falta el parámetro "processor"</response>           
        /// <response code="404">El "processor" no corresponde a un processor válido</response>           
        /// <response code="200">La transacción contra el Processor fue exitosa</response>        
        /// <response code="500">Error interno, por ejemplo de runtime o de comunicacion con el Processor</response>           
        /// <response code="401">Unauthorized: El header "Authorization: Bearer --token--" no existe o token inválido"</response>                       
        [ProducesResponseType(typeof(CardTypesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [HttpGet("accepted-cards")]
        [Authorize()]
        public async Task<IActionResult> AcceptedCards(string processor)
        {
            AdminCommandResponse<IList<CardType>> result;

            using (var authorizerService = _authorizerFactory.Create(processor))
            {
                if (authorizerService is null)
                    return NotFound(
                        ErrorResponseDto.FromMissingProcessor(processor)
                        );

                result = await authorizerService.GetAcceptedCardTypesAsync();
            }

            if (result.Success)
            {
                return Ok(new CardTypesResponseDto(result));
            }
            else
            {
                return BadRequest(ErrorResponseDto.FromProcessorResponse(result));
            }
        }


        /// <summary>
        /// Procesa una transacción de pago, cancelación o devolución.
        /// </summary>
        /// <param name="processor">Processor contra el cual se efectúa la transacción</param>
        /// <param name="transaction">Contenido de la transacción a procesar</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/transaction        
        ///        {
        ///             "trans_type": 1,
        ///             "amount": 0,
        ///             "card_type": "VISA",
        ///             "gratuity_amount": 0,
        ///             "transaction_id": 1234,
        ///             "transaction_reference": "Fact-046",
        ///             "pos_id": "CAJA0001",
        ///             "items" :[{
        ///                    "unit_price" : 125.00,
        ///                    "quantity" : 1,
        ///                    "description": "Almendras"    
        ///                         }]
        ///        }        
        /// </remarks>                
        /// <returns>ErrorResponseDto en caso de error, o SuccessResponseDto en caso de operación exitosa</returns>
        /// <response code="200">La transacción contra el Processor fue exitosa, lo que NO quiere decir que fue aprobada</response>        
        /// <response code="400">Json request inválido. La validación puede haber fallado en el UTG, el Processor o a nivel Framework</response>           
        /// <response code="500">Error interno, por ejemplo de runtime o de comunicacion con el Processor</response>           
        /// <response code="401">Unauthorized: El header "Authorization: Bearer --token--" no existe o token inválido"</response>                       

        [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(TransactionDto), typeof(TransactionDtoExample))]
        [SwaggerResponseExample(400, typeof(BadRequestResponseExample))]
        [SwaggerResponseExample(500, typeof(InternalServerErrorResponseExample))]
        [HttpPost("transaction")]
        [Authorize()]
        public async Task<IActionResult> Transaction([FromBody] TransactionDto transaction, string processor)
        {
            var transDef = _mapper.Map<TransactionDto, TransactionRequest>(transaction);

            TransactionResponse result;
            using (var authorizerService = _authorizerFactory.Create(processor))
            {
                if (authorizerService is null)
                    return BadRequest(ErrorResponseDto.FromMissingProcessor(processor));

                result = await authorizerService.ProcessTransactionAsync(transDef);
            }

            switch (result.ResultCode)
            {
                case BaseResponse.ResultCodesEnum.InternalProcessorError:
                case BaseResponse.ResultCodesEnum.CommunicationsError:
                    return StatusCode(500, ErrorResponseDto.FromProcessorResponse(result));
                case BaseResponse.ResultCodesEnum.NotReady:
                case BaseResponse.ResultCodesEnum.InvalidParametersError:
                case BaseResponse.ResultCodesEnum.ProcessorError:
                    return BadRequest(ErrorResponseDto.FromProcessorResponse(result));
                default:
                    return Ok(new TransactionResponseDto(result));
            }

        }

    }
}
