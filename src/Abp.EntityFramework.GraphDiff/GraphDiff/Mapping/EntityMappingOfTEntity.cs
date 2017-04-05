using System;

namespace Abp.EntityFramework.GraphDiff.Mapping
{
    /// <summary>
    /// 实体映射
    /// </summary>
    public class EntityMapping
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType;

        /// <summary>
        /// 映射表达式
        /// </summary>
        public object MappingExpression { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="mappingExpression">映射表达式</param>
        public EntityMapping(Type type, object mappingExpression)
        {
            EntityType = type;
            MappingExpression = mappingExpression;
        }
    }
}