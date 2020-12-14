using LikeButton.Core.DTOs.APIRequest;
using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeButton.Core.Interfaces.IAuthenticationRepository
{
    public interface IAuthentication
    {
        Task<SuccessCreateUserResponse> RegisterAsync(CreateUserAPIRequest request);
        Task<UserSuccessLoginResponse> LoginAsync(UserLoginRequest request);
        Task<bool> UserExistsAsync(string username);
    }
}
