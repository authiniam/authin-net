using System.Collections.Generic;
using Newtonsoft.Json;

namespace Authin.Api.Sdk.Model
{
    public class Jwks
    {
        [JsonProperty("keys")]
        public List<RsaJwk> Keys { get; set; }
    }
}
