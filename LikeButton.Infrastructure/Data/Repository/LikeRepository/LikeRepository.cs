using LikeButton.Core.Constants;
using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Entities;
using LikeButton.Core.Interfaces;
using LikeButton.Core.Interfaces.ILikeRepository;
using LikeButton.Infrastructure.Data.Repository.ArticleRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeButton.Infrastructure.Data.Repository.LikeRepository
{
    public class LikeRepository : ILikeRepository
    {

        private readonly AppDbContext _context;
        private readonly IFileLogger _logger;
        private readonly IMemoryCache _cache;
        private readonly IArticleRepository _articleRepo;

        public LikeRepository(AppDbContext context, IFileLogger logger, IMemoryCache cache,IArticleRepository articleRepo)
        {
            _context = context ?? throw new ArgumentNullException("appDbContext", "Null DBContext injection");
            _logger = logger?? throw new ArgumentNullException("fileLogger", "Null FileLogger injection");
            _cache = cache;
            _articleRepo = articleRepo;
        }
        public async Task<bool> AddAsync(Like like)
        {

            try
            {

                var getLike =await  GetLikeAsync(like.LikerUniqueId, like.ArticleUniqueId);
                if (getLike != null)
                {
                    getLike.LikeStatus = like.LikeStatus;

                }
                else
                {
                    _context.Likes.Add(like);
                }
                var response = await _context.SaveChangesAsync() > 0;
                if (response) 
                {
                    var newList = new List<GetArticleResponse>();
                    var updateCache =await _cache.Set(AppConstants.CACHEKEYARTICLE, _articleRepo.GetArticlesFromDbAsync());
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException.Message, " LikeRepositoryAddAsync");
                return false;
            }


            
        }

        public async Task<int> GetArticleLikesCountAsync(Guid articleUniqueId)
        {
            try
            {  
                return  await _context.Likes.Select(x => x.ArticleUniqueId == articleUniqueId && x.LikeStatus == true).CountAsync();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException.Message, " LikeRepositoryGetArticleLikesCountAsync");

                return default;
            }

        }

        public async Task<Like> GetLikeAsync(Guid userUniqueId, Guid articleUniqueId)
        {

            try
            {
                return await _context.Likes.SingleOrDefaultAsync(x => x.ArticleUniqueId == articleUniqueId
                && x.LikerUniqueId == userUniqueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException.Message, " LikeRepositoryGetLikeAsync");
                return default;
            }

        }
    }
}
