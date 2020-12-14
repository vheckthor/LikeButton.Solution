using LikeButton.API.Controllers;
using LikeButton.Core.Interfaces;
using LikeButton.Infrastructure.Data.Repository.ArticleRepository;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeButton.Tests.API.Unit.ArticleControllr
{
    public class BaseControllerTest
    {
        
        protected Mock<IArticleRepository> articleManagerMock;
       
        protected ArticleController controller;

        protected BaseControllerTest()
        {
            
            articleManagerMock = new Mock<IArticleRepository>();


            controller = new ArticleController(articleManagerMock.Object);

        }
    }
}
