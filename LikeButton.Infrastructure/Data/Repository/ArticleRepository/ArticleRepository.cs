using LikeButton.Core.Constants;
using LikeButton.Core.DTOs.APIRequest;
using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Entities;
using LikeButton.Core.Helpers;
using LikeButton.Core.Interfaces;
using LikeButton.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeButton.Infrastructure.Data.Repository.ArticleRepository
{
    public class ArticleRepository : IArticleRepository
    {

        private readonly AppDbContext _context;
        private readonly IFileLogger _logger;
        private readonly IMemoryCache _cache;

        public ArticleRepository(AppDbContext context, IFileLogger logger,IMemoryCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        public async Task<bool> CreateArticleAsync(CreateArticleRequest request)
        {
            try
            {
                var article = new Article
                {
                    ArticleUniqueIdentifier = request.ArticleUniqueIdentifier,
                    DateAdded = DateTime.Now,
                    Description = request.Description,
                    Title = request.Title,
                    Body = request.Body,
 
                    
                };

                var newUpdate = await CachedData();
                newUpdate.Add(new GetArticleResponse
                {
                    ArticleLikes = 0,
                    ArticleUniqueIdentifier = article.ArticleUniqueIdentifier,
                    DateAdded = article.DateAdded,
                    Body = article.Body,
                    Description = article.Description,
                    Title = article.Title

                });
                _cache.Set(AppConstants.CACHEKEYARTICLE, newUpdate);


                await _context.Articles.AddAsync(article);
                var result = await _context.SaveChangesAsync();
                if(result > 0)
                {
                    
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException.Message, " ArticleRepositoryCreateArticleAsync");
                return default;
            }

        }

        public async Task<GetArticleResponse> GetArticleAsync(Guid articleuniqueId)
        {
            try
            {
                var result = await CachedData();

                return result.SingleOrDefault(x => x.ArticleUniqueIdentifier == articleuniqueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException.Message, " ArticleRepositoryGetArticleAsync");
                return default;
            }

        }

       
        public async Task<PagedList<GetArticleResponse>> GetArticlesAsync(UserParams param)
        {
            try 
            {


                var articlesResponse =await CachedData();

                return await PagedList<GetArticleResponse>.CreateAsync(articlesResponse, param.PageNUmber, param.PageSize);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException.Message, " ArticleRepositoryGetArticlesAsync");
                return default;
            }

        }

        public async Task<List<GetArticleResponse>> GetArticlesFromDbAsync()
        {
            var articles = _context.Articles.AsEnumerable().OrderByDescending(x => x.DateAdded);


            var articlesResponse = new List<GetArticleResponse>();

            foreach (var art in articles)
            {
                var articlelikes = await _context.Likes.Where(
                    x => x.ArticleUniqueId == art.ArticleUniqueIdentifier && x.LikeStatus == true)
                    .CountAsync();

                var formatResponse = new GetArticleResponse
                {
                    ArticleUniqueIdentifier = art.ArticleUniqueIdentifier,
                    DateAdded = art.DateAdded,
                    Body = art.Body,
                    Description = art.Description,
                    Title = art.Title,
                    ArticleLikes = articlelikes
                };

                articlesResponse.Add(formatResponse);

            }
            return articlesResponse;

        }

        private async Task<List<GetArticleResponse>> CachedData()
        {
            var cacheKey = AppConstants.CACHEKEYARTICLE;
            if (!_cache.TryGetValue(cacheKey, out List<GetArticleResponse> articlesResponse))
            {

                articlesResponse = await GetArticlesFromDbAsync() ?? new List<GetArticleResponse>();


                var cacheExpiration = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays
                    (1),
                    Priority = CacheItemPriority.Normal,
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                };

                _cache.Set(cacheKey, articlesResponse, cacheExpiration);

            }
            return articlesResponse;
        }


    }
}
