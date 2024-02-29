using System.Security.Cryptography;

namespace Authin.Api.Sdk.Common;

public class CodeVerifier
{
    public static string NewCodeVerifier()
    {
#if NET481 || NET48
        using var rng = new RNGCryptoServiceProvider();
        var tokenData = new byte[128];
        rng.GetBytes(tokenData);
        return tokenData.ToBase64UrlSafeNoPadding();
#else
        var tokenData = RandomNumberGenerator.GetBytes(128);
        return tokenData.ToBase64UrlSafeNoPadding();
#endif
    }
}
