using Xunit;

namespace ABP.Test.Events.Bus
{
    /// <summary>
    /// 继承测试
    /// </summary>
    public class Inheritance_Tests : EventBusTestBase
    {
        /// <summary>
        /// 应该为派生类处理事件
        /// </summary>
        [Fact]
        public void Should_Handle_Events_For_Derived_Classes()
        {
            var totalData = 0;

            EventBus.Register<MySimpleEventData>(eventData =>
            {
                totalData += eventData.Value;
                Assert.Equal(this, eventData.EventSource);
            });

            EventBus.Trigger(this, new MySimpleEventData(1));//应该直接处理注册类
            EventBus.Trigger(this, new MySimpleEventData(2));
            EventBus.Trigger(this, new MyDerivedEventData(3));//也应该处理派生类
            EventBus.Trigger(this, new MyDerivedEventData(4));

            Assert.Equal(10, totalData);
        }

        /// <summary>
        /// 注册的是派生类则不处理基类的事件
        /// </summary>
        [Fact]
        public void Should_Not_Handle_Events_For_Base_Classes()
        {
            var totalData = 0;

            EventBus.Register<MyDerivedEventData>(eventData =>
            {
                    totalData += eventData.Value;
                    Assert.Equal(this, eventData.EventSource);
            });

            EventBus.Trigger(this, new MySimpleEventData(1)); //不处理
            EventBus.Trigger(this, new MySimpleEventData(2)); 
            EventBus.Trigger(this, new MyDerivedEventData(3)); //处理
            EventBus.Trigger(this, new MyDerivedEventData(4)); 

            Assert.Equal(7, totalData);
        }
    }
}