using LikeButton.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeButton.Core.Interfaces.ILikeRepository
{
    public interface ILikeRepository
    {
        Task<bool> AddAsync(Like like);
        Task<Like> GetLikeAsync(Guid userUniqueId, Guid articleUniqueId);
        Task<int> GetArticleLikesCountAsync(Guid articleUniqueId);
    }
}
