using Newtonsoft.Json;

namespace Authin.Core.Api.Model
{
    public class RsaJwk : Jwk
    {
        [JsonProperty("n")] public string Modulus { get; set; }
        [JsonProperty("e")] public string Exponent { get; set; }
    }
}