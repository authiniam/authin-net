using System.Security.Cryptography;

namespace Authin.Api.Sdk.Common;

public class CodeVerifier
{
    public static string NewCodeVerifier()
    {
        var tokenData = RandomNumberGenerator.GetBytes(128);
        return tokenData.ToBase64UrlSafeNoPadding();
    }
}