using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using TikiSoft.UniversalPaymentGateway.API.ApplicationServices;
using TikiSoft.UniversalPaymentGateway.API.Dto;
using TikiSoft.UniversalPaymentGateway.Dto.Examples;

namespace TikiSoft.UniversalPaymentGateway.API.Controllers
{
    /// <response code="500">Error interno, usualmente runtime</response>           
    /// <response code="401">Unauthorized: El header "Authorization: Bearer --token--" no existe o token inválido"</response>           
    [ApiController()]
    [Route("api/config")]   
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(500, typeof(InternalServerErrorResponseExample))]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ConfigController :Controller
    {
        IProcessorConfigService _processorConfigService;

        public ConfigController(IProcessorConfigService processorConfigService)
        {
            _processorConfigService = processorConfigService;
        }

        /// <summary>
        /// Obtiene la configuración de un processor. Método sólo válido cuando el hosting NO es Cloud.
        /// </summary>
        /// <remarks>        
        ///     Esta operación no chequea la existencia del processor en cuestión, de no existir devolverá una colección vacía.  
        ///     El parámetro terminal_id es opcional ya que algunos processors tienen configuración común a todas sus terminales,
        ///     o directamente no utilizan el concepto de "terminal".  
        ///     De no ser incluído el parámetro se devolverá la configuración de la terminal por defecto.
        /// </remarks>        
        /// <param name="processor">Nombre del processor</param>
        /// <param name="terminalId">Id de la terminal o punto de venta.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [HttpGet()]        
        public async Task<IActionResult> Get(
            string processor,            
            string terminalId = "")
        {
            if (processor is null || processor == "")
                return BadRequest(ErrorResponseDto.CustomInvalidRequest("'processor' parameter missing"));

            return Ok(await _processorConfigService.GetConfig(processor, terminalId));
        }

        /// <summary>
        /// Configura un Processor. Método sólo válido cuando el hosting NO es Cloud.
        /// </summary>
        /// <remarks>        
        /// Esta operación no chequea la existencia del processor en cuestión, sencillamente configura un processor con el nombre especificado.  
        /// El parámetro terminal_id es opcional ya que algunos processors tienen configuración común a todas sus terminales,
        /// o directamente no utilizan el concepto de "terminal".  
        /// De no ser incluído el parámetro se configurará el la terminal por defecto.
        ///     
        /// Sample request:
        ///
        ///     POST api/config/lapos?terminal_id=pos-0001
        ///        {
        ///             [
	    ///                 {
		///                     "key":"serialport",
		///                     "value":"com3"
	    ///                 },
	    ///                 {
		///                     "key":"menchantId",
		///                     "value":"1111113"
	    ///                 },
	    ///                 {
		///                     "key":"merchantName",
		///                     "value":"Test Merchant
	    ///                 }
	    ///             ]
        ///        }        
        /// 
        /// </remarks>        
        /// <param name="processor">Nombre del processor</param>
        /// <param name="terminalId">Id de la terminal o punto de venta.</param>
        /// <param name="config">Datos de la configuración.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost()]
        public async Task<IActionResult> Post(string processor, [FromBody()] IList<ConfigItemDto> config, [FromQuery(Name = "terminal_id")] string terminalId = "default")
        {
            await _processorConfigService.ConfigureProcessor(processor, config, terminalId);
            return Ok();
        }
    }
}
