using HealthApp.API.IoC;
using HealthApp.API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthApp.API.Extensions
{
    public static class JwtExtension
    {
        public static string GetJwtToken(this User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IoCContainer.Configuration["JWT:SecretKey"])),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: IoCContainer.Configuration["JWT:Issuer"],
                audience: IoCContainer.Configuration["JWT:Audience"],
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(30));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
