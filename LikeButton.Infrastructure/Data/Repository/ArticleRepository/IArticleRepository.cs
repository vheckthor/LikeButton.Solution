using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Helpers;
using LikeButton.Core.Interfaces.IArticle;
using LikeButton.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeButton.Infrastructure.Data.Repository.ArticleRepository
{
    public interface IArticleRepository:IArticle
    {

        Task<PagedList<GetArticleResponse>> GetArticlesAsync(UserParams param);
        public Task<List<GetArticleResponse>> GetArticlesFromDbAsync();

    }
}
