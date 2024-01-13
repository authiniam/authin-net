using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using Authin.Api.Sdk.Model;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Authin.Api.Sdk.Validation;

public static class TokenValidator
{
    private static readonly string[] EqualsPadding = GetEqualsPadding();

    private static string[] GetEqualsPadding()
    {
        return Enumerable.Range(0, 4)
            .Select(numberOfEqualSigns => new string('=', numberOfEqualSigns))
            .ToArray();
    }

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
        var paddedBase64Url = PadBase64(base64Url);
        return Convert.FromBase64String(paddedBase64Url
                .Replace("_", "/")
                .Replace("-", "+"));
    }

    private static string PadBase64(string base64)
    {
        if(base64.Length % 4 == 0)
        {
            return base64;
        }
        
        var numberOfEqualsSigns = 4 - base64.Length % 4;
        return string.Concat(base64, EqualsPadding[numberOfEqualsSigns]);
    }
}