using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Authin.Core.Api.Model;
using Newtonsoft.Json;

namespace Authin.Core.Api.Request
{
    public class TokenRequest : IExecutable<TokenResponse>
    {
        private TokenRequest()
        {
        }

        public string Code { get; protected set; }
        public string RedirectUri { get; private set; }
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }
        public string GrantType { get; private set; }

        public static Builder GetBuilder()
        {
            return new Builder();
        }

        public class Builder
        {
            private string _code;
            private string _redirectUri;
            private string _clientId;
            private string _clientSecret;
            private string _grantType;

            public Builder SetCode(string code)
            {
                _code = code;
                return this;
            }
            public Builder SetRedirectUri(string redirectUri)
            {
                _redirectUri = redirectUri;
                return this;
            }

            public Builder SetClientId(string clientId)
            {
                _clientId = clientId;
                return this;
            }

            public Builder SetClientSecret(string clientSecret)
            {
                _clientSecret = clientSecret;
                return this;
            }

            public Builder SetGrantType(string grantType)
            {
                _grantType = grantType;
                return this;
            }

            public TokenRequest Build()
            {
                if(string.IsNullOrEmpty(_code))
                    throw new ArgumentException("Code is a required field");

                if (string.IsNullOrEmpty(_redirectUri))
                    throw new ArgumentException("RedirectUri is a required field");

                if (string.IsNullOrEmpty(_clientId))
                    throw new ArgumentException("ClientId is a required field");

                if (string.IsNullOrEmpty(_clientSecret))
                    throw new ArgumentException("ClientSecret is a required field");

                if (string.IsNullOrEmpty(_grantType))
                    throw new ArgumentException("GrantType is a required field");

                return new TokenRequest
                {
                    Code = _code,
                    RedirectUri = _redirectUri,
                    ClientId = _clientId,
                    ClientSecret = _clientSecret,
                    GrantType = _grantType
                };
            }
        }

        public async Task<TokenResponse> Execute()
        {
            var baseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
            var tokenEndpoint = baseUrl + "/api/v1/oauth/token";
            var httpClient = new HttpClient();

            var tokenRequestBody = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("code", Code),
                new KeyValuePair<string, string>("redirect_uri", RedirectUri),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("grant_type", GrantType)
            };

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
            tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            tokenRequest.Content = new FormUrlEncodedContent(tokenRequestBody);
            var tokenResponse = await httpClient.SendAsync(tokenRequest);
            tokenResponse.EnsureSuccessStatusCode();
            var response = await tokenResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenResponse>(response);
        }
    }


}
