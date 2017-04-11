using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Abp.Json;
using Abp.Timing;

namespace ABP.Test.Json
{
    public class JsonSerializationHelper_Tests
    {
        [Fact]
        public void Test_1()
        {
            var str = JsonSerializationHelper.SerializeWithType(new MyClass1("Derrick"));
            var obj = JsonSerializationHelper.DeserializeWithType(str) as MyClass1;
            obj.ShouldNotBe(null);
            obj.Name.ShouldBe("Derrick");
        }

        [Fact]
        public void Test_2()
        {
            Clock.Provider = ClockProviders.Utc;
            var class2 = JsonSerializationHelper.SerializeWithType(new MyClass2(new DateTime(2017, 4, 11, 11, 11, 11)));
            var dataStr = "ABP.Test.Json.JsonSerializationHelper_Tests+MyClass2, ABP.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null|{\"Date\":\"2017 - 04 - 11T11: 11:11Z\"}";
            var result = (MyClass2)JsonSerializationHelper.DeserializeWithType(dataStr);
            result.ShouldNotBe(null);
            result.Date.ShouldBe(new DateTime(2017, 4, 11, 11, 11, 11, Clock.Kind));
            result.Date.Kind.ShouldBe(Clock.Kind);
        }

        [Fact]
        public void Test_3()
        {
            Clock.Provider = ClockProviders.Local;

            var myClass = new MyClass2(new DateTime(2016, 04, 13, 08, 58, 10, 526, Clock.Kind));
            var str = JsonSerializationHelper.SerializeWithType(myClass);
            var result = (MyClass2)JsonSerializationHelper.DeserializeWithType(str);

            result.Date.ShouldBe(new DateTime(2016, 04, 13, 08, 58, 10, 526, Clock.Kind));
            result.Date.Kind.ShouldBe(Clock.Kind);
        }

        public class MyClass1
        {
            public string Name { get; set; }
            public MyClass1()
            {

            }

            public MyClass1(string name)
            {
                Name = name;
            }
        }

        public class MyClass2
        {
            public DateTime Date { get; set; }
            public MyClass2(DateTime date)
            {
                Date = date;
            }
        }
    }
}
