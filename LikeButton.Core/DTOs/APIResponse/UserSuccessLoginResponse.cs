using LikeButton.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeButton.Core.DTOs.APIResponse
{
    public class UserSuccessLoginResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid UserUniqueIdentity { get; set; }
        public ICollection<LikeDTO> ArticleLiked { get; set; }
    }
}
