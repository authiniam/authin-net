using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Authin.Core.Api.Request
{
    public class AuthorizationCodeRequest : IExecutable<Uri>
    {
        private AuthorizationCodeRequest()
        {
        }

        public string ClientId { get; private set; }
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
            private string _clientId;
            private string _responseType;
            private string _redirectUri;
            private readonly IList<string> _scopes = new List<string>();
            private string _state;
            private readonly IList<string> _userInfoClaims = new List<string>();
            private readonly IList<string> _idTokenClaims = new List<string>();

            public Builder SetClientId(string clientId)
            {
                _clientId = clientId;
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
                ((List<string>)_scopes).AddRange(scopes);
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
                ((List<string>)_userInfoClaims).AddRange(claims);
                return this;
            }

            public Builder AddIdTokenClaim(string claim)
            {
                _idTokenClaims.Add(claim);
                return this;
            }

            public Builder AddIdTokenClaims(IList<string> claims)
            {
                ((List<string>)_idTokenClaims).AddRange(claims);
                return this;
            }

            public AuthorizationCodeRequest Build()
            {
                if (string.IsNullOrEmpty(_clientId))
                    throw new ArgumentException("ClientId is a required field");

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
                ((List<string>)_userInfoClaims).ForEach(c => claims.UserInfo.Add(c, null));
                ((List<string>)_idTokenClaims).ForEach(c => claims.IdToken.Add(c, null));

                return new AuthorizationCodeRequest
                {
                    ClientId = _clientId,
                    RedirectUri = _redirectUri,
                    ResponseType = _responseType,
                    Scopes= _scopes,
                    State = _state,
                    Claims = claims
                };
            }
        }

        public async Task<Uri> Execute()
        {
            var baseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
            var authorizationEndpoint = baseUrl + "/openidauthorize";

            var scope = string.Join(" ", Scopes);
            var authorizationEndpointRedirectUri =
                authorizationEndpoint +
                "?client_id=" + Uri.EscapeDataString(ClientId) +
                "&response_type=" + Uri.EscapeDataString(ResponseType) +
                "&redirect_uri=" + Uri.EscapeDataString(RedirectUri) +
                "&scope=" + Uri.EscapeDataString(scope) +
                "&state=" + Uri.EscapeDataString(State) +
                "&claims=" + Uri.EscapeDataString(JsonConvert.SerializeObject(Claims));

            return new Uri(authorizationEndpointRedirectUri);

        }

        public class ClaimsModel
        {
            [JsonProperty("user_info")] public Dictionary<string, string> UserInfo { get; set; }
            [JsonProperty("id_token")] public Dictionary<string, string> IdToken { get; set; }
        }
    }
}