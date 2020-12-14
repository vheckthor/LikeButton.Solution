using System;
using System.Collections.Generic;
using System.Text;
using LikeButton.Core.Interfaces;
using LikeButton.API.Controllers;
using Microsoft.Extensions.Configuration;
using Moq;
using LikeButton.Core.Interfaces.IAuthenticationRepository;
using LikeButton.Core.Interfaces.ILikeRepository;
using LikeButton.Infrastructure.Data.Repository.ArticleRepository;
using LikeButton.Core.Interfaces.IUserRepository;

namespace LikeButton.Tests.API.Unit.LikeArticleControllr
{
    public class BaseControllerTest
    {
        protected Mock<ILikeRepository> likeManagerMock;
        protected Mock<IUserRepository> userManagerMock;
        protected Mock<IArticleRepository>articleMock;
        protected Mock<IClaim> claimMock;
        protected LikeArticleController controller;

        protected BaseControllerTest()
        {

            likeManagerMock = new Mock<ILikeRepository>();
            userManagerMock = new Mock<IUserRepository>();
            articleMock = new Mock<IArticleRepository>();
            claimMock = new Mock<IClaim>();
            
            controller = new LikeArticleController(articleMock.Object,userManagerMock.Object,likeManagerMock.Object,claimMock.Object);

        }
    }
}
