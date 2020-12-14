using System;

namespace LikeButton.Core.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 10;
        public int PageNUmber { get; set; } = 1;
        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value >= MaxPageSize) ? MaxPageSize : value; }
        }
        public Guid UserId { get; set; } = default;
        public string OrderBy { get; set; }
    }
}
