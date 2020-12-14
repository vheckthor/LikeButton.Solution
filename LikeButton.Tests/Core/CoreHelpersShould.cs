using LikeButton.Tests.Core.Helpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LikeButton.Tests.Core
{
    public class CoreHelpersShould
    {
         [Fact]
        public void PaginationHeader_Should_Return_Values_Set()
        {
            var result =HelperModelSetters.SetPaginationHeader();
            result.ShouldNotBeNull();

        }
        
        [Fact]
        public void UserParams_Should_Return_Values_Set()
        {
            var result = HelperModelSetters.SetUserParams();
            result.ShouldNotBeNull();
            result.OrderBy.ShouldBe("value");
            result.PageNUmber.ShouldBe(1);
            result.PageSize.ShouldBe(3);
            result.UserId.ShouldBe(Guid.Empty);
        }
    }
}
