using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace LikeButton.Core.Interfaces
{
    public interface IClaim
    {
        public string GetClaims(ClaimsPrincipal User);
    }
}
