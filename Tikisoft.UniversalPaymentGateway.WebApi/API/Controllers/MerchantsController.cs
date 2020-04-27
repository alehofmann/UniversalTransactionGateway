using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using TikiSoft.UniversalPaymentGateway.API.ApplicationServices;
using TikiSoft.UniversalPaymentGateway.API.Dto;
using TikiSoft.UniversalPaymentGateway.Dto.Examples;
using TikiSoft.UniversalPaymentGateway.Persistence.Contexts;

namespace TikiSoft.UniversalPaymentGateway.API.Controllers
{
    /// <response code="200">Operación exitosa</response>                
    [ApiController()]
    [Route("api/merchants")]    
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(500, typeof(InternalServerErrorResponseExample))]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class MerchantsController : Controller
    {
        ConfigDbContext _context;
        IMerchantConfigService _merchantConfigService;     
        
        public MerchantsController(ConfigDbContext context, IMerchantConfigService merchantConfigService)
        {            
            _context = context;
            _merchantConfigService = merchantConfigService;
        }

        /// <summary>Recupera una lista de toros los Merchants en la base de datos.</summary>                
        /// <remarks>Requiere privilegios de Admin</remarks>        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var retVal = await _context.Merchants.ToListAsync();
            return Ok(retVal);

        }

        /// <summary>
        /// Recupera la configuración para todos los Processors de un Merchant de la base de datos.
        /// </summary>        
        /// <param name="merchantId">Id del Merchant a recuperar</param>
        /// <param name="processor">Opcional: nombre del processor a filtrar</param>
        /// <param name="terminalId">Opcional: nombre de la terminal a filtrar</param>        
        /// <response code="404">NotFound: El merchantId no existe en la base de datos</response>                   
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        [HttpGet("{merchantId}/config")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        public async Task<IActionResult> GetConfig(int merchantId, string processor, [FromQuery(Name = "terminal_id")] string terminalId="")
        {
            var merchant = await _context.Merchants.Include(p => p.Config).FirstOrDefaultAsync(p => p.Id == merchantId);
            if (merchant is null)
            {
                return NotFound(ErrorResponseDto.FromNotFoundInDb("merchantId", merchantId.ToString()));
            }

            var config=await _merchantConfigService.GetConfig(processor, merchantId, terminalId);

            if (processor is null || processor == "")
                return Ok(config);
            else
                return Ok(config.Where(p=>p.Processor==processor));
        }

        /// <summary>
        /// Recupera los datos de un Merchant de la base de datos
        /// </summary>
        /// <param name="merchantId">Id del Merchant a recuperar</param>        
        /// <response code="404">NotFound: El merchantId no existe en la base de datos</response>           
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{merchantId}")]
        public async Task<IActionResult> Get(int merchantId)
        {
            var merchant = await _context.Merchants.FirstOrDefaultAsync(p => p.Id == merchantId);
            if (merchant is null)
            {
                return NotFound(ErrorResponseDto.FromNotFoundInDb("merchantId", merchantId.ToString()));
            }
            return Ok(merchant);

        }

        /// <summary>
        /// Configura un Processor para un Merchant determinado (entornos Cloud).
        /// </summary>
        /// <remarks>        
        /// Esta operación no chequea la existencia del processor en cuestión, sencillamente configura un processor con el nombre especificado.  
        /// El parámetro terminal_id es opcional ya que algunos processors tienen configuración común a todas sus terminales,
        /// o directamente no utilizan el concepto de "terminal".  
        /// De no ser incluído el parámetro se configurará el la terminal por defecto.
        /// Sample request:
        ///
        ///     POST api/merchants/5/config/mercadopago
        ///        {
        ///             [
        ///                 {
        ///                     "key":"token",
        ///                     "value":"TEST-670277105605974-112019-ea4cf085d895535fb933d8dbd26ef055-491531505"
        ///                 },
        ///                 {
        ///                     "user_id":"user_id",
        ///                     "value":"491531505"
        ///                 },
        ///                 {
        ///                     "key":"order_timeout",
        ///                     "value":300
        ///                 }
        ///             ]
        ///        }        
        /// </remarks>        
        /// <param name="merchantId">Id del comercio a configurar</param>
        /// <param name="processor">Nombre del processor</param>
        /// <param name="terminalId">Id de la terminal o punto de venta.</param>
        /// <param name="config">Datos de la configuración.</param>                
        /// <response code="401">Unauthorized: El header "Authorization: Bearer --token--" no existe o token inválido"</response>     
        /// <response code="404">NotFound: El merchantId no existe en la base de datos</response>
        /// <response code="500">Error interno, usualmente runtime</response>           
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{merchantId}/config")]
        public async Task<IActionResult> Post(int merchantId, [FromQuery(Name = "processor")] string processor, [FromBody()]IList<ConfigItemDto> config, [FromQuery(Name = "terminal_id")] string terminalId)
        {
            var merchant = await _context.Merchants.Include(p => p.Config).FirstOrDefaultAsync(p => p.Id == merchantId);
            if (merchant is null)
            {
                return NotFound(ErrorResponseDto.FromNotFoundInDb("merchantId", merchantId.ToString()));
            }

                await _merchantConfigService.ConfigureProcessor(processor, merchantId, config, terminalId);
                        
            return Ok();
        }


    }
}
