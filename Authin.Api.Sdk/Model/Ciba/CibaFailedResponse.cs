using Newtonsoft.Json;

namespace Authin.Api.Sdk.Model.Ciba;

public class CibaFailedResponse : ICibaResponse
{
    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }
}