using LikeButton.Core.DTOs;
using LikeButton.Core.Entities;
using LikeButton.Tests.Core.Helpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LikeButton.Tests.Core
{
    public class CoreDTOShould
    {
        [Fact]
        public void CreateArticleRequest_Should_Return_Values_Set()
        {
            var result =DTOModelSetters.SetCreateArticleRequest();
            result.ShouldNotBeNull();
            result.Body.ShouldBe("value");
            result.Title.ShouldBe("value");
            result.Description.ShouldBe("value");

        } 
        
        [Fact]
        public void CreateUserAPIRequest_Should_Return_Values_Set()
        {
            var result =DTOModelSetters.SetCreateUserAPIRequest();
            result.ShouldNotBeNull();
            result.password.ShouldBe("value");
            result.username.ShouldBe("value");
            result.Email.ShouldBe("value");
       
        }
        
        [Fact]
        public void LikeActionRequest_Should_Return_Values_Set()
        {
            var result =DTOModelSetters.SetLikeActionRequest();
            result.ShouldNotBeNull();
            result.ArticleLiked.ShouldBe(true);


        }
        
        [Fact]
        public void UserLoginRequest_Should_Return_Values_Set()
        {
            var result =DTOModelSetters.SetUserLoginRequest();
            result.ShouldNotBeNull();
            result.username.ShouldBe("value");
            result.password.ShouldBe("value");


        }   
        
        [Fact]
        public void GetArticleResponse_Should_Return_Values_Set()
        {
            var result =DTOModelSetters.SetGetArticleResponse();
            result.ShouldNotBeNull();
            result.Body.ShouldBe("value");
            result.Title.ShouldBe("value");
            result.Description.ShouldBe("value");
            result.DateAdded.ShouldBe(DateTime.Now.Date);
            result.ArticleUniqueIdentifier.ShouldBe(Guid.Empty);
            result.ArticleLikes.ShouldBe(0);

        }  
        
        [Fact]
        public void SuccessCreateUserResponse_Should_Return_Values_Set()
        {
            var result =DTOModelSetters.SetSuccessCreateUserResponse();
            result.ShouldNotBeNull();
            result.Email.ShouldBe("value");
            result.Username.ShouldBe("value");
            result.UserUniqueIdentity.ShouldBe(Guid.Empty);

        }
        
        [Fact]
        public void UserSuccessLoginResponse_Should_Return_Values_Set()
        {
            var result =DTOModelSetters.SetUserSuccessLoginResponse();
            result.ShouldNotBeNull();
            result.Username.ShouldBe("value");
            result.UserUniqueIdentity.ShouldBe(Guid.Empty);
            result.Email.ShouldBe("value");
            result.ArticleLiked.ShouldBe(new List<LikeDTO>());

        }

        [Fact]
        public void CreateArticleData_Should_Return_Values_Set()
        {
            var result = DTOModelSetters.SetCreateArticleUserData();
            result.ShouldNotBeNull();
            result.Body.ShouldBe("value");
            result.Title.ShouldBe("value");
        }



    }
}
