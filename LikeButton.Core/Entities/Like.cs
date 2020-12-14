using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LikeButton.Core.Entities
{
    public class Like
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public Guid ArticleUniqueId { get; set; }
        public Guid LikerUniqueId { get; set; }
        public bool LikeStatus { get; set; }
    }
}
