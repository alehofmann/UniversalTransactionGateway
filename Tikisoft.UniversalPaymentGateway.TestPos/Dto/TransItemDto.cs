using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Tikisoft.UniversalPaymentGateway.TestPos.Dto
{
    public class TransItemDto
    {
        
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }
    }
}
