﻿using Newtonsoft.Json;

namespace Authin.Api.Sdk.Model;

public class TokenResponse
{
    [JsonProperty("scope")] public string Scope { get; set; }
    [JsonProperty("token_type")] public string TokenType { get; set; }
    [JsonProperty("expires_in")] public string ExpiresIn { get; set; }
    [JsonProperty("access_token")] public string AccessToken { get; set; }
    [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
    [JsonProperty("id_token")] public string IdToken { get; set; }
}