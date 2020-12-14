using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LikeButton.Core.DTOs.APIRequest
{
    public class CreateUserAPIRequest
    {
        [Required]
        public string username { get; set; }
       
        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "You must specify password between 6 to 20 characters")]
        public string password { get; set; }
       
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
