using Newtonsoft.Json;

namespace Authin.Api.Sdk.Model;

public class Jwk
{
    [JsonProperty("kty")] public string KeyType { get; set; }

    [JsonProperty("use")] public string PublicKeyUse { get; set; }

    [JsonProperty("key_ops")] public string KeyOperations { get; set; }

    [JsonProperty("alg")] public string Algorithm { get; set; }

    [JsonProperty("kid")] public string KeyId { get; set; }

    [JsonProperty("x5u")] public string X509Url { get; set; }

    [JsonProperty("x5c")] public string X509CertificateChain { get; set; }

    [JsonProperty("x5t")] public string X509CertificateSha1Thumbprint { get; set; }

    [JsonProperty("x5t#s256")] public string X509CertificateSha256Thumbprint { get; set; }
}