using LikeButton.Core.Entities;
using LikeButton.Core.Interfaces;
using LikeButton.Core.Interfaces.IUserRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeButton.Infrastructure.Data.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IFileLogger _logger;

        public UserRepository(AppDbContext context, IFileLogger logger)
        {
            _context = context ?? throw new ArgumentNullException("appDbContext", "Null DBContext injection");
            _logger = logger ?? throw new ArgumentNullException("fileLogger", "Null FileLogger injection");
    
        }
        public async Task<User> GetUserAsync(Guid userUniqueId)
        {
            try
            {
                var user = await _context.Users.Include(o=> o.ArticleLiked).FirstOrDefaultAsync(x => x.UserUniqueIdentity == userUniqueId);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException.Message, " UserRepositoryGetUserAsync");
                return default;

            }
        }
    }
}
