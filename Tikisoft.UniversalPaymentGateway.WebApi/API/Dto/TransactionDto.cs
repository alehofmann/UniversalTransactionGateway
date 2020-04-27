using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TikiSoft.UniversalPaymentGateway.Domain.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using Newtonsoft.Json;

namespace TikiSoft.UniversalPaymentGateway.API.Dto
{
    /// <example>
    /// { "name": "Smith" }
    /// </example>
    public class TransactionDto
    {
        [JsonProperty("id")]
        [JsonIgnore]
        public long TransDefId { get; set; }

        //Si no lo configuro como nullable, lo que ocurre es que el "Required" no funciona como debe y defaultea a 0 cuando no se manda el parametro trans_type
        [Required]
        [JsonProperty("trans_type")]
        public TransactionType? TransType { get; set; }

        //[Required]
        //[JsonProperty("target_authorizer")]
        //public AuthorizersEnum TargetAuthorizer { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        //[JsonProperty("card_type")]
        //public string CardType { get; set; }

        [JsonProperty("gratuity_amount")]
        public decimal Gratuity { get; set; }

        //[JsonProperty("transaction_id")]
        //public long TransactionId { get; set; }

        [JsonProperty("transaction_reference")]
        [Required]
        public string TransactionReference { get; set; }

        [Required]
        [JsonProperty("pos_id")]
        public string PosId { get; set; }

        [JsonProperty("items")]
        public IList<TransItemDto> Items { get; set; }

        [DefaultValue(0)]
        [JsonProperty("merchant_id")]
        public int MerchantId { get; set; }
        
        [JsonProperty("card_code")]
        public string CardCode { get; set; }
    }
}
