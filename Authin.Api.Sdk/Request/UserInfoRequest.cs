using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Authin.Core.Api.Model;
using Newtonsoft.Json;

namespace Authin.Core.Api.Request
{
    public class UserInfoRequest : IExecutable<UserInfoResponse>
    {
        private UserInfoRequest()
        {
        }

        public string AccessToken { get; private set; }
        public Method Method { get; private set; }

        public static Builder GetBuilder()
        {
            return new Builder();
        }

        public class Builder
        {
            private string _accessToken;
            private Method _method;

            public Builder SetAccessToken(string accessToken)
            {
                _accessToken = accessToken;
                return this;
            }

            public Builder SetMethod(Method method)
            {
                _method = method;
                return this;
            }

            public UserInfoRequest Build()
            {
                if (string.IsNullOrEmpty(_accessToken))
                    throw new ArgumentException("AccessToken is a required field");

                return new UserInfoRequest
                {
                    AccessToken = _accessToken,
                    Method = _method
                };
            }

        }

        public async Task<UserInfoResponse> Execute()
        {
            var baseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
            var tokenEndpoint = baseUrl + "/api/v1/oauth/userinfo";
            var httpClient = new HttpClient();
            var tokenRequest = new HttpRequestMessage();

            switch (Method)
            {
                case Method.Get:
                    tokenRequest.Method = HttpMethod.Get;
                    break;
                case Method.Post:
                    tokenRequest.Method = HttpMethod.Post;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            tokenRequest.RequestUri = new Uri(tokenEndpoint);
            tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var tokenResponse = await httpClient.SendAsync(tokenRequest);
            tokenResponse.EnsureSuccessStatusCode();
            var response = await tokenResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserInfoResponse>(response);
        }
    }

    public enum Method
    {
        Get,
        Post
    }
}
