using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Abp.EntityFramework.Extensions
{
    //TODO: MOVE TO ABP
    //TODO: We can create simpler extension methods to create indexes
    //TODO: Check https://github.com/mj1856/EntityFramework.IndexingExtensions for example
    /// <summary>
    /// EF Model构建器扩展
    /// </summary>
    internal static class EntityFrameworkModelBuilderExtensions
    {
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="propertyConfiguration">原属性配置</param>
        /// <returns></returns>
        public static PrimitivePropertyConfiguration CreateIndex(this PrimitivePropertyConfiguration propertyConfiguration)
        {
            return propertyConfiguration.HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(
                    new IndexAttribute()
                    )
                );
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="propertyConfiguration">原属性配置</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static PrimitivePropertyConfiguration CreateIndex(this PrimitivePropertyConfiguration propertyConfiguration, string name)
        {
            return propertyConfiguration.HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(
                    new IndexAttribute(name)
                    )
                );
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="propertyConfiguration">原属性配置</param>
        /// <param name="name">名称</param>
        /// <param name="order">排序号</param>
        /// <returns></returns>
        public static PrimitivePropertyConfiguration CreateIndex(this PrimitivePropertyConfiguration propertyConfiguration, string name, int order)
        {
            return propertyConfiguration.HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(
                    new IndexAttribute(name, order)
                    )
                );
        }
    }
}