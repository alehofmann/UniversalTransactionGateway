using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Domain.Model
{
    public class CardInfo
    {
        [JsonProperty("first_six")]
        public string FirstSix { get; set; }
        [JsonProperty("last_four")]
        public string LastFour { get; set; }
        [JsonProperty("card_type")]
        public string CardType { get; set; }
    }
}
