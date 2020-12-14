using System;
using System.Collections.Generic;
using System.Text;
using LikeButton.Core.Interfaces;
using LikeButton.API.Controllers;
using Microsoft.Extensions.Configuration;
using Moq;
using LikeButton.Core.Interfaces.IAuthenticationRepository;

namespace LikeButton.Tests.API.Unit.AuthenticationControllr
{
    public class BaseControllerTest
    {
        protected Mock<IFileLogger> fileLogerMock;
        protected Mock<IAuthentication> authenticationManagerMock;
        protected Mock<ITokenProvider> tokenManagerMock;
        protected Mock<IConfiguration>configMock;
        protected AuthenticationController controller;

        protected BaseControllerTest()
        {
            fileLogerMock = new Mock<IFileLogger>();
            authenticationManagerMock = new Mock<IAuthentication>();
            tokenManagerMock = new Mock<ITokenProvider>();
            configMock = new Mock<IConfiguration>();

            
            controller = new AuthenticationController(authenticationManagerMock.Object,tokenManagerMock.Object,fileLogerMock.Object);

        }
    }
}
