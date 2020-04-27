using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tikisoft.UniversalPaymentGateway.TestPos.Services
{
    public class Auth0TokenService : ITokenService
    {
        private Auth0Token token = new Auth0Token();

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public Auth0TokenService()
        {
            ClientId=ConfigurationManager.AppSettings.Get("Auth0:ClientId");            
            ClientSecret = ConfigurationManager.AppSettings.Get("Auth0:ClientSecret");
        }

        public async Task<string> GetTokenAsync()
        {
            if (!this.token.IsValidAndNotExpiring)
            {
                this.token = await this.GetNewAccessToken();
            }
            return token.AccessToken;
        }

        private async Task<Auth0Token> GetNewAccessToken()
        {
            var token = new Auth0Token();
            var client = new HttpClient();
            var client_id = ClientId;
            var client_secret = ClientSecret;
            var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(clientCreds));

            var postMessage = new Dictionary<string, string>();
            postMessage.Add("grant_type", "client_credentials");
            postMessage.Add("audience","https://utgapi.com");
            //postMessage.Add("scope", "access_token");
            var request = new HttpRequestMessage(HttpMethod.Post, "https://tikisoft.auth0.com/oauth/token")
            {
                Content = new FormUrlEncodedContent(postMessage)
            };

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<Auth0Token>(json);
                token.ExpiresAt = DateTime.UtcNow.AddSeconds(this.token.ExpiresIn);
            }
            else
            {
                throw new ApplicationException("Unable to retrieve access token from Auth0");
            }

            return token;
        }

        private class Auth0Token
        {
            [JsonProperty(PropertyName = "access_token")]
            public string AccessToken { get; set; }

            [JsonProperty(PropertyName = "expires_in")]
            public int ExpiresIn { get; set; }

            public DateTime ExpiresAt { get; set; }

            public string Scope { get; set; }

            [JsonProperty(PropertyName = "token_type")]
            public string TokenType { get; set; }

            public bool IsValidAndNotExpiring
            {
                get
                {
                    return !String.IsNullOrEmpty(this.AccessToken) && 
                        this.ExpiresAt > DateTime.UtcNow.AddSeconds(30);
                }
            }
        }
    }


}
