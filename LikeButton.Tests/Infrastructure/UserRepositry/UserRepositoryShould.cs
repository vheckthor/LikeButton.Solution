using LikeButton.Core.Interfaces;
using LikeButton.Infrastructure.Data;
using LikeButton.Infrastructure.Data.Repository.UserRepository;
using LikeButton.Tests.Infrastructure.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LikeButton.Tests.Infrastructure.UserRepositry
{
    public class UserRepositoryShould
    {
        [Fact]
        public void Throw_ArgumentNullException_On_Null_DbContextInjection()
        {
            var _fileLoggerMock = new Mock<IFileLogger>();

            ArgumentNullException exc = Assert.Throws<ArgumentNullException>(() => new UserRepository(default, _fileLoggerMock.Object));
            Assert.Equal("appDbContext", exc.ParamName);
            Assert.Equal("Null DBContext injection (Parameter 'appDbContext')", exc.Message);
        }

        [Fact]
        public void Throw_ArgumentNullException_On_Null_FileLoggerInjection()
        {
            AppDbContext context = DbHelpers.InitContextWithTransactionAndSQLSupport();
            ArgumentNullException exc = Assert.Throws<ArgumentNullException>(() => new UserRepository(context, default));
            Assert.Equal("fileLogger", exc.ParamName);
            Assert.Equal("Null FileLogger injection (Parameter 'fileLogger')", exc.Message);
        }


        
        [Fact]
        public async Task ReturnsValue_On_GuidFound_GetUserAsync()
        {
            var _fileLoggerMock = new Mock<IFileLogger>();
            AppDbContext context = DbHelpers.InitContext("TestDB");
            var constguid = Guid.NewGuid();
            context.Users.Add(new LikeButton.Core.Entities.User { UserUniqueIdentity = constguid, Username = "Drey", Email = "ade@gmail.com" });
            var saved = await context.SaveChangesAsync()>0;
            Assert.True(saved);
            var command = new UserRepository(context, _fileLoggerMock.Object);
            
            var exc = await command.GetUserAsync(constguid);

            Assert.NotNull(exc);
            Assert.True(exc.Username == "Drey");
            Assert.True(exc.Email == "ade@gmail.com");
        }

    }
}
