using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LikeButton.Core.DTOs.APIRequest;
using LikeButton.Core.Entities;
using LikeButton.Core.Interfaces;
using LikeButton.Core.Interfaces.ILikeRepository;
using LikeButton.Core.Interfaces.IUserRepository;
using LikeButton.Infrastructure.Data.Repository.ArticleRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LikeButton.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LikeArticleController : ControllerBase
    {
        private readonly IArticleRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly ILikeRepository _likeRepo;
        private readonly IClaim _clame;

        public LikeArticleController(IArticleRepository repo, IUserRepository userRepo, ILikeRepository likeRepo,IClaim clame)
        {
            _repo = repo;
            _userRepo = userRepo;
            _likeRepo = likeRepo;
            _clame = clame;
        }



        [HttpPost("{UserUniqueIdentity}/like/{ArticleUniqueId}")]
        public async Task<IActionResult> LikeArticle(Guid UserUniqueIdentity, Guid ArticleUniqueId,[FromBody] LikeActionRequest likeStatus)
        {
            var claim = _clame.GetClaims(User);

            if (UserUniqueIdentity.ToString() != claim)
            {
                return Unauthorized("Invalid user Access");
            }
            var articleLiked = await _repo.GetArticleAsync(ArticleUniqueId);

            var user= await _userRepo.GetUserAsync(UserUniqueIdentity);
            if (articleLiked == null)
            {
                return NotFound("Article not Found");
            }

            var like = new Like
            {
                LikerUniqueId = user.UserUniqueIdentity,
                LikeStatus = likeStatus.ArticleLiked,
                ArticleUniqueId = articleLiked.ArticleUniqueIdentifier,
                UserId = user.Id,
                User = user

            };

            
            var response = await _likeRepo.AddAsync(like);
            var actionupdate = likeStatus.ArticleLiked ? "Article liked":"Article unliked";


            if (response)
            {
                return Ok(actionupdate);
            }

            var actionupdateBadRequest = likeStatus.ArticleLiked ? "You already liked this article" : "You already unliked this article";
          
            return BadRequest(actionupdateBadRequest);
        }
    }
}
