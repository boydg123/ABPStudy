using System;
using Xunit;
using Abp.Collections;

namespace Abp.Test.Collections
{
    public class TypeList_Tests
    {
        public interface IMyInterface { }
        public class MyClass1 : IMyInterface { }
        public class MyClass2 : IMyInterface { }
        public class MyClass3 { }

        [Fact]
        public void Should_Only_Add_True_Types()
        {
            var list = new TypeList<IMyInterface>();
            list.Add<MyClass1>();
            list.Add(typeof(MyClass2));
            Assert.Throws<ArgumentException>(() => list.Add(typeof(MyClass3)));
        }
    }
}
