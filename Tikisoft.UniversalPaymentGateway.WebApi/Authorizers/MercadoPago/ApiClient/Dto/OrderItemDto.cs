
using System.Text.Json.Serialization;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient.Dto
{
    public class OrderItemDto
    {
        [JsonPropertyName("title")]        
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("currency_id")]
        public string CurrencyID { get; set; }

        [JsonPropertyName("unit_price")]
        public float UnitPrice { get; set; }

        [JsonPropertyName("quantity")]
        public int ItemQuantity { get; set; }
    }
}
