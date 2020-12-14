using LikeButton.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeButton.Tests.Core.Helpers
{
    public class EntityModelSetters
    {
        public static Article SetArticle()
        {
            return new Article
            {
                ArticleUniqueIdentifier = Guid.Empty,
                Body = "value",
                Description = "value",
                Id = 1,
                Title = "value",
                DateAdded = DateTime.Now.Date
            };
        }

        public static Like SetLike()
        {
            return new Like
            {
                ArticleUniqueId = Guid.Empty,
                Id = 1,
                LikeStatus = true,
                User = new User(),
                UserId = 1,
                LikerUniqueId = Guid.Empty

            };
        }

        public static User SetUser()
        {
            return new User
            {
                Email = "value",
                ArticleLiked = new List<Like>(),
                Username = "value",
                UserUniqueIdentity = Guid.Empty
            };
        }
    }
}
