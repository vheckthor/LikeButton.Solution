using LikeButton.Core.Entities;
using LikeButton.Tests.Core.Helpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LikeButton.Tests.Core
{
    public class CoreEntitiesShould
    {
        [Fact]
        public void Article_Should_Return_Values_Set()
        {
            var result = EntityModelSetters.SetArticle();
            result.ShouldNotBeNull();
            result.Body.ShouldBe("value");
            result.Title.ShouldBe("value");
            result.Description.ShouldBe("value");
            result.Id.ShouldBe(1);
            result.ArticleUniqueIdentifier.ShouldBe(Guid.Empty);
            result.DateAdded.ShouldBe(DateTime.Now.Date);

        }
        [Fact]
        public void User_Should_Return_Values_Set()
        {
            var result = EntityModelSetters.SetUser();
            result.ShouldNotBeNull();
            result.Email.ShouldBe("value");
            result.ArticleLiked.ShouldBe(new List<Like>());
            result.Username.ShouldBe("value");
            result.UserUniqueIdentity.ShouldBe(Guid.Empty);

        }
        [Fact]
        public void Like_Should_Return_Values_Set()
        {
            var result = EntityModelSetters.SetLike();
            result.ShouldNotBeNull();
            result.ArticleUniqueId.ShouldBe(Guid.Empty);
            result.Id.ShouldBe(1);
            result.LikeStatus.ShouldBe(true);
            result.UserId.ShouldBe(1);
            result.LikerUniqueId.ShouldBe(Guid.Empty);

        }
    }
}
