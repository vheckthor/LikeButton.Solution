using LikeButton.Core.Entities;
using LikeButton.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LikeButton.Infrastructure.JwtToken
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IConfiguration _config;
        public TokenProvider(IConfiguration config)
        {
            _config = config;

        }

        public string GeneratedToken(User logged)
        {
            JwtSecurityTokenHandler tokenHandler;
            SecurityToken token;
            Tokenizer(logged, out tokenHandler, out token);
            var jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }

        public void Tokenizer(User logged, out JwtSecurityTokenHandler tokenHandler, out SecurityToken token)
        {
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,logged.UserUniqueIdentity.ToString()),
                new Claim(ClaimTypes.Name,logged.Username)

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            tokenHandler = new JwtSecurityTokenHandler();
            token = tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
