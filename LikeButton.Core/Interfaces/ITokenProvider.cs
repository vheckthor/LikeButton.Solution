using LikeButton.Core.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LikeButton.Core.Interfaces
{
    public interface ITokenProvider
    {
        public void Tokenizer(User logged, out JwtSecurityTokenHandler tokenHandler, out SecurityToken token);
        public string GeneratedToken(User logged);
    }
}
