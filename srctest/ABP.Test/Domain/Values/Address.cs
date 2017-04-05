using Abp.Domain.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABP.Test.Domain.Values
{
    /// <summary>
    /// 地址1
    /// </summary>
    public class Address : ValueObject<Address>
    {
        /// <summary>
        /// 城市ID
        /// </summary>
        public Guid CityId { get; set; }

        /// <summary>
        /// 街道名称
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Address(Guid cityId, string street, int number)
        {
            CityId = cityId;
            Street = street;
            Number = number;
        }
    }

    /// <summary>
    /// 地址2
    /// </summary>
    public class Address2 : ValueObject<Address2>
    {
        /// <summary>
        /// 城市ID
        /// </summary>
        public Guid? CityId { get; set; }

        /// <summary>
        /// 街道名称
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Address2(Guid? cityId, string street, int number)
        {
            CityId = cityId;
            Street = street;
            Number = number;
        }
    }
}
