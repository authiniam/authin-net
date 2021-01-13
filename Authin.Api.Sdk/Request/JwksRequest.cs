using System;
using System.Net.Http;
using System.Threading.Tasks;
using Authin.Api.Sdk.Model;
using Newtonsoft.Json;

namespace Authin.Api.Sdk.Request
{
    public class JwksRequest : IExecutable<Jwks>
    {
        public string BaseUrl { get; private set; }

        private JwksRequest()
        {
        }

        public static Builder GetBuilder()
        {
            return new Builder();
        }

        public class Builder
        {
            private string _baseUrl;

            public Builder SetBaseUrl(string baseUrl)
            {
                _baseUrl = baseUrl;
                return this;
            }

            public JwksRequest Build()
            {
                _baseUrl = _baseUrl ?? System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
                if (string.IsNullOrEmpty(_baseUrl))
                    throw new ArgumentException("BaseUrl is a required field");

                return new JwksRequest
                {
                    BaseUrl = _baseUrl
                };
            }
        }

        public async Task<Jwks> Execute()
        {
            var keysEndpoint = new Uri(new Uri(BaseUrl), "/api/v1/keys");
            var httpClient = new HttpClient();
            var keysResponse = await httpClient.GetAsync(keysEndpoint);
            keysResponse.EnsureSuccessStatusCode();
            var response = await keysResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Jwks>(response);
        }

        public Jwks ExecuteSync()
        {
            var keysEndpoint = new Uri(new Uri(BaseUrl), "/api/v1/keys");
            var httpClient = new HttpClient();
            var keysResponse = httpClient.GetAsync(keysEndpoint).Result;
            keysResponse.EnsureSuccessStatusCode();
            var response = keysResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Jwks>(response);
        }
    }
}
