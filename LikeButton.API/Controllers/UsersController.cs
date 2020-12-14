using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Interfaces;
using LikeButton.Core.Interfaces.IUserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LikeButton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
       // private readonly ITokenProvider _provider;
        private IFileLogger _logger;

        public UsersController(IUserRepository repo, IFileLogger logger)
        {
            _repo = repo;
            _logger = logger;

        }

        [HttpGet("{UserUniqueIdentity}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(Guid UserUniqueIdentity)
        {
            _logger.LogInfo("Endpoint called", "GetUser");

            var user = await _repo.GetUserAsync(UserUniqueIdentity);

            if(user == null)
            {
                return NotFound("User not found");
            }
            var response = new GetUserResponse
            {
                    Email = user.Email,
                    Username = user.Username,
                    UserUniqueIdentity = user.UserUniqueIdentity
            };
            return Ok(response);
        }

    }
}
