using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Authin.Api.Sdk.Common;
using Newtonsoft.Json;

namespace Authin.Api.Sdk.Request.Pkce;

public class PkceAuthorizationCodeRequest : IExecutable<Uri>
{
    private PkceAuthorizationCodeRequest()
    {
    }

    public string BaseUrl { get; private set; }
    public string ClientId { get; private set; }
    public string CodeChallenge { get; private set; }
    public string CodeChallengeMethod { get; private set; }
    public string ResponseType { get; protected set; }
    public string RedirectUri { get; private set; }
    public IList<string> Scopes { get; private set; }
    public string State { get; private set; }
    public ClaimsModel Claims { get; private set; }

    public static Builder GetBuilder()
    {
        return new Builder();
    }

    public class Builder
    {
        private string _baseUrl;
        private string _clientId;
        private string _codeVerifier;
        private string _responseType;
        private string _redirectUri;
        private readonly IList<string> _scopes = new List<string>();
        private string _state;
        private readonly IList<string> _userInfoClaims = new List<string>();
        private readonly IList<string> _idTokenClaims = new List<string>();

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

        public Builder SetCodeVerifier(string codeVerifier)
        {
            _codeVerifier = codeVerifier;
            return this;
        }

        public Builder SetResponseType(string responseType)
        {
            _responseType = responseType;
            return this;
        }

        public Builder SetRedirectUri(string redirectUri)
        {
            _redirectUri = redirectUri;
            return this;
        }

        public Builder AddScopes(IList<string> scopes)
        {
            ((List<string>) _scopes).AddRange(scopes);
            return this;
        }

        public Builder SetState(string state)
        {
            _state = state;
            return this;
        }

        public Builder AddUserInfoClaim(string claim)
        {
            _userInfoClaims.Add(claim);
            return this;
        }

        public Builder AddUserInfoClaims(IList<string> claims)
        {
            ((List<string>) _userInfoClaims).AddRange(claims);
            return this;
        }

        public Builder AddIdTokenClaim(string claim)
        {
            _idTokenClaims.Add(claim);
            return this;
        }

        public Builder AddIdTokenClaims(IList<string> claims)
        {
            ((List<string>) _idTokenClaims).AddRange(claims);
            return this;
        }

        public PkceAuthorizationCodeRequest Build()
        {
            _baseUrl = _baseUrl ?? ConfigurationManager.AppSettings["BaseUrl"];
            if (string.IsNullOrEmpty(_baseUrl))
                throw new ArgumentException("BaseUrl is a required field");

            if (string.IsNullOrEmpty(_clientId))
                throw new ArgumentException("ClientId is a required field");

            if (string.IsNullOrEmpty((_codeVerifier)))
                throw new ArgumentException("CodeVerifier is a required field");

            if (string.IsNullOrEmpty(_responseType))
                throw new ArgumentException("ResponseType is a required field");

            if (string.IsNullOrEmpty(_redirectUri))
                throw new ArgumentException("RedirectUri is a required field");

            if (_scopes == null)
                throw new ArgumentException("Scope is a required field");


            var claims = new ClaimsModel
            {
                UserInfo = new Dictionary<string, string>(),
                IdToken = new Dictionary<string, string>()
            };
            ((List<string>) _userInfoClaims).ForEach(c => claims.UserInfo.Add(c, null));
            ((List<string>) _idTokenClaims).ForEach(c => claims.IdToken.Add(c, null));

            return new PkceAuthorizationCodeRequest
            {
                BaseUrl = _baseUrl,
                ClientId = _clientId,
                CodeChallenge = _codeVerifier.ToSha256().ToBase64UrlSafeNoPadding(),
                CodeChallengeMethod = "S256",
                RedirectUri = _redirectUri,
                ResponseType = _responseType,
                Scopes = _scopes,
                State = _state,
                Claims = claims
            };
        }
    }

    public Task<Uri> Execute()
    {
        var authorizationEndpoint = new Uri(new Uri(BaseUrl), "/openidauthorize").ToString();

        var scope = string.Join(" ", Scopes);
        var authorizationEndpointRedirectUri =
            authorizationEndpoint +
            "?client_id=" + Uri.EscapeDataString(ClientId) +
            "&code_challenge=" + Uri.EscapeDataString(CodeChallenge) +
            "&code_challenge_method=" + Uri.EscapeDataString(CodeChallengeMethod) +
            "&response_type=" + Uri.EscapeDataString(ResponseType) +
            "&redirect_uri=" + Uri.EscapeDataString(RedirectUri) +
            "&scope=" + Uri.EscapeDataString(scope) +
            "&state=" + Uri.EscapeDataString(State) +
            "&claims=" + Uri.EscapeDataString(JsonConvert.SerializeObject(Claims));

        return Task.FromResult(new Uri(authorizationEndpointRedirectUri));
    }

    public Uri ExecuteSync()
    {
        return Execute().Result;
    }

    public class ClaimsModel
    {
        [JsonProperty("userinfo")] public Dictionary<string, string> UserInfo { get; set; }
        [JsonProperty("id_token")] public Dictionary<string, string> IdToken { get; set; }
    }
}