using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Authin.Api.Sdk.Model.Ciba;
using Newtonsoft.Json;

namespace Authin.Api.Sdk.Request.Ciba
{
    public class CibaRequest : IExecutable<ICibaResponse>
    {
        #region Constructor
        private CibaRequest() { }
        #endregion

        #region Properties
        public string BaseUrl { get; private set; }
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }
        public string GrantType { get; private set; }
        public string Scope { get; private set; }
        public string ClientNotificationToken { get; private set; }
        public string AcrValues { get; private set; }
        public string LoginHint { get; private set; }
        public string BindingMessage { get; private set; }
        #endregion

        #region Public Methods
        public static Builder GetBuilder()
        {
            return new Builder();
        }

        public async Task<ICibaResponse> Execute()
        {
            var cibaEndpoint = new Uri(new Uri(BaseUrl), "/api/v1/oauth/bc-authorize");
            var httpClient = new HttpClient();

            var cibaRequestBody = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("grant_type", GrantType),
                new KeyValuePair<string, string>("scope", Scope),
                new KeyValuePair<string, string>("client_notification_token", ClientNotificationToken),
                new KeyValuePair<string, string>("acr_values", AcrValues),
                new KeyValuePair<string, string>("login_hint", LoginHint),
                new KeyValuePair<string, string>("binding_message", BindingMessage)
            };

            var cibaRequest = new HttpRequestMessage(HttpMethod.Post, cibaEndpoint);
            cibaRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            cibaRequest.Content = new FormUrlEncodedContent(cibaRequestBody);
            var cibaResponse = await httpClient.SendAsync(cibaRequest);
            var response = await cibaResponse.Content.ReadAsStringAsync();

            if (cibaResponse.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<CibaSuccessfulResponse>(response);
            else
                return JsonConvert.DeserializeObject<CibaFailedResponse>(response);
        }

        public ICibaResponse ExecuteSync()
        {
            var cibaEndpoint = new Uri(new Uri(BaseUrl), "/api/v1/oauth/bc-authorize");
            var httpClient = new HttpClient();

            var cibaRequestBody = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("grant_type", GrantType),
                new KeyValuePair<string, string>("scope", Scope),
                new KeyValuePair<string, string>("client_notification_token", ClientNotificationToken),
                new KeyValuePair<string, string>("acr_values", AcrValues),
                new KeyValuePair<string, string>("login_hint", LoginHint),
                new KeyValuePair<string, string>("binding_message", BindingMessage)
            };

            var cibaRequest = new HttpRequestMessage(HttpMethod.Post, cibaEndpoint);
            cibaRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            cibaRequest.Content = new FormUrlEncodedContent(cibaRequestBody);
            var cibaResponse = httpClient.SendAsync(cibaRequest).Result;
            var response = cibaResponse.Content.ReadAsStringAsync().Result;

            if (cibaResponse.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<CibaSuccessfulResponse>(response);
            else
                return JsonConvert.DeserializeObject<CibaFailedResponse>(response);
        }
        #endregion

        #region Enums
        public enum Mode
        {
            Authentication,
            Transaction
        }
        #endregion

        #region Builder
        public class Builder
        {
            private string _baseUrl;
            private string _clientId;
            private string _clientSecret;
            private Mode? _mode;
            private readonly IList<string> _scopes = new List<string>();
            private string _clientNotificationToken;
            private readonly IList<string> _acrValues = new List<string>();
            private string _loginHint;
            private string _bindingMessage;

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

            public Builder SetMode(Mode mode)
            {
                _mode = mode;
                return this;
            }

            public Builder AddScope(string scope)
            {
                ((List<string>)_scopes).Add(scope);
                return this;
            }

            public Builder SetClientNotificationToken(string clientNotificationToken)
            {
                _clientNotificationToken = clientNotificationToken;
                return this;
            }

            public Builder AddAcrValue(string acr)
            {
                ((List<string>)_acrValues).Add(acr);
                return this;
            }

            public Builder SetLoginHint(string loginHint)
            {
                _loginHint = loginHint;
                return this;
            }

            public Builder SetBindingMessage(string bindingMessage)
            {
                _bindingMessage = bindingMessage;
                return this;
            }

            public CibaRequest Build()
            {
                _baseUrl = _baseUrl ?? System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
                if (string.IsNullOrEmpty(_baseUrl))
                    throw new ArgumentException("BaseUrl is a required field");

                if (string.IsNullOrEmpty(_clientId))
                    throw new ArgumentException("ClientId is a required field");

                if (string.IsNullOrEmpty(_clientSecret))
                    throw new ArgumentException("ClientSecret is a required field");

                if (string.IsNullOrEmpty(_clientNotificationToken))
                    throw new ArgumentException("ClientNotificationToken is a required field");

                if (string.IsNullOrEmpty(_loginHint))
                    throw new ArgumentException("LoginHint is a required field");

                if (_mode == null)
                    throw new ArgumentException("Mode is a required field");

                if (!_scopes.Contains("openid"))
                    _scopes.Add("openid");

                if (_mode == Mode.Transaction)
                    _scopes.Add("authin_trx");

                return new CibaRequest
                {
                    BaseUrl = _baseUrl,
                    ClientId = _clientId,
                    ClientSecret = _clientSecret,
                    GrantType = "urn:openid:params:grant-type:ciba",
                    Scope = string.Join(" ", _scopes),
                    ClientNotificationToken = _clientNotificationToken,
                    AcrValues = string.Join(" ", _acrValues),
                    LoginHint = _loginHint,
                    BindingMessage = _bindingMessage
                };

            }

            
        }
        #endregion
    }
}
