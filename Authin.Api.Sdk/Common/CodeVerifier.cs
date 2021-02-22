using System;
using System.Security.Cryptography;

namespace Authin.Api.Sdk.Common
{
    public class CodeVerifier
    {
        public static string NewCodeVerifier()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[128];
                rng.GetBytes(tokenData);
                return tokenData.ToBase64UrlSafeNoPadding();
            }
        }
    }
}
