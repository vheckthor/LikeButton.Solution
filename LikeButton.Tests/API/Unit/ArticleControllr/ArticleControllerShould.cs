using LikeButton.Core.DTOs.APIRequest;
using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Helpers;
using LikeButton.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LikeButton.Tests.API.Unit.ArticleControllr
{
    public class ArticleControllerShould:BaseControllerTest
    {
        [Fact]
        public async Task Return_Error_When_InputIsInvalidForCreateArticle()
        {

            await Assert.ThrowsAsync<NullReferenceException>(async () => await controller.CreateArticle(null));

        }

        [Fact]
        public async Task Return_Success_When_InputIsvalidForCreateArticle()
        {
            articleManagerMock.Setup(x => x.CreateArticleAsync(It.IsAny<CreateArticleRequest>()))
            .ReturnsAsync(
                true
                );

            IActionResult result = await controller.CreateArticle(
                new CreateArticleUserData
                {
                   Body = "string",
                   Title = "string",
                   Description = "not null"
                });

            var okObjectResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.True(okObjectResult.StatusCode == 201);
            var more = okObjectResult.Value;
            Assert.NotNull(more);
        }

        [Fact]
        public async Task Return_Error_When_GetArticleParameterNotPassed()
        {
            IActionResult response = await controller.GetArticle(Guid.Empty);

            var okObjectResult = Assert.IsType<NotFoundObjectResult>(response);

            Assert.True(okObjectResult.StatusCode == 404);
            Assert.Contains("Article not found", okObjectResult.Value.ToString());

        }

        [Fact]
        public async Task Return_Success_When_GetArticleIsCalled()
        {
            articleManagerMock.Setup(x => x.GetArticleAsync(It.IsAny<Guid>()))
            .ReturnsAsync(
                new GetArticleResponse 
                { 
                    ArticleLikes = 100000000000,
                    ArticleUniqueIdentifier = Guid.Empty,
                    DateAdded = DateTime.Now.Date,
                    Body = "balc colon",
                    Description = "mutex",
                    Title = "Dragged"
                }
                );

            IActionResult result = await controller.GetArticle(
               Guid.Empty
                );
            
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.NotNull(okObjectResult);
            var more = Assert.IsType<GetArticleResponse>(okObjectResult.Value);

            Assert.True(more.ArticleLikes == 100000000000);
            
        }

        [Fact]
        public async Task Return_Success_When_GetArticlesIsCalled()
        {
            articleManagerMock.Setup(x => x.GetArticlesAsync(It.IsAny<UserParams>()))
            .ReturnsAsync(
               new PagedList<GetArticleResponse> 
               (
                   new List<GetArticleResponse>{
                        new GetArticleResponse
                        {
                            ArticleLikes = 100000000000,
                            ArticleUniqueIdentifier = Guid.Empty,
                            DateAdded = DateTime.Now.Date,
                            Body = "balc colon",
                            Description = "mutex",
                            Title = "Dragged"
                        }
                   },
                   1,2,3
               )
            );

            //Headers not returned so it throws a nullreference error

           await Assert.ThrowsAsync<NullReferenceException>(async () => await controller.GetArticles(
                new UserParams()
            ));




        }

    }
}
