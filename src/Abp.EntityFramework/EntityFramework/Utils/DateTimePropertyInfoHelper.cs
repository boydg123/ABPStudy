using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Abp.Reflection;
using Abp.Timing;

namespace Abp.EntityFramework.Utils
{
    /// <summary>
    /// <see cref="DateTime"/>属性信息帮助类
    /// </summary>
    public static class DateTimePropertyInfoHelper
    {
        /// <summary>
        /// Key: Entity type. Value: DateTime property infos
        /// DateTime属性字典。Key：实体类型。 值：DateTime属性信息
        /// </summary>
        private static readonly ConcurrentDictionary<Type, EntityDateTimePropertiesInfo> DateTimeProperties;

        /// <summary>
        /// 构造函数
        /// </summary>
        static DateTimePropertyInfoHelper()
        {
            DateTimeProperties = new ConcurrentDictionary<Type, EntityDateTimePropertiesInfo>();
        }

        /// <summary>
        /// 获取时间属性信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>时间属性信息</returns>
        public static EntityDateTimePropertiesInfo GetDatePropertyInfos(Type entityType)
        {
            return DateTimeProperties.GetOrAdd(
                       entityType,
                       key => FindDatePropertyInfosForType(entityType)
                   );
        }

        /// <summary>
        /// 规范日期属性类型
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="entityType">实体类型</param>
        public static void NormalizeDatePropertyKinds(object entity, Type entityType)
        {
            var dateTimePropertyInfos = GetDatePropertyInfos(entityType);

            dateTimePropertyInfos.DateTimePropertyInfos.ForEach(property =>
            {
                var dateTime = (DateTime?)property.GetValue(entity);
                if (dateTime.HasValue)
                {
                    property.SetValue(entity, Clock.Normalize(dateTime.Value));
                }
            });

            dateTimePropertyInfos.ComplexTypePropertyPaths.ForEach(propertPath =>
            {
                var dateTime = (DateTime?)ReflectionHelper.GetValueByPath(entity, entityType, propertPath);
                if (dateTime.HasValue)
                {
                    ReflectionHelper.SetValueByPath(entity, entityType, propertPath, Clock.Normalize(dateTime.Value));
                }
            });
        }

        /// <summary>
        /// 从类型中查找日期属性信息
        /// </summary>
        /// <param name="entityType">类型</param>
        /// <returns>实体时间属性信息</returns>
        private static EntityDateTimePropertiesInfo FindDatePropertyInfosForType(Type entityType)
        {
            var datetimeProperties = entityType.GetProperties()
                                     .Where(property =>
                                         (property.PropertyType == typeof(DateTime) ||
                                         property.PropertyType == typeof(DateTime?)) &&
                                         property.CanWrite
                                     ).ToList();

            var complexTypeProperties = entityType.GetProperties()
                                                   .Where(p => p.PropertyType.IsDefined(typeof(ComplexTypeAttribute), true))
                                                   .ToList();

            var complexTypeDateTimePropertyPaths = new List<string>();
            foreach (var complexTypeProperty in complexTypeProperties)
            {
                AddComplexTypeDateTimePropertyPaths(entityType.FullName + "." + complexTypeProperty.Name, complexTypeProperty, complexTypeDateTimePropertyPaths);
            }

            return new EntityDateTimePropertiesInfo
            {
                DateTimePropertyInfos = datetimeProperties,
                ComplexTypePropertyPaths = complexTypeDateTimePropertyPaths
            };
        }

        /// <summary>
        /// 添加复合型时间属性路径
        /// </summary>
        /// <param name="pathPrefix">路径前缀</param>
        /// <param name="complexProperty">复合型属性信息</param>
        /// <param name="complexTypeDateTimePropertyPaths">复合型属性路径集合</param>
        private static void AddComplexTypeDateTimePropertyPaths(string pathPrefix, PropertyInfo complexProperty, List<string> complexTypeDateTimePropertyPaths)
        {
            if (!complexProperty.PropertyType.IsDefined(typeof(ComplexTypeAttribute), true))
            {
                return;
            }

            var complexTypeDateProperties = complexProperty.PropertyType
                                                            .GetProperties()
                                                            .Where(property =>
                                                                property.PropertyType == typeof(DateTime) ||
                                                                property.PropertyType == typeof(DateTime?)
                                                            ).Select(p => pathPrefix + "." + p.Name).ToList();

            complexTypeDateTimePropertyPaths.AddRange(complexTypeDateProperties);

            var complexTypeProperties = complexProperty.PropertyType.GetProperties()
                                                  .Where(p => p.PropertyType.IsDefined(typeof(ComplexTypeAttribute), true))
                                                  .ToList();

            if (!complexTypeProperties.Any())
            {
                return;
            }

            foreach (var complexTypeProperty in complexTypeProperties)
            {
                AddComplexTypeDateTimePropertyPaths(pathPrefix + "." + complexTypeProperty.Name, complexTypeProperty, complexTypeDateTimePropertyPaths);
            }
        }
    }
}
