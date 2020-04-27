using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient.Dto
{
    public class CreateOrderResponseDto
    {
        [JsonPropertyName("id")]
        public string OrderId { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
        
        public string Content { get; set; }

        public string ErrorMessage { 
            get
            {
                if (Status == 404)
                {
                    return ("Status " + Status + " (Route Not Found: '" + RequestUri + "')");
                }
                else
                {
                    return ("Status " + Status + " (" + Message + ")");
                }
            }
        }
        public bool Success { get; set; }
        public string RequestUri { get; set; }
       
    }
}
