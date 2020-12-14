using LikeButton.Core.DTOs.APIRequest;
using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace LikeButton.Core.Interfaces.IArticle
{
    public interface IArticle
    {
        Task<GetArticleResponse> GetArticleAsync(Guid articleuniqueId);
        Task<bool> CreateArticleAsync(CreateArticleRequest request);
    }
}
