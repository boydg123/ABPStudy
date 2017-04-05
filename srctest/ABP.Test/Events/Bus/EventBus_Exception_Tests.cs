using Xunit;
using Shouldly;
using System;

namespace ABP.Test.Events.Bus
{
    /// <summary>
    /// 事件总线异常测试
    /// </summary>
    public class EventBus_Exception_Tests : EventBusTestBase
    {
        /// <summary>
        /// 如果只有一个处理失败，则抛出单个异常
        /// </summary>
        [Fact]
        public void Should_Throw_Single_Exception_If_Only_One_Of_Handlers_Fails()
        {
            EventBus.Register<MySimpleEventData>(eventData =>
            {
                throw new ApplicationException("这个是故意抛出的异常！");
            });

            var appException = Assert.Throws<ApplicationException>(() =>
            {
                EventBus.Trigger<MySimpleEventData>(null, new MySimpleEventData(1));
            });

            appException.Message.ShouldBe("这个是故意抛出的异常！");
        }

        /// <summary>
        /// 如果多个处理失败，则抛出聚合异常
        /// </summary>
        [Fact]
        public void Should_Throw_Aggregate_Exception_If_More_Than_One_Of_Handlers_Fail()
        {
            EventBus.Register<MySimpleEventData>(
            eventData =>
            {
                throw new ApplicationException("这个是故意抛出的异常！- 1");
            });

            EventBus.Register<MySimpleEventData>(
            eventData =>
            {
                throw new ApplicationException("这个是故意抛出的异常！- 2");
            });

            var aggrException = Assert.Throws<AggregateException>(() =>
            {
                EventBus.Trigger<MySimpleEventData>(null, new MySimpleEventData(1));
            });

            aggrException.InnerExceptions.Count.ShouldBe(2);
            aggrException.InnerExceptions[0].Message.ShouldBe("这个是故意抛出的异常！- 1");
            aggrException.InnerExceptions[1].Message.ShouldBe("这个是故意抛出的异常！- 2");
        }
    }
}
