using System;
using System.Collections.Generic;
using System.Text;

namespace LikeButton.Core.DTOs.APIResponse
{
    public class GetArticleResponse
    {
        public Guid ArticleUniqueIdentifier { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime DateAdded { get; set; }
        public long ArticleLikes { get; set; }
    }
}
