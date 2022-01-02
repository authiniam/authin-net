using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Authin.Api.Sdk.Model;
using Newtonsoft.Json;

namespace Authin.Api.Sdk.Request
{
    public class RefreshTokenRequest : IExecutable<TokenResponse>
    {
        private RefreshTokenRequest()
        {
        }

        public string BaseUrl { get; private set; }
        public string AccessToken { get; private set; }
        public string GrantType { get; private set; }
        public string RefreshToken { get; private set; }
        public List<string> Scopes { get; private set; }
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }

        public static Builder GetBuilder()
        {
            return new Builder();
        }

        public class Builder
        {
            private string _baseUrl;
            private string _grantType;
            private string _accessToken;
            private string _refreshToken;
            private List<string> _scopes;
            private string _clientId;
            private string _clientSecret;

            public Builder SetBaseUrl(string baseUrl)
            {
                _baseUrl = baseUrl;
                return this;
            }

            public Builder SetGrantType(string grantType)
            {
                _grantType = grantType;
                return this;
            }

            public Builder SetRefreshToken(string refreshToken)
            {
                _refreshToken = refreshToken;
                return this;
            }

            public Builder SetScopes(List<string> scopes)
            {
                _scopes = scopes;
                return this;
            }

            public Builder SetClientId(string clientId)
            {
                _clientId= clientId;
                return this;
            }

            public Builder SetClientSecret(string clientSecret)
            {
                _clientSecret = clientSecret;
                return this;
            }

            public RefreshTokenRequest Build()
            {
                _baseUrl = _baseUrl ?? System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
                if (string.IsNullOrEmpty(_baseUrl))
                    throw new ArgumentException("BaseUrl is a required field");

                if (string.IsNullOrEmpty(_grantType))
                    throw new ArgumentException("GrantType is a required field");

                if (string.IsNullOrEmpty(_refreshToken))
                    throw new ArgumentException("RefreshToken is a required field");

                if (string.IsNullOrEmpty(_clientId))
                    throw new ArgumentException("ClientId is a required field");

                if (string.IsNullOrEmpty(_clientSecret))
                    throw new ArgumentException("ClientSecret is a required field");

                return new RefreshTokenRequest
                {
                    BaseUrl = _baseUrl,
                    GrantType = _grantType,
                    RefreshToken = _refreshToken,
                    Scopes = _scopes,
                    ClientId = _clientId,
                    ClientSecret = _clientSecret,
                };
            }
        }

        public async Task<TokenResponse> Execute()
        {
            var tokenEndpoint = new Uri(new Uri(BaseUrl), "/api/v1/oauth/token");
            var httpClient = new HttpClient();


            var tokenRequestBody = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", GrantType),
                new KeyValuePair<string, string>("refresh_token", RefreshToken),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
            };

            if (Scopes != null)
                tokenRequestBody.Add(new KeyValuePair<string, string>("scope", string.Join(" ", Scopes)));

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
            tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            tokenRequest.Content = new FormUrlEncodedContent(tokenRequestBody);
            var tokenResponse = await httpClient.SendAsync(tokenRequest);
            tokenResponse.EnsureSuccessStatusCode();
            var response = await tokenResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenResponse>(response);
        }

        public TokenResponse ExecuteSync()
        {
            var tokenEndpoint = new Uri(new Uri(BaseUrl), "/api/v1/oauth/token");
            var httpClient = new HttpClient();

            var tokenRequestBody = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", GrantType),
                new KeyValuePair<string, string>("refresh_token", RefreshToken),
                new KeyValuePair<string, string>("scope", string.Join(" ", Scopes)),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
            };

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
            tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            tokenRequest.Content = new FormUrlEncodedContent(tokenRequestBody);
            var tokenResponse = httpClient.SendAsync(tokenRequest).Result;
            tokenResponse.EnsureSuccessStatusCode();
            var response = tokenResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<TokenResponse>(response);
        }
    }


}
