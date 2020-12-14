using System.Threading.Tasks;
using LikeButton.Core.DTOs.APIRequest;
using LikeButton.Core.Entities;
using LikeButton.Core.Interfaces;
using LikeButton.Core.Interfaces.IAuthenticationRepository;
using Microsoft.AspNetCore.Mvc;

namespace LikeButton.API.Controllers
{
    [Route("api/authentication/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _repo;
        private readonly ITokenProvider _provider;
        private IFileLogger _logger;

        public AuthenticationController(IAuthentication repo, ITokenProvider provider, IFileLogger mapper)
        {
            _provider = provider;

            _repo = repo;
            _logger = mapper;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserAPIRequest user)
        {               

                if (await _repo.UserExistsAsync(user.username.ToLower()))
                {
                    return BadRequest("Username already Exists");
                }


                var result = await _repo.RegisterAsync(user);
                if (result == null)
                {
                    return BadRequest("An error occured saving changes");
                }
                return CreatedAtRoute("GetUser", new { Controller = "Users", UserUniqueIdentity = result.UserUniqueIdentity }, result);


        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest user)
        {

            
                var logged = await _repo.LoginAsync(user);
                if (logged == null)
                {
                    return Unauthorized("You do not have access to login");
                }
                

               string jwtToken = _provider.GeneratedToken(
                    new User { 
                        UserUniqueIdentity = logged.UserUniqueIdentity,
                        Username = logged.Username,
                        Email = logged.Email
                
                    });

                return Ok(new
                {
                    token = jwtToken,
                    logged
                   
                });


        }

    }
}
