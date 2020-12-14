using System;
using System.Collections.Generic;
using System.Text;

namespace LikeButton.Core.DTOs.APIResponse
{
    public class GetUserResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid UserUniqueIdentity { get; set; }
    }
}
