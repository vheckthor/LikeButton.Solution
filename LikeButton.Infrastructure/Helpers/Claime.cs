using LikeButton.Core.Interfaces;
using System.Security.Claims;


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
