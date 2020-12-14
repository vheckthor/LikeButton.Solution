using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Interfaces;
using LikeButton.Infrastructure.Data;
using LikeButton.Infrastructure.Data.Repository.ArticleRepository;
using LikeButton.Tests.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LikeButton.Tests.Infrastructure.ArticleRepositry
{
    public class ArticleRepositoryShould
    {

        [Fact]
        public async Task Return_Success_OnValue_Found_GetArticleAsync()
        {
            var mockCache = new Mock<IMemoryCache>();
            var mockCacheEntry = new Mock<ICacheEntry>();

            string? keyPayload = null;
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

            var _fileLoggerMock = new Mock<IFileLogger>();
            AppDbContext context = DbHelpers.InitContext("TestDB");

            await context.Articles.AddAsync(new LikeButton.Core.Entities.Article { ArticleUniqueIdentifier = Guid.Empty, Title = "some" });
            var saved = await context.SaveChangesAsync() > 0;
            Assert.True(saved);
            var command = new ArticleRepository(context, _fileLoggerMock.Object,mockCache.Object);

            var exc = await command.GetArticleAsync(Guid.Empty);
            Assert.NotNull(exc);
            Assert.True(exc.Title == "some");
        } 
        
        [Fact]
        public async Task Return_Success_OnValue_Found_GetArticlesAsync()
        {
            var _fileLoggerMock = new Mock<IFileLogger>();
            var mockCache = new Mock<IMemoryCache>();
            var mockCacheEntry = new Mock<ICacheEntry>();

            string? keyPayload = null;
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

            AppDbContext context = DbHelpers.InitContext("TestDB");

            await context.Articles.AddAsync(new LikeButton.Core.Entities.Article { ArticleUniqueIdentifier = Guid.NewGuid(), Title = "some" });
            var saved = await context.SaveChangesAsync() > 0;
            Assert.True(saved);
            var command = new ArticleRepository(context, _fileLoggerMock.Object,mockCache.Object);

            var exc = await command.GetArticlesAsync(new LikeButton.Core.Helpers.UserParams());
            Assert.NotNull(exc);
            Assert.True(exc.Count>0);
        }

        [Fact]
        public async Task Return_False_OnCreatesAnArticle_CreateArticleAsync()
        {
            var _fileLoggerMock = new Mock<IFileLogger>();
            
            AppDbContext context = DbHelpers.InitContext("TestDB");



            var mockCache = new Mock<IMemoryCache>();
            var mockCacheEntry = new Mock<ICacheEntry>();

            string? keyPayload = null;
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


            var command = new ArticleRepository(context, _fileLoggerMock.Object,mockCache.Object);

            var exc = await command.CreateArticleAsync(new LikeButton.Core.DTOs.APIRequest.CreateArticleRequest { ArticleUniqueIdentifier=Guid.NewGuid(),Body="England"});


            Assert.True(exc);
        }


    }
}
