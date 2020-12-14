using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LikeButton.Core.DTOs.APIRequest
{
    public class LikeActionRequest
    {
        [Required]
        public bool ArticleLiked { get; set; }
    }
}
