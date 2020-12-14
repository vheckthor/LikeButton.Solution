using LikeButton.Core.DTOs;
using LikeButton.Core.DTOs.APIRequest;
using LikeButton.Core.DTOs.APIResponse;
using LikeButton.Core.Entities;
using LikeButton.Core.Interfaces;
using LikeButton.Core.Interfaces.IAuthenticationRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LikeButton.Infrastructure.Data.Repository
{
    public class AuthenticationRepository:IAuthentication
    {
        private readonly AppDbContext _context;
        private readonly IFileLogger _logger;

        public AuthenticationRepository(AppDbContext context, IFileLogger logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<UserSuccessLoginResponse> LoginAsync(UserLoginRequest request)
        {
            _logger.LogInfo(request.username + $" Attempts Login @{DateTime.Now}", "LoginAsync");

            try
            {
                var finder = await _context.Users.Include(x => x.ArticleLiked).FirstOrDefaultAsync(x => x.Username == request.username);
                if (finder == null)
                {
                    _logger.LogInfo(request.username + " Login Denied reason: username does not exist", "LoginAsync");
                    return null;
                }
                if (!VerifyPasswordHash(request.password, finder.PasswordHash, finder.PasswordSalt))
                {
                    _logger.LogInfo(request.username + " Login Denied reason: password incorrect", "LoginAsync");

                    return null;

                }
                List<LikeDTO> likes = new List<LikeDTO>();
                
                foreach(var like in finder.ArticleLiked)
                {
                    if (like.LikeStatus)
                    {
                        var current = new LikeDTO
                        {
                            ArticleUniqueId = like.ArticleUniqueId,
                            LikerUniqueId = like.LikerUniqueId,
                            LikeStatus = like.LikeStatus
                        };
                        
                        likes.Add(current);
                    }
                    else
                    {
                        continue;
                    }


                    
                }
                var result = new UserSuccessLoginResponse
                {
                    ArticleLiked = likes,
                    Email = finder.Email,
                    Username = finder.Username,
                    UserUniqueIdentity = finder.UserUniqueIdentity
                };
                _logger.LogInfo(request.username + " Login Successful", "LoginAsync");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException.Message, "  AuthenticationRepositoryLoginAsync");
                throw;
            }



        }

        public async Task<bool> UserExistsAsync(string username)
        {

            try
            {
                if (await _context.Users.AnyAsync(x => x.Username == username))
                {
                    _logger.LogInfo(username + " already exists", "UserExistsAsync");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException.Message, "  AuthenticationRepositoryUserExistsAsync");
                throw ex;
            }


        }

        public async Task<SuccessCreateUserResponse> RegisterAsync(CreateUserAPIRequest request)
        {
            try
            {
                _logger.LogInfo(request.username + " Creating user...", "RegisterAsync");
                byte[] passwordhash, passwordSalt;
                CreatePasswordHash(request.password, out passwordhash, out passwordSalt);

                var user = new User
                {
                    PasswordHash = passwordhash,
                    PasswordSalt = passwordSalt,
                    Email = request.Email,
                    Username = request.username,
                    UserUniqueIdentity = Guid.NewGuid()
                };
                await _context.Users.AddAsync(user);
                var success = await _context.SaveChangesAsync() > 0;
                if (success)
                {
                    _logger.LogInfo(request.username + " Created Successfully", "RegisterAsync");
                    return new SuccessCreateUserResponse
                    {
                        Email = user.Email,
                        Username = user.Username,
                        UserUniqueIdentity = user.UserUniqueIdentity
                    };

                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException?.Message, "  AuthenticationRepositoryRegisterAsync");

                return default;
            }


        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var haser = new HMACSHA512(passwordSalt))
            {
                var ComputedpasswordHash = haser.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < ComputedpasswordHash.Length; i++)
                {
                    if (ComputedpasswordHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

            }
            return true;

        }

        private void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordSalt)
        {
            using (var haser = new HMACSHA512())
            {
                passwordSalt = haser.Key;
                passwordhash = haser.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }



    }
}
