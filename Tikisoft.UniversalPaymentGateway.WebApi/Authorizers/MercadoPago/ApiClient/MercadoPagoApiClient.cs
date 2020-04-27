using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient.Dto;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient
{
    public class MercadoPagoApiClient : IMercadoPagoApiClient
    {     
        private string _userId;        
        private string _accessToken;
        private string _baseUri = "https://api.mercadopago.com";
        private int _orderTimeoutSeconds;
        public MercadoPagoApiClient(string userId, string accessToken, int orderTimeoutSeconds)
        {
            _accessToken = accessToken;
            _userId = userId;
            _orderTimeoutSeconds = orderTimeoutSeconds;

        }
        public async Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderRequestDto order)
        {
            var uri = _baseUri + "/mpmobile/instore/qr/" + _userId + "/" + order.ExternalId + "?access_token=" + _accessToken;

            using (var client=new HttpClient())
            {
                HttpContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");
                content.Headers.Add("X-Ttl-Store-Preference", _orderTimeoutSeconds.ToString());
                HttpResponseMessage response = await client.PostAsync(uri, content);
              
                var responseText = await response.Content.ReadAsStringAsync();
                var responseDto = JsonSerializer.Deserialize<CreateOrderResponseDto>(responseText);                
                responseDto.Success = response.IsSuccessStatusCode;
                responseDto.Status= (int)response.StatusCode;
                responseDto.Content = responseText;
                responseDto.RequestUri = response.RequestMessage.RequestUri.OriginalString;

                return (responseDto);
            }
        }

        public async Task<SearchPaymentsResponseDto> SearchPaymentAsync(string externalReference)
        {
            var uri = _baseUri + "/v1/payments/search/?access_token=" + _accessToken + "&external_reference=" + externalReference;            

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                
                var responseStream = await response.Content.ReadAsStreamAsync();
                
                var searchResponse = await SearchPaymentsResponseDto.ParseAsync(responseStream);
                searchResponse.RequestUri = response.RequestMessage.RequestUri.OriginalString;

                searchResponse.Success = response.IsSuccessStatusCode;

                return searchResponse;
            }

            
        }



    }
}
