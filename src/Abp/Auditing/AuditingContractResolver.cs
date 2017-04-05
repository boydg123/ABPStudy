using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Abp.Auditing
{
    /// <summary>
    /// Decides which properties of auditing class to be serialized
    /// 决定哪些属性在审计类中将被序列化
    /// </summary>
    public class AuditingContractResolver : CamelCasePropertyNamesContractResolver
    {
        /// <summary>
        /// 将JSON属性映射到.NET成员或构造函数参数
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <param name="memberSerialization">成员序列化</param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (member.IsDefined(typeof(DisableAuditingAttribute)) || member.IsDefined(typeof(JsonIgnoreAttribute)))
            {
                property.ShouldSerialize = instance => false;
            }

            return property;
        }
    }
}
