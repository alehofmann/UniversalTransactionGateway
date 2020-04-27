using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tikisoft.UniversalPaymentGateway.TestPos.Dto
{
    class TransDefDto
    {
        [JsonProperty("id")]
        public long TransDefId { get; set; }

        [JsonProperty("trans_type")]
        public int TransType { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("card_type")]
        public string CardType { get; set; }

        [JsonProperty("gratuity_amount")]
        public decimal Gratuity { get; set; }

        [JsonProperty("transaction_id")]
        public long TransactionId { get; set; }

        [JsonProperty("transaction_reference")]
        public string TransactionReference { get; set; }

        [JsonProperty("pos_id")]
        public string PosId { get; set; }

        [JsonProperty("items")]
        public IList<TransItemDto> Items { get; set; }
    }
}
