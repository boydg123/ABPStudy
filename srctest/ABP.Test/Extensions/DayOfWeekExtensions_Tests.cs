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
    /// DayOfWeek测试
    /// </summary>
    public class DayOfWeekExtensions_Tests
    {
        [Fact]
        public void Weekend_Weekday_Test()
        {
            DayOfWeek.Monday.IsWeekday().ShouldBe(true);
            DayOfWeek.Monday.IsWeekend().ShouldBe(false);

            DayOfWeek.Saturday.IsWeekend().ShouldBe(true);
            DayOfWeek.Saturday.IsWeekday().ShouldBe(false);

            var datetime1 = new DateTime(2017, 4, 10, 12, 12, 12); //周一
            var datetime2 = new DateTime(2017, 4, 9, 12, 12, 12); //周日

            datetime1.DayOfWeek.IsWeekend().ShouldBe(false);
            datetime2.DayOfWeek.IsWeekend().ShouldBe(true);

            datetime1.DayOfWeek.IsWeekday().ShouldBe(true);
            datetime2.DayOfWeek.IsWeekday().ShouldBe(false);
        }
    }
}
