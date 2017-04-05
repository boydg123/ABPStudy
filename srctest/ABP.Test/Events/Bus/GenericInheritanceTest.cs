using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Abp.Domain.Entities;
using Abp.Events.Bus.Entities;

namespace ABP.Test.Events.Bus
{
    /// <summary>
    /// 通用继承测试
    /// </summary>
    public class GenericInheritanceTest : EventBusTestBase
    {
        public class Person : Entity { }
        public class Student : Person { }

        /// <summary>
        /// 通用继承事件数据类触发1
        /// </summary>
        [Fact]
        public void Should_Trigger_For_Inherited_Generic_1()
        {
            var triggeredEvent = false;
            //当传递的实体Person创建/修改/删除/的时候会触发方法
            EventBus.Register<EntityChangedEventData<Person>>(eventData =>
            {
                eventData.Entity.Id.ShouldBe(1);
                triggeredEvent = true;
            });

            EventBus.Trigger(new EntityChangedEventData<Person>(new Person() { Id = 1 }));

            triggeredEvent.ShouldBe(true);
        }

        /// <summary>
        /// 通用继承事件数据类触发2
        /// </summary>
        [Fact]
        public void Should_Trigger_For_Inherited_Generic_2()
        {
            var triggeredEvent = false;
            //当传递的实体Person创建/修改/删除/的时候会触发方法
            EventBus.Register<EntityChangedEventData<Person>>(eventData =>
            {
                eventData.Entity.Id.ShouldBe(1);
                triggeredEvent = true;
            });

            EventBus.Trigger(new EntityChangedEventData<Person>(new Student() { Id = 1 }));

            triggeredEvent.ShouldBe(true);
        }
    }
}
