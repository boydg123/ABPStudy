using Abp.Domain.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Events.Bus.Entities;
using Xunit;
using Shouldly;
using System;

namespace Abp.Test.Events.Bus
{
    /// <summary>
    /// 事件总线，多处理测试
    /// </summary>
    public class EventBus_MultipleHandle_Tests : EventBusTestBase
    {
        public class MyEntity : Entity { }

        public class MyEventHandler : IEventHandler<EntityCreatedEventData<MyEntity>>, IEventHandler<EntityChangedEventData<MyEntity>>
        {
            public int EntityCreatedEventCount { get; set; }
            public int EntityChangedEventCount { get; set; }
            public void HandleEvent(EntityCreatedEventData<MyEntity> eventData)
            {
                EntityCreatedEventCount++;
            }

            public void HandleEvent(EntityChangedEventData<MyEntity> eventData)
            {
                EntityChangedEventCount++;
            }
        }

        /// <summary>
        /// 调用一次创建和更改
        /// </summary>
        [Fact]
        public void Should_Call_Created_And_Changed_Once()
        {
            var handler = new MyEventHandler();

            EventBus.Register<EntityCreatedEventData<MyEntity>>(handler);
            EventBus.Register<EntityChangedEventData<MyEntity>>(handler);

            EventBus.Trigger(new EntityCreatedEventData<MyEntity>(new MyEntity() { Id = 1 }));

            handler.EntityCreatedEventCount.ShouldBe(1);
            handler.EntityChangedEventCount.ShouldBe(1);
        }
    }
}
