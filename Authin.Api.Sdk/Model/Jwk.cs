using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authin.Api.Sdk.Model
{
    public class Jwk
    {
        [JsonProperty("kty")] public KeyType KeyType { get; set; }
        [JsonProperty("use")] public PublicKeyUse PublicKeyUse { get; set; }
        [JsonProperty("key_ops")] public string KeyOperations { get; set; }
        [JsonProperty("alg")] public string Algorithm { get; set; }
        [JsonProperty("kid")] public string KeyId { get; set; }
        [JsonProperty("x5u")] public string X509Url { get; set; }
        [JsonProperty("x5c")] public string X509CertificateChain { get; set; }
        [JsonProperty("x5t")] public string X509CertificateSha1Thumbprint { get; set; }
        [JsonProperty("x5t#s256")] public string X509CertificateSha256Thumbprint { get; set; }
    }

    public enum KeyType
    {
        [EnumMember(Value = "RSA")] Rsa,
        [EnumMember(Value = "EC")] Ec
    }

    public enum PublicKeyUse
    {
        [EnumMember(Value = "sig")] Signature,
        [EnumMember(Value = "enc")] Encryption
    }

    public enum KeyOperations
    {
        [EnumMember(Value = "sign")] Sign,
        [EnumMember(Value = "verify")] Verify,
        [EnumMember(Value = "encrypt")] Encrypt,
        [EnumMember(Value = "decrypt")] Decrypt,
        [EnumMember(Value = "wrapkey")] WrapKey,
        [EnumMember(Value = "unwrapkey")] UnwrapKey,
        [EnumMember(Value = "derivekey")] DeriveKey,
        [EnumMember(Value = "derivebits")] DeriveBits
    }

    public enum Jwa
    {
        [EnumMember(Value = "HS256")] Hs256,
        [EnumMember(Value = "RS256")] Rs256,
        [EnumMember(Value = "ES256")] Es256
    }
}