using LikeButton.Core.Interfaces;
using LikeButton.API.Controllers;
using Moq;
using LikeButton.Core.Interfaces.IUserRepository;

namespace LikeButton.Tests.API.Unit.UsersControllr
{
    public class BaseControllerTest
    {
        protected Mock<IFileLogger> fileLogerMock;
        protected Mock<IUserRepository> userManagerMock;
        protected UsersController controller;

        protected BaseControllerTest()
        {
            fileLogerMock = new Mock<IFileLogger>();
            userManagerMock = new Mock<IUserRepository>();
          
            
            controller = new UsersController(userManagerMock.Object,fileLogerMock.Object);

        }
    }
}
