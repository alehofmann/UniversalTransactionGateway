using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.Model;

namespace TikiSoft.UniversalPaymentGateway.API.Dto
{
    public class CardTypesResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get { return true; } }

        [JsonProperty("card_types")]
        public IList<CardTypeDto> CardTypes { get; }

        public CardTypesResponseDto(AdminCommandResponse<IList<CardType>> response)
        {
            CardTypes = response.Content.Select(x => new CardTypeDto() { Name = x.Name }).ToList();
        }


    }
}
