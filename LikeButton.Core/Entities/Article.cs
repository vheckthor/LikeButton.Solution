using System;
using System.Collections.Generic;
using System.Text;

namespace LikeButton.Core.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public Guid ArticleUniqueIdentifier { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
  
    }
}
