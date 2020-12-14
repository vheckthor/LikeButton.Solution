using System;
using System.Collections.Generic;
using System.Text;

namespace LikeButton.Core.DTOs
{
    public class LikeDTO
    {
        public Guid ArticleUniqueId { get; set; }
        public Guid LikerUniqueId { get; set; }
        public bool LikeStatus { get; set; }
    }
}
