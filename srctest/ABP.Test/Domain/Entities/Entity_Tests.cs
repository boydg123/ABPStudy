using Xunit;
using Shouldly;

namespace Abp.Test.Domain.Entities
{
    /// <summary>
    /// 实体测试
    /// </summary>
    public class Entity_Tests
    {
        /// <summary>
        /// 相等操作符测试
        /// </summary>
        [Fact]
        public void Equality_Operator_Works()
        {
            var w1 = new Worker() { Id = 1, Name = "Derrick" };
            var w2 = new Worker() { Id = 1, Name = "Derrick1" };
            //相同ID的实体必须相等
            Assert.True(w1 == w2, "相同ID的实体必须相等");
            Assert.True(w1.Equals(w2), "相同ID的实体必须相等");

            Worker w3 = null;
            Worker w4 = null;
            //相同实体的Null对象必须相等
            Assert.True(w3 == w4, "相同实体的Null对象必须相等");

            var m1 = new Manager { Id = 1, Name = "Derrick2", Title = "软件工程师" };
            //类与该类的派生类必须相等，如果他们有相同的ID
            Assert.True(w1 == m1, "类与该类的派生类必须相等，如果他们有相同的ID");

            var d1 = new Department() { Id = 1, Name = "ES" };
            //不同类对象必须不相等，如果他们有相同的ID
            Assert.False(d1 == w1, "不同类对象必须不相等，如果他们有相同的ID");

            var w5 = w1;
            w5.Id = 2;
            //相同类的实例必须相等
            Assert.True(w5 == w1, "相同类的实例必须相等");
            w1.Id.ShouldBe(2);
        }

        /// <summary>
        /// 是否为临时实体测试(如果没有ID则为临时实体)
        /// </summary>
        [Fact]
        public void IsTransinet_Works()
        {
            var w1 = new Worker() { Name = "Derrick" };
            var w2 = new Worker() { Id = 1, Name = "Drrick" };

            Assert.True(w1.IsTransient());
            Assert.False(w2.IsTransient());
        }
    }
}
