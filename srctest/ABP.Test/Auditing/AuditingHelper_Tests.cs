using Abp.Auditing;
using Xunit;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABP.Test.Auditing
{
    /// <summary>
    /// 审计帮助类测试
    /// </summary>
    public class AuditingHelper_Tests
    {
        /// <summary>
        /// 审计帮助测试 - 人员实体Dto
        /// </summary>
        public class AuditingHelperTestPersonDto
        {
            public string FullName { get; set; }
            [DisableAuditing]
            public int Age { get; set; }
            public AuditingHelperTestSchoolDto School { get; set; }
        }

        /// <summary>
        /// 审计帮助测试 - 学校实体Dto
        /// </summary>
        public class AuditingHelperTestSchoolDto
        {
            public string Name { get; set; }
            [DisableAuditing]
            public string Address { get; set; }
        }

        /// <summary>
        /// 忽略属性不应该被序列化
        /// </summary>
        [Fact]
        public void Ignored_Properties_Should_Not_Be_Serialized()
        {
            var json = AuditingHelper.Serialize(new AuditingHelperTestPersonDto()
            {
                FullName = "Derrick Dou",
                Age = 28,
                School = new AuditingHelperTestSchoolDto()
                {
                    Name = "大幕小学",
                    Address = "咸宁大幕"
                }
            });

            json.ShouldBe("{\"fullName\":\"Derrick Dou\",\"school\":{\"name\":\"大幕小学\"}}");
        }
    }
}
