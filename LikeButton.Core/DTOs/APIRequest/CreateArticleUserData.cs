using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LikeButton.Core.DTOs.APIRequest
{
    public class CreateArticleUserData
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
