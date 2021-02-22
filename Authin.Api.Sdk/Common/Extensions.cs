using System;
using System.Security.Cryptography;
using System.Text;

namespace Authin.Api.Sdk.Common
{
    public static class Extensions
    {
        public static byte[] ToSha256(this string value)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

        public static string ToBase64UrlSafeNoPadding(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
}
