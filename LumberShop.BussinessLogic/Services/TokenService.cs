using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LumberStoreSystem.BussinessLogic.Services
{
        public class TokenService
        {
            private readonly string _secret;

            public TokenService(string secret)
            {
                _secret = secret;
            }

            public string GenerateToken(string email, int userId)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secret);

                // Create a list of claims, adding userId and email
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
            new Claim("userId", userId.ToString()) // Adding the userId as a claim
        };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        }
    }
