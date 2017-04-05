using System.Collections.Generic;
using System.Reflection;

namespace Abp.EntityFramework.Utils
{
    /// <summary>
    /// 实体时间属性信息
    /// </summary>
    public class EntityDateTimePropertiesInfo
    {
        /// <summary>
        /// 属性信息集合
        /// </summary>
        public List<PropertyInfo> DateTimePropertyInfos { get; set; }

        /// <summary>
        /// 复合型属性路径
        /// </summary>
        public List<string> ComplexTypePropertyPaths { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EntityDateTimePropertiesInfo()
        {
            DateTimePropertyInfos = new List<PropertyInfo>();
            ComplexTypePropertyPaths = new List<string>();
        }
    }
}