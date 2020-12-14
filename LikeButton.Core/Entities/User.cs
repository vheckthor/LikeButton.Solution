using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LikeButton.Core.Entities
{
    public class User
    {
        //users information more fileds can be added as required
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid UserUniqueIdentity { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        [JsonIgnore]
        public ICollection<Like> ArticleLiked { get; set; }
    }
}
