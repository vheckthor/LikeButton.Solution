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
                var result = await _context.Articles.FirstOrDefaultAsync(x => x.ArticleUniqueIdentifier == articleuniqueId);
                var articlelikes = await _context.Likes.Select(x => x.ArticleUniqueId == articleuniqueId && x.LikeStatus == true).CountAsync();

                if(result == default)
                {
                    return default;
                }

                return new GetArticleResponse
                {
                    DateAdded = result.DateAdded,
                    ArticleUniqueIdentifier = result.ArticleUniqueIdentifier,
                    Description = result.Description,
                    Body = result.Body,
                    Title = result.Title,
                    ArticleLikes = articlelikes
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException.Message, " ArticleRepositoryGetArticleAsync");
                return default;
            }

        }

       // public async Task<PagedList<GetArticleResponse>> GetLikedArticlesAsync(UserParams param)
        //{

        //}
        
        public async Task<PagedList<GetArticleResponse>> GetArticlesAsync(UserParams param)
        {
            try 
            {
                var cacheKey = AppConstants.CACHEKEYARTICLE;

                if (!_cache.TryGetValue(cacheKey, out List<GetArticleResponse> articlesResponse))
                {

                   articlesResponse = await GetArticlesFromDbAsync();


                    var cacheExpiration = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddDays
                        (1),
                        Priority = CacheItemPriority.Normal,
                        SlidingExpiration = TimeSpan.FromMinutes(10)
                    };

                    _cache.Set(cacheKey, articlesResponse, cacheExpiration);

                }

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


    }
}
