using LikeButton.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeButton.Tests.Core.Helpers
{
    public class HelperModelSetters
    {
        public static PaginationHeader SetPaginationHeader()
        {
            var paged = new PaginationHeader(1, 2, 3, 4);

            return paged;
        }

        public static UserParams SetUserParams()
        {
            return new UserParams
            {
                OrderBy = "value",
                PageNUmber = 1,
                PageSize = 3,
                UserId = Guid.Empty
            };
        }
    }
}
