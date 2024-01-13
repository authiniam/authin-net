using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using Authin.Api.Sdk.Model;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Authin.Api.Sdk.Validation;

public class TokenValidator
{
    public static JObject Validate(string token, Jwks jwks, string issuer, string audience)
    {
        var cryptoServiceProvider = new RSACryptoServiceProvider();
        cryptoServiceProvider.ImportParameters(new RSAParameters()
        {
            Modulus = FromBase64Url(jwks.Keys[0].Modulus),
            Exponent = FromBase64Url(jwks.Keys[0].Exponent)
        });

        var validationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new RsaSecurityKey(cryptoServiceProvider)
        };

        var handler = new JwtSecurityTokenHandler();
        handler.ValidateToken(token, validationParameters, out var validatedSecurityToken);
        var validatedJwt = validatedSecurityToken as JwtSecurityToken;
        var claims = new JObject();
        validatedJwt?.Claims.ToList().ForEach(c => claims.Add(c.Type, c.Value));

        return claims;
    }

    private static byte[] FromBase64Url(string base64Url)
    {
        return Convert
            .FromBase64String((base64Url.Length % 4 == 0 ? base64Url : string.Concat(base64Url, "====".AsSpan(base64Url.Length % 4)))
                .Replace("_", "/")
                .Replace("-", "+"));
    }
}