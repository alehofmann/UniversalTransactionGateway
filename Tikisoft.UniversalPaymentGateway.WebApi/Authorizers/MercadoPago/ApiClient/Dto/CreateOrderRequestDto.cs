using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.Model;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient.Dto

{
    public class CreateOrderRequestDto
    {

        /*[JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; }*/

        [JsonPropertyName("external_reference")]
        public string ExternalReference { get; set; }

        [JsonPropertyName("notification_url")]
        public string NotificationUrl { get; set; }

        public string ExternalId { get; set; }

        [JsonPropertyName("items")]        
        public IList<OrderItemDto> Items { get; set; }

        public CreateOrderRequestDto() { }

        public CreateOrderRequestDto(TransactionRequest transaction)
        {
            ExternalReference = transaction.TransactionReference;
                ExternalId = transaction.PosId;
                Items = transaction.Items.Select(i => new OrderItemDto()
                {
                    CurrencyID = "ARS",
                    Description = i.Description,
                    ItemQuantity = i.Quantity,
                    Title = i.Description,
                    UnitPrice = (float)i.UnitPrice
                }).ToList();         
        }
    }
 
}
