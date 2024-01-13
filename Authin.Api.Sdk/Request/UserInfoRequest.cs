using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Authin.Api.Sdk.Model;
using Newtonsoft.Json;

namespace Authin.Api.Sdk.Request;

public class UserInfoRequest : IExecutable<UserInfoResponse>
{
    private UserInfoRequest()
    {
    }

    public string BaseUrl { get; private set; }
    public string AccessToken { get; private set; }
    public Method Method { get; private set; }

    public static Builder GetBuilder()
    {
        return new Builder();
    }

    public class Builder
    {
        private string _baseUrl;
        private string _accessToken;
        private Method _method;

        public Builder SetBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
            return this;
        }

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
            _baseUrl = _baseUrl ?? System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
            if (string.IsNullOrEmpty(_baseUrl))
                throw new ArgumentException("BaseUrl is a required field");

            if (string.IsNullOrEmpty(_accessToken))
                throw new ArgumentException("AccessToken is a required field");

            return new UserInfoRequest
            {
                BaseUrl = _baseUrl,
                AccessToken = _accessToken,
                Method = _method
            };
        }

    }

    public async Task<UserInfoResponse> Execute()
    {
        var userinfoEndpoint = new Uri(new Uri(BaseUrl), "/api/v1/oauth/userinfo");
        var httpClient = new HttpClient();
        var userinfoRequest = new HttpRequestMessage();

        switch (Method)
        {
            case Method.Get:
                userinfoRequest.Method = HttpMethod.Get;
                break;
            case Method.Post:
                userinfoRequest.Method = HttpMethod.Post;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        userinfoRequest.RequestUri = userinfoEndpoint;
        userinfoRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        userinfoRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        var userinfoResponse = await httpClient.SendAsync(userinfoRequest);
        userinfoResponse.EnsureSuccessStatusCode();
        var response = await userinfoResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<UserInfoResponse>(response);
    }

    public UserInfoResponse ExecuteSync()
    {
        var userinfoEndpoint = new Uri(new Uri(BaseUrl), "/api/v1/oauth/userinfo");
        var httpClient = new HttpClient();
        var userinfoRequest = new HttpRequestMessage();

        switch (Method)
        {
            case Method.Get:
                userinfoRequest.Method = HttpMethod.Get;
                break;
            case Method.Post:
                userinfoRequest.Method = HttpMethod.Post;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        userinfoRequest.RequestUri = userinfoEndpoint;
        userinfoRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        userinfoRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        var userinfoResponse = httpClient.SendAsync(userinfoRequest).Result;
        userinfoResponse.EnsureSuccessStatusCode();
        var response = userinfoResponse.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<UserInfoResponse>(response);
    }
}

public enum Method
{
    Get,
    Post
}