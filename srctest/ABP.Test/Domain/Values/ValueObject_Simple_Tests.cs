using System;
using Xunit;

namespace ABP.Test.Domain.Values
{
    /// <summary>
    /// 值对象简单测试
    /// </summary>
    public class ValueObject_Simple_Tests
    {
        /// <summary>
        /// 值对象如果值一样则必须相等
        /// </summary>
        [Fact]
        public void Value_Objects_Should_Be_Same_If_Contains_Same_Data()
        {
            var address1 = new Address(new Guid("21C67A65-ED5A-4512-AA29-66308FAAB5AF"), "曹阳六村", 110);
            var address2 = new Address(new Guid("21C67A65-ED5A-4512-AA29-66308FAAB5AF"), "曹阳六村", 110);

            Assert.Equal(address1, address2);
            Assert.Equal(address1.GetHashCode(), address2.GetHashCode());
            Assert.True(address1 == address2);
            Assert.False(address1 != address2);
        }

        /// <summary>
        /// 值对象如果值不一样则必须不相等
        /// </summary>
        [Fact]
        public void Value_Objects_Should_Not_Be_Same_If_Contains_Different_Data()
        {
            Assert.NotEqual(
                new Address(new Guid("21C67A65-ED5A-4512-AA29-66308FAAB5AF"), "曹阳六村", 110),
                new Address(new Guid("21C67A65-ED5A-4512-AA29-66308FAAB5A0"), "曹阳六村", 110)
            );

            Assert.NotEqual(
                new Address(new Guid("21C67A65-ED5A-4512-AA29-66308FAAB5AF"), "曹阳六村", 110),
                new Address(new Guid("21C67A65-ED5A-4512-AA29-66308FAAB5AF"), "曹阳六村1", 110)
            );

            Assert.NotEqual(
                new Address(new Guid("21C67A65-ED5A-4512-AA29-66308FAAB5AF"), "曹阳六村", 110),
                new Address(new Guid("21C67A65-ED5A-4512-AA29-66308FAAB5AF"), "曹阳六村", 111)
            );
        }

        /// <summary>
        /// 值对象不与Null对象相等
        /// </summary>
        [Fact]
        public void Value_Objects_Should_Not_Be_Same_If_One_Of_Them_Is_Null()
        {
            Assert.NotEqual(
                new Address(new Guid("21C67A65-ED5A-4512-AA29-66308FAAB5AF"), "曹阳六村", 110),
                null
            );

            Assert.True(new Address(new Guid("21C67A65-ED5A-4512-AA29-66308FAAB5AF"), "曹阳六村", 110) != null);
        }

        /// <summary>
        /// 值对象如果他们属性有为Null的则不想等
        /// </summary>
        [Fact]
        public void Value_Object_Nullable_Guid_Property_Test()
        {
            var anAddress = new Address2(new Guid("21C67A65-ED5A-4512-AA29-66308FAAB5AF"), "曹阳六村", 110);
            var anotherAddress = new Address2(null, "曹阳六村", 110);

            Assert.NotEqual(anAddress, anotherAddress);
            Assert.False(anotherAddress.Equals(anAddress));
            Assert.True(anAddress != anotherAddress);
        }
    }
}
