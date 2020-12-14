using LikeButton.Core.DTOs.APIRequest;
using LikeButton.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LikeButton.Tests.API.Unit.LikeArticleControllr
{
    public class LikeArticleControllerShould:BaseControllerTest
    {
        [Fact]
        public async Task Return_Error_When_InputUserUniqueId_IsInvalid_ForLikeArticle() 
        {

            IActionResult response = await controller.LikeArticle(Guid.Empty,Guid.Empty,new LikeActionRequest());

            var okObjectResult = Assert.IsType<UnauthorizedObjectResult>(response);

            Assert.True(okObjectResult.StatusCode == 401);
            Assert.Contains("Invalid user Access", okObjectResult.Value.ToString());

        }
        
        [Fact]
        public async Task Return_Error_When_InputArticleUniqueId_IsInvalid_ForLikeArticle()
        {

            claimMock.Setup(x => x.GetClaims(It.IsAny<ClaimsPrincipal>()))
                .Returns(Guid.Empty.ToString());
            IActionResult response = await controller.LikeArticle(Guid.Empty, Guid.Empty, new LikeActionRequest());

            var okObjectResult = Assert.IsType<NotFoundObjectResult>(response);

            Assert.True(okObjectResult.StatusCode == 404);
            Assert.Contains("Article not Found", okObjectResult.Value.ToString());
        }
        
        [Fact]
        public async Task Return_Error_When_InputIsInvalidForLikeArticle_LikeStatus()
        {
            claimMock.Setup(x => x.GetClaims(It.IsAny<ClaimsPrincipal>()))
             .Returns(Guid.Empty.ToString());
            articleMock.Setup(x => x.GetArticleAsync(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new LikeButton.Core.DTOs.APIResponse.GetArticleResponse
                    {
                        DateAdded = DateTime.Now.Date,
                        Description = "striger",
                        ArticleLikes = 0,
                        ArticleUniqueIdentifier = Guid.Empty,
                        Body = "stere",
                        Title = "Title"
                    }
                );

            userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(
                new LikeButton.Core.Entities.User()
                );
            await Assert.ThrowsAsync<NullReferenceException>(async () => await controller.LikeArticle(Guid.Empty,Guid.Empty,null));

        }

        [Fact]
        public async Task Return_Success_When_InputIsvalid_ArticleIsLiked_ForLikeArticle()
        {
            claimMock.Setup(x => x.GetClaims(It.IsAny<ClaimsPrincipal>()))
             .Returns(Guid.Empty.ToString());
            articleMock.Setup(x => x.GetArticleAsync(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new LikeButton.Core.DTOs.APIResponse.GetArticleResponse
                    {
                        DateAdded = DateTime.Now.Date,
                        Description = "striger",
                        ArticleLikes = 0,
                        ArticleUniqueIdentifier = Guid.Empty,
                        Body = "stere",
                        Title = "Title"
                    }
                );

            userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(
                new LikeButton.Core.Entities.User()
                );
            likeManagerMock.Setup(x => x.AddAsync(It.IsAny<Like>())).ReturnsAsync(true);
            

            IActionResult result = await controller.LikeArticle(Guid.Empty, Guid.Empty, new LikeActionRequest { ArticleLiked = true}); 

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(okObjectResult.StatusCode == 200);
            var more = okObjectResult.Value;
            Assert.NotNull(more);
            Assert.Contains("Article liked", more.ToString());
        }

        [Fact]
        public async Task Return_Success_When_When_InputIsvalid_ArticleIsUnLiked_ForLikeArticle()
        {
            claimMock.Setup(x => x.GetClaims(It.IsAny<ClaimsPrincipal>()))
             .Returns(Guid.Empty.ToString());
            articleMock.Setup(x => x.GetArticleAsync(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new LikeButton.Core.DTOs.APIResponse.GetArticleResponse
                    {
                        DateAdded = DateTime.Now.Date,
                        Description = "striger",
                        ArticleLikes = 0,
                        ArticleUniqueIdentifier = Guid.Empty,
                        Body = "stere",
                        Title = "Title"
                    }
                );

            userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(
                new LikeButton.Core.Entities.User()
                );
            likeManagerMock.Setup(x => x.AddAsync(It.IsAny<Like>())).ReturnsAsync(true);


            IActionResult result = await controller.LikeArticle(Guid.Empty, Guid.Empty, new LikeActionRequest { ArticleLiked = false });

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(okObjectResult.StatusCode == 200);
            var more = okObjectResult.Value;
            Assert.NotNull(more);
            Assert.Contains("Article unliked", more.ToString());
        }


        /// <summary>
        /// To avoid duplicate likes and Unlikes from scrupulous users
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Return_Error_When_InputIsvalid_AndArticle__IsAlreadyLiked_ForLikeArticle()
        {
            claimMock.Setup(x => x.GetClaims(It.IsAny<ClaimsPrincipal>()))
             .Returns(Guid.Empty.ToString());
            articleMock.Setup(x => x.GetArticleAsync(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new LikeButton.Core.DTOs.APIResponse.GetArticleResponse
                    {
                        DateAdded = DateTime.Now.Date,
                        Description = "striger",
                        ArticleLikes = 0,
                        ArticleUniqueIdentifier = Guid.Empty,
                        Body = "stere",
                        Title = "Title"
                    }
                );

            userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(
                new LikeButton.Core.Entities.User()
                );
            likeManagerMock.Setup(x => x.AddAsync(It.IsAny<Like>())).ReturnsAsync(false);


            IActionResult result = await controller.LikeArticle(Guid.Empty, Guid.Empty, new LikeActionRequest { ArticleLiked = true });

            var okObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(okObjectResult.StatusCode == 400);
            var more = okObjectResult.Value;
            Assert.NotNull(more);
            Assert.Contains("You already liked this article", more.ToString());
        }

        [Fact]
        public async Task Return_Error_When_InputIsvalid_AndArticle__IsAlreadyUnLiked_ForLikeArticle()
        {
            claimMock.Setup(x => x.GetClaims(It.IsAny<ClaimsPrincipal>()))
             .Returns(Guid.Empty.ToString());
            articleMock.Setup(x => x.GetArticleAsync(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new LikeButton.Core.DTOs.APIResponse.GetArticleResponse
                    {
                        DateAdded = DateTime.Now.Date,
                        Description = "striger",
                        ArticleLikes = 0,
                        ArticleUniqueIdentifier = Guid.Empty,
                        Body = "stere",
                        Title = "Title"
                    }
                );

            userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(
                new LikeButton.Core.Entities.User()
                );
            likeManagerMock.Setup(x => x.AddAsync(It.IsAny<Like>())).ReturnsAsync(false);


            IActionResult result = await controller.LikeArticle(Guid.Empty, Guid.Empty, new LikeActionRequest { ArticleLiked = false });

            var okObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(okObjectResult.StatusCode == 400);
            var more = okObjectResult.Value;
            Assert.NotNull(more);
            Assert.Contains("You already unliked this article", more.ToString());
        }

    }
}
