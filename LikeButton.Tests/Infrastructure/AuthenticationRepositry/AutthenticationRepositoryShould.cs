using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Interfaces;
using LikeButton.Infrastructure.Data;
using LikeButton.Infrastructure.Data.Repository;
using LikeButton.Tests.Infrastructure.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LikeButton.Tests.Infrastructure.AuthenticationRepositry
{
    public class AutthenticationRepositoryShould
    {
        [Fact]
        public async Task Return_Success_OnCall_LoginAsync()
        {
            var _fileLoggerMock = new Mock<IFileLogger>();
            AppDbContext context = DbHelpers.InitContext("TestDB");
            var command = new AuthenticationRepository(context, _fileLoggerMock.Object);

            var res = await command.RegisterAsync(new LikeButton.Core.DTOs.APIRequest.CreateUserAPIRequest { Email = "gru@gmail.com",
                username = "dan", password = "null@lol"
            });
          
          
           

            var exc = await command.LoginAsync(new LikeButton.Core.DTOs.APIRequest.UserLoginRequest {username="dan",password="null@lol" });
            Assert.NotNull(exc);
            Assert.IsType<UserSuccessLoginResponse>(exc);
        }

        [Fact]
        public async Task Return_Success_OnCall_UserExistsAsync()
        {
            var _fileLoggerMock = new Mock<IFileLogger>();
            AppDbContext context = DbHelpers.InitContext("TestDB");

            await context.Users.AddAsync(new LikeButton.Core.Entities.User { Email = "adede@gmail.com", Username = "goal" });
            var saved = await context.SaveChangesAsync() > 0;
            Assert.True(saved);
            var command = new AuthenticationRepository(context, _fileLoggerMock.Object);

            var exc = await command.UserExistsAsync("goal");
            
            Assert.True(exc);
        }

        [Fact]
        public async Task Return_Success_OnCreateUser_RegisterAsync()
        {
            var _fileLoggerMock = new Mock<IFileLogger>();
            AppDbContext context = DbHelpers.InitContext("TestDB");


            var command = new AuthenticationRepository(context, _fileLoggerMock.Object);

            var exc = await command.RegisterAsync(new LikeButton.Core.DTOs.APIRequest.CreateUserAPIRequest { Email="ade@gmail.com",password="null@bool",username = "drey"});

            Assert.NotNull(exc);
            Assert.IsType<SuccessCreateUserResponse>(exc);
        }
    }
}
