using Newtonsoft.Json;

namespace Authin.Api.Sdk.Model.Ciba;

public class CibaSuccessfulResponse : ICibaResponse
{
    [JsonProperty("auth_request_id")]
    public string AuthRequestId { get; set; }

    [JsonProperty("expiresIn")]
    public int ExpiresIn { get; set; }

    [JsonProperty("interval")]
    public int Interval{ get; set; }
}