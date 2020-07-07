using System.Net.Http;
using System.Threading.Tasks;
using Authin.Core.Api.Model;
using Newtonsoft.Json;

namespace Authin.Core.Api.Request
{
    public class JwksRequest : IExecutable<Jwks>
    {
        private JwksRequest()
        {
        }

        public static Builder GetBuilder()
        {
            return new Builder();
        }

        public class Builder
        {
            public JwksRequest Build()
            {
                return new JwksRequest();
            }
        }

        public async Task<Jwks> Execute()
        {
            var baseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
            var keysEndpoint = baseUrl + "/api/v1/keys";
            var httpClient = new HttpClient();
            var keysResponse = await httpClient.GetAsync(keysEndpoint);
            keysResponse.EnsureSuccessStatusCode();
            var response = await keysResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Jwks>(response);
        }
    }
}
