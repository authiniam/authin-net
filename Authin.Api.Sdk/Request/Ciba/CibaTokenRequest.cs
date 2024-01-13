using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Authin.Api.Sdk.Model;
using Newtonsoft.Json;

namespace Authin.Api.Sdk.Request.Ciba;

public class CibaTokenRequest : IExecutable<TokenResponse>
{
    private CibaTokenRequest()
    {
    }

    public string BaseUrl { get; private set; }
    public string ClientId { get; private set; }
    public string ClientSecret { get; private set; }
    public string GrantType { get; private set; }
    public string AuthRequestId { get; private set; }

    public static Builder GetBuilder()
    {
        return new Builder();
    }

    public class Builder
    {
        private string _baseUrl;
        private string _clientId;
        private string _clientSecret;
        private string _authRequestId;

        public Builder SetBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
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

        public Builder SetAuthRequestId(string authRequestId)
        {
            _authRequestId = authRequestId;
            return this;
        }

        public CibaTokenRequest Build()
        {
            _baseUrl = _baseUrl ?? System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
            if (string.IsNullOrEmpty(_baseUrl))
                throw new ArgumentException("BaseUrl is a required field");

            if (string.IsNullOrEmpty(_clientId))
                throw new ArgumentException("ClientId is a required field");

            if (string.IsNullOrEmpty(_clientSecret))
                throw new ArgumentException("ClientSecret is a required field");

            if (string.IsNullOrEmpty(_authRequestId))
                throw new ArgumentException("AuthRequestId is a required field");

            return new CibaTokenRequest
            {
                BaseUrl = _baseUrl,
                ClientId = _clientId,
                ClientSecret = _clientSecret,
                GrantType = "urn:openid:params:grant-type:ciba",
                AuthRequestId = _authRequestId
            };
        }
    }

    public async Task<TokenResponse> Execute()
    {
        var tokenEndpoint = new Uri(new Uri(BaseUrl), "/api/v1/oauth/token");
        var httpClient = new HttpClient();

        var tokenRequestBody = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("client_id", ClientId),
            new KeyValuePair<string, string>("client_secret", ClientSecret),
            new KeyValuePair<string, string>("grant_type", GrantType),
            new KeyValuePair<string, string>("auth_request_id", AuthRequestId)
        };

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
            new KeyValuePair<string, string>("client_id", ClientId),
            new KeyValuePair<string, string>("client_secret", ClientSecret),
            new KeyValuePair<string, string>("grant_type", GrantType),
            new KeyValuePair<string, string>("auth_request_id", AuthRequestId)
        };

        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
        tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        tokenRequest.Content = new FormUrlEncodedContent(tokenRequestBody);
        var tokenResponse = httpClient.SendAsync(tokenRequest).Result;
        tokenResponse.EnsureSuccessStatusCode();
        var response = tokenResponse.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<TokenResponse>(response);
    }
}