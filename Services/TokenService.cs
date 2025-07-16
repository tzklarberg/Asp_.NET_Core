using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
// using Microsoft.Extensions.Hosting;

namespace CoreProject.Services;

public static class TokenService
{
    private static SymmetricSecurityKey key
        = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                "SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));
    private static string issuer = "http://localhost:5187";
    public static SecurityToken GetToken(List<Claim> claims) =>
        new JwtSecurityToken(
            issuer,
            issuer,
            claims,
            expires: DateTime.Now.AddDays(31.0),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

    public static string WriteToken(SecurityToken token) =>
        new JwtSecurityTokenHandler().WriteToken(token);


    public static TokenValidationParameters
    GetTokenValidationParameters() =>
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = key,
            LifetimeValidator = LifetimeValidator,
            ClockSkew = TimeSpan.Zero 
        };


    // internal static TokenValidationParameters ValidateToken()
    // {
    //     throw new NotImplementedException();
    // }
    public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters validationParameters)
    {
        // // קביעת התוקף של הטוקן ל-30 ימים
        // var expirationDate = notBefore?.AddMinutes(30);
        // // בדיקה האם התוקף המוגדר הוא קטן מהתאריך הנוכחי
        // if (expirationDate < DateTime.UtcNow)
        // {
        //     // התוקף פג, מסר תוקף לא חוקי
        //     return false;
        // }

        // // התוקף עדיין תקף
        // return true;
        if (expires != null)
        {
            return expires > DateTime.UtcNow;
        }
        return false;
        }
}
