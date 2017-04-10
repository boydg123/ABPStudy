using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Abp.Extensions;

namespace ABP.Test.Extensions
{
    /// <summary>
    /// 比较扩展测试
    /// </summary>
    public class ComparableExtensions_Tests
    {
        /// <summary>
        /// IsBetween测试
        /// </summary>
        [Fact]
        public void IsBetween_Tests()
        {
            var number = 5;
            number.IsBetween(0, 10).ShouldBe(true);
            number.IsBetween(4, 6).ShouldBe(true);
            number.IsBetween(1, 2).ShouldBe(false);

            var dateTimeValue = new DateTime(2017, 4, 10, 21, 42, 1);
            dateTimeValue.IsBetween(new DateTime(2017, 1, 1), new DateTime(2018, 1, 1)).ShouldBe(true);
            dateTimeValue.IsBetween(new DateTime(2017, 1, 1), new DateTime(2017, 2, 1)).ShouldBe(false);
        }
    }
}
