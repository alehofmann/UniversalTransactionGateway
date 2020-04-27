using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.API.Dto
{
    public class ConfigItemDto
    {
        [JsonRequired]
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonRequired]
        [JsonProperty("value")]
        public string Value { get; set; }

    }
}
