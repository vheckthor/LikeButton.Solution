using System;
using System.Threading.Tasks;
using LikeButton.API.Extensions;
using LikeButton.Core.DTOs.APIRequest;
using LikeButton.Core.Helpers;
using LikeButton.Infrastructure.Data.Repository.ArticleRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LikeButton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _repo;


        public ArticleController(IArticleRepository repo)
        {
            _repo = repo;
           
        }

        [HttpGet("GetArticles")]
        public async Task<IActionResult> GetArticles([FromQuery] UserParams param)
        {

            var articles = await _repo.GetArticlesAsync(param);


            if (articles == null)
            {
                return BadRequest("Articles not found");
            }
                Response.AddPagination(articles.CurrentPage,
            articles.PageSize, articles.TotalCount, articles.TotalPages);


            return Ok(articles);
        }

        

        [HttpGet("{ArticleUniqueIdentity}", Name = "GetArticle")]
        public async Task<IActionResult> GetArticle(Guid ArticleUniqueIdentity)
        {
            var article = await _repo.GetArticleAsync(ArticleUniqueIdentity);
           if(article != null)
           {
                return Ok(article);
           }
            return NotFound("Article not found");

        }
        

        [Authorize]
        [HttpPost("CreateArticle")]
        public async Task<IActionResult> CreateArticle(CreateArticleUserData request)
        {

            var quest = new CreateArticleRequest
            {
                ArticleUniqueIdentifier = Guid.NewGuid(),
                Body = request.Body,
                Description = request.Description,
                Title = request.Title

            };

            var response = await _repo.CreateArticleAsync(quest);

            if(response)
            {
                return CreatedAtRoute("GetArticle", new { Controller = "Article", ArticleUniqueIdentity = quest.ArticleUniqueIdentifier }, 
                    new {quest.ArticleUniqueIdentifier, quest.Body, quest.Description, quest.Title });
            }
            return BadRequest("Unable to create article");
        }

    }
}
