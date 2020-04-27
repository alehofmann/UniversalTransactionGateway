using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.API.Dto
{
    public class CardTypeDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
