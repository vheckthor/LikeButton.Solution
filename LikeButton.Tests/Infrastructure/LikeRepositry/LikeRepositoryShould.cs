using LikeButton.Core.Interfaces;
using LikeButton.Infrastructure.Data;
using LikeButton.Infrastructure.Data.Repository.ArticleRepository;
using LikeButton.Infrastructure.Data.Repository.LikeRepository;
using LikeButton.Tests.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LikeButton.Tests.Infrastructure.LikeRepositry
{
    public class LikeRepositoryShould
    {
        [Fact]
        public void Throw_ArgumentNullException_On_Null_DbContextInjection()
        {
            var _fileLoggerMock = new Mock<IFileLogger>();
            var _cacheMock = new Mock<IMemoryCache>();
            var _articleMock = new Mock<IArticleRepository>();

            ArgumentNullException exc = Assert.Throws<ArgumentNullException>(() => new LikeRepository(default, _fileLoggerMock.Object,
                _cacheMock.Object,_articleMock.Object));
            Assert.Equal("appDbContext", exc.ParamName);
            Assert.Equal("Null DBContext injection (Parameter 'appDbContext')", exc.Message);
        }

        [Fact]
        public void Throw_ArgumentNullException_On_Null_FileLoggerInjection()
        {
           ;
            var _cacheMock = new Mock<IMemoryCache>();
            var _articleMock = new Mock<IArticleRepository>();

            AppDbContext context = DbHelpers.InitContextWithTransactionAndSQLSupport();

            ArgumentNullException exc = Assert.Throws<ArgumentNullException>(() => new LikeRepository(context, default,
                _cacheMock.Object, _articleMock.Object));
            Assert.Equal("fileLogger", exc.ParamName);
            Assert.Equal("Null FileLogger injection (Parameter 'fileLogger')", exc.Message);
        }

        [Fact]
        public async Task ReturnNull_WhenValueNotFound__GetLikeAsync()
        {
            var _fileLoggerMock = new Mock<IFileLogger>();
            var mockCache = new Mock<IMemoryCache>();
            var mockCacheEntry = new Mock<ICacheEntry>();

            string keyPayload = null;
            mockCache
                .Setup(mc => mc.CreateEntry(It.IsAny<object>()))
                .Callback((object k) => keyPayload = (string)k)
                .Returns(mockCacheEntry.Object); 

            object? valuePayload = null;
            mockCacheEntry
                .SetupSet(mce => mce.Value = It.IsAny<object>())
                .Callback<object>(v => valuePayload = v);

            TimeSpan? expirationPayload = null;
            mockCacheEntry
                .SetupSet(mce => mce.AbsoluteExpirationRelativeToNow = It.IsAny<TimeSpan?>())
                .Callback<TimeSpan?>(dto => expirationPayload = dto);

            var _articleMock = new Mock<IArticleRepository>();
            AppDbContext context = DbHelpers.InitContext("TestDB");
            
            var command = new LikeRepository(context, _fileLoggerMock.Object, mockCache.Object, _articleMock.Object);

            var exc = await command.GetLikeAsync(default,default);

            Assert.Null(exc);
        }
        
        [Fact]
        public async Task Return_Success_OnValue_Found_AddLikeAsync()
        {
            var _fileLoggerMock = new Mock<IFileLogger>();
           
            var mockCache = new Mock<IMemoryCache>();
            var mockCacheEntry = new Mock<ICacheEntry>();

            string? keyPayload = null;
            mockCache
                .Setup(mc => mc.CreateEntry(It.IsAny<object>()))
                .Callback((object k) => keyPayload = (string)k)
                .Returns(mockCacheEntry.Object); // this should address your null reference exception

            object? valuePayload = null;
            mockCacheEntry
                .SetupSet(mce => mce.Value = It.IsAny<object>())
                .Callback<object>(v => valuePayload = v);

            TimeSpan? expirationPayload = null;
            mockCacheEntry
                .SetupSet(mce => mce.AbsoluteExpirationRelativeToNow = It.IsAny<TimeSpan?>())
                .Callback<TimeSpan?>(dto => expirationPayload = dto);

            var _articleMock = new Mock<IArticleRepository>();

            _articleMock.Setup(x => x.GetArticlesFromDbAsync())
                .ReturnsAsync(
                new List<LikeButton.Core.DTOs.APIResponse.GetArticleResponse>());
            AppDbContext context = DbHelpers.InitContext("TestDB");
            var articleguid = Guid.NewGuid();
            var likerguid = Guid.NewGuid();
            context.Likes.Add(new LikeButton.Core.Entities.Like { ArticleUniqueId = articleguid, LikerUniqueId = likerguid, LikeStatus = true });
             await context.SaveChangesAsync();
            var command = new LikeRepository(context, _fileLoggerMock.Object, mockCache.Object, _articleMock.Object);

            var exc = await command.AddAsync(new LikeButton.Core.Entities.Like { ArticleUniqueId =articleguid ,LikeStatus = false,
                LikerUniqueId = likerguid});
          
            Assert.True(exc);
        }
        
        [Fact]
        public async Task Return_False_OnUserAlready_AddLikeAsync()
        {
            var _fileLoggerMock = new Mock<IFileLogger>();
            
            var mockCache = new Mock<IMemoryCache>();
            var mockCacheEntry = new Mock<ICacheEntry>();

            string? keyPayload = null;
            mockCache
                .Setup(mc => mc.CreateEntry(It.IsAny<object>()))
                .Callback((object k) => keyPayload = (string)k)
                .Returns(mockCacheEntry.Object); // this should address your null reference exception

            object? valuePayload = null;
            mockCacheEntry
                .SetupSet(mce => mce.Value = It.IsAny<object>())
                .Callback<object>(v => valuePayload = v);

            TimeSpan? expirationPayload = null;
            mockCacheEntry
                .SetupSet(mce => mce.AbsoluteExpirationRelativeToNow = It.IsAny<TimeSpan?>())
                .Callback<TimeSpan?>(dto => expirationPayload = dto);

            var _articleMock = new Mock<IArticleRepository>();
            AppDbContext context = DbHelpers.InitContext("TestDB");

            await context.Likes.AddAsync(new LikeButton.Core.Entities.Like { ArticleUniqueId = Guid.Empty, LikerUniqueId = Guid.Empty });
            var saved = await context.SaveChangesAsync() > 0;
            Assert.True(saved);
            var command = new LikeRepository(context, _fileLoggerMock.Object, mockCache.Object, _articleMock.Object);

            var exc = await command.AddAsync(new LikeButton.Core.Entities.Like { ArticleUniqueId = Guid.Empty, LikerUniqueId = Guid.Empty });
          
          
            Assert.True(!exc);
        }


    }
}
