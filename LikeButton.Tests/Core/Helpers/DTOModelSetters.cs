using LikeButton.Core.Constants;
using LikeButton.Core.DTOs;
using LikeButton.Core.DTOs.APIRequest;
using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeButton.Tests.Core.Helpers
{
    public class DTOModelSetters
    {

        public static CreateArticleRequest SetCreateArticleRequest()
        {
            return new CreateArticleRequest
            {
                Description = "value",
                ArticleUniqueIdentifier = Guid.Empty,
                Body = "value",
                Title = "value"

            };
        }


        public static CreateArticleUserData SetCreateArticleUserData()
        {
            return new CreateArticleUserData
            {
                Description = "value",
                Body = "value",
                Title = "value"
            };
        }

        public static CreateUserAPIRequest SetCreateUserAPIRequest()
        {
            return new CreateUserAPIRequest()
            {
                Email = "value",
                password = "value",
                username = "value"
            };
        }


        public static LikeActionRequest SetLikeActionRequest()
        {
            return new LikeActionRequest
            {
                ArticleLiked = true
            };
        }

        public static UserLoginRequest SetUserLoginRequest()
        {
            return new UserLoginRequest
            {
                password = "value",
                username = "value"
            };
        }


        public static GetArticleResponse SetGetArticleResponse()
        {
            return new GetArticleResponse
            {
                DateAdded = DateTime.Now.Date,
                ArticleLikes = 0,
                ArticleUniqueIdentifier = Guid.Empty,
                Description = "value",
                Body = "value",
                Title = "value"
            };
        }

        public static SuccessCreateUserResponse SetSuccessCreateUserResponse()
        {
            return new SuccessCreateUserResponse
            {
                Email = "value",
                Username = "value",
                UserUniqueIdentity = Guid.Empty
            };
        }

        public static UserSuccessLoginResponse SetUserSuccessLoginResponse()
        {
            return new UserSuccessLoginResponse
            {
                ArticleLiked = new List<LikeDTO>(),
                Email = "value",
                Username = "value",
                UserUniqueIdentity = Guid.Empty

            };
        }
    }
}
