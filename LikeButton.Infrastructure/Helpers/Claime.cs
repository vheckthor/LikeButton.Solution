using LikeButton.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;


namespace LikeButton.Infrastructure.Helpers
{
    public class Claime : IClaim
    {
        public string GetClaims(ClaimsPrincipal User)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return claim;
        }
    }
}
