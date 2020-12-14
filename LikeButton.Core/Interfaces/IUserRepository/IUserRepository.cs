using LikeButton.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeButton.Core.Interfaces.IUserRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(Guid userUniqueId);
    }
}
