using LikeButton.Core.DTOs.APIRequest;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using LikeButton.Core.DTOs;

namespace LikeButton.Tests.API.Unit.AuthenticationControllr
{
    public class AuthenticationControllerShould:BaseControllerTest
    {
        [Fact]
        public async Task Return_Error_When_InputIsInvalidForRegister()
        {

            await Assert.ThrowsAsync<NullReferenceException>(async()=>await controller.Register(null));
         
        }

        [Fact]
        public async Task Return_Success_When_InputIsvalidForRegister()
        {
            authenticationManagerMock.Setup(x => x.RegisterAsync(It.IsAny<CreateUserAPIRequest>()))
            .ReturnsAsync(
            
                new SuccessCreateUserResponse
                {
                    Email = "ade@gmail.com",
                    Username = "drey",
                    UserUniqueIdentity = Guid.Empty
                }
                );

            IActionResult result = await controller.Register(
                new CreateUserAPIRequest
                {
                    Email = "ade@gmail.com",
                    username = "drey",
                    password = "null@bool"
                });

            // CreatedAtRoute("GetUser", new { Controller = "Users", UserUniqueIdentity = result.UserUniqueIdentity }, result)
            var okObjectResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.True(okObjectResult.StatusCode == 201);
            var more = Assert.IsType<SuccessCreateUserResponse>(okObjectResult.Value);
            Assert.True(more.Email == "ade@gmail.com");
            Assert.True(more.Username == "drey");
            Assert.True(more.UserUniqueIdentity == Guid.Empty);
        }


        [Fact]
        public async Task Return_Success_When_InputIsvalidForLogin()
        {


   
            tokenManagerMock.Setup(x => x.GeneratedToken(It.IsAny<User>())).Returns("ldsjgfgljfcgflfgdfsgkjhbjkgdfhklfdg");

            authenticationManagerMock.Setup(x => x.LoginAsync(It.IsAny<UserLoginRequest>()))
                .ReturnsAsync(
                    new UserSuccessLoginResponse
                    {
                        ArticleLiked = new List<LikeDTO>(),
                        Email = "dram@gim.com",
                        Username = "drey",
                        UserUniqueIdentity = Guid.Empty
                    }

                );


           

            IActionResult result = await controller.Login(
                new UserLoginRequest
                {
                    username = "drey",
                    password = "null@bool"
                });
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(okObjectResult.StatusCode == 200);
            var more = okObjectResult.Value;

            //Assert.True(more.token == )
        }


        [Fact]
        public async Task Return_Error_When_InputIsInvalidForLogin()
        {
            IActionResult response = await controller.Login(null);
            var okObjectResult = Assert.IsType<UnauthorizedObjectResult>(response);

            Assert.True(okObjectResult.StatusCode == 401);
            Assert.Contains("You do not have access to login", okObjectResult.Value.ToString());
        }

        
    }
}
