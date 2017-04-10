using Xunit;
using Shouldly;
using Abp.Timing;

namespace ABP.Test.Extensions
{
    /// <summary>
    /// DateTimeRange测试
    /// </summary>
    public class DateTimeRange_Tests
    {
        [Fact]
        public void StaticRanges_Test()
        {
            var statr = DateTimeRange.Today.StartTime;
            var end = DateTimeRange.Yesterday.EndTime;
            DateTimeRange.Today.StartTime.ShouldBeGreaterThan(DateTimeRange.Yesterday.EndTime);
            DateTimeRange.Today.EndTime.ShouldBeLessThan(DateTimeRange.Tomorrow.StartTime);

            DateTimeRange.ThisMonth.StartTime.Day.ShouldBe(1);
            DateTimeRange.LastMonth.StartTime.Day.ShouldBe(1);
            DateTimeRange.NextMonth.StartTime.Day.ShouldBe(1);

            DateTimeRange.ThisMonth.StartTime.ShouldBeGreaterThan(DateTimeRange.LastMonth.EndTime);
            DateTimeRange.ThisMonth.EndTime.ShouldBeLessThan(DateTimeRange.NextMonth.EndTime);

            DateTimeRange.ThisYear.StartTime.Month.ShouldBe(1);
            DateTimeRange.ThisYear.StartTime.Day.ShouldBe(1);

            DateTimeRange.ThisYear.StartTime.ShouldBeGreaterThan(DateTimeRange.LastYear.EndTime);
            DateTimeRange.ThisYear.EndTime.ShouldBeLessThan(DateTimeRange.NextYear.StartTime);

            DateTimeRange.Last7DaysExceptToday.EndTime.ShouldBeLessThan(DateTimeRange.Today.StartTime);
            DateTimeRange.Last30DaysExceptToday.EndTime.ShouldBeLessThan(DateTimeRange.Today.StartTime);
        }
    }
}
