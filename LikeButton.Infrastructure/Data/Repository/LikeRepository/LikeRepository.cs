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
                var updatedLike = await CachedLikeData();
                var newLike = await _context.Likes.SingleOrDefaultAsync(x => x.LikerUniqueId == like.LikerUniqueId &&
                x.ArticleUniqueId == like.ArticleUniqueId);
               

                var currentIndex = updatedLike.IndexOf(getLike);
               

                if (getLike != null)
                {
                    newLike.LikeStatus = like.LikeStatus;
                    var check1 = _context.Entry(getLike).State;

                    updatedLike[currentIndex].LikeStatus = like.LikeStatus;
                    _cache.Set(AppConstants.CACHEKEYLIKE, updatedLike);
                    var check = _context.Entry(getLike).State;

                   
                }
                else
                {
                    var res = _context.Likes.Add(like);
                    

                }
                var response = await _context.SaveChangesAsync() > 0;
                if (response) 
                {
                    updatedLike = await _context.Likes.ToListAsync();
                   var newList = await _articleRepo.GetArticlesFromDbAsync() ?? new List<GetArticleResponse>();
                    
                    _cache.Set(AppConstants.CACHEKEYLIKE, updatedLike);

                    _cache.Set(AppConstants.CACHEKEYARTICLE, newList);
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
                var likeResponse = await CachedLikeData();
                return  likeResponse.Select(x => x.ArticleUniqueId == articleUniqueId && x.LikeStatus == true).Count();

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

                var LikesResponse = await CachedLikeData();
                
                return  LikesResponse.SingleOrDefault(x => x.ArticleUniqueId == articleUniqueId
                && x.LikerUniqueId == userUniqueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException.Message, " LikeRepositoryGetLikeAsync");
                return default;
            }

        }


        private async Task<List<Like>> CachedLikeData()
        {
            var cacheKey = AppConstants.CACHEKEYLIKE;

            if (!_cache.TryGetValue(cacheKey, out List<Like> LikesResponse))
            {

                LikesResponse = await _context.Likes.ToListAsync();


                var cacheExpiration = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays
                    (1),
                    Priority = CacheItemPriority.Normal,
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                };

                _cache.Set(cacheKey, LikesResponse, cacheExpiration);

            }

            return LikesResponse;
        }
    }
}
