using LikeButton.Core.DTOs;
using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LikeButton.Tests.API.Unit.UsersControllr
{
    public class UserControllerShould:BaseControllerTest
    {
        [Fact]
        public async Task Return_Error_When_InputUserUniqueId_IsInvalid_ForGetUser()
        {

            IActionResult response = await controller.GetUser(Guid.Empty);
            Assert.NotNull(response);
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(response);

            Assert.True(okObjectResult.StatusCode == 404);
            Assert.Contains("User not found", okObjectResult.Value.ToString());
        }


        [Fact]
        public async Task Return_Success_When_InputUserUniqueId_IsValid_ForGetUser()
        {

            userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new User
                    {
                        Email = "a@gmail.com",
                        ArticleLiked = new List<Like>(),
                        Username = "drey",
                        UserUniqueIdentity = Guid.Empty
                    }

                );
            IActionResult response = await controller.GetUser(Guid.Empty);
            Assert.NotNull(response);
            var okObjectResult = Assert.IsType<OkObjectResult>(response);
            Assert.True(okObjectResult.StatusCode == 200);
            var more = Assert.IsType<GetUserResponse>(okObjectResult.Value);
            Assert.True(Guid.Empty == more.UserUniqueIdentity);
            Assert.Contains("a@gmail.com", more.Email);
            Assert.Contains("drey", more.Username);
        }


    }
}
