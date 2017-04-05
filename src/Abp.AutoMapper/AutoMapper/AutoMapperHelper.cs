using System;
using System.Reflection;
using Abp.Collections.Extensions;
using AutoMapper;

namespace Abp.AutoMapper
{
    /// <summary>
    /// 映射器帮助类
    /// </summary>
    public static class AutoMapperHelper
    {
        /// <summary>
        /// 创建Abp特性映射
        /// </summary>
        /// <param name="configuration">映射器配置</param>
        /// <param name="type">类型</param>
        public static void CreateAbpAttributeMaps(this IMapperConfigurationExpression configuration, Type type)
        {
            configuration.CreateAbpAttributeMap<AutoMapFromAttribute>(type);
            configuration.CreateAbpAttributeMap<AutoMapToAttribute>(type);
            configuration.CreateAbpAttributeMap<AutoMapAttribute>(type);
        }

        /// <summary>
        /// 创建Abp特性映射
        /// </summary>
        /// <typeparam name="TAttribute">特性类型</typeparam>
        /// <param name="configuration">映射器配置</param>
        /// <param name="type">类型</param>
        private static void CreateAbpAttributeMap<TAttribute>(this IMapperConfigurationExpression configuration, Type type)
            where TAttribute : AutoMapAttribute
        {
            if (!type.IsDefined(typeof(TAttribute)))
            {
                return;
            }

            foreach (var autoMapToAttribute in type.GetCustomAttributes<TAttribute>())
            {
                if (autoMapToAttribute.TargetTypes.IsNullOrEmpty())
                {
                    continue;
                }

                foreach (var targetType in autoMapToAttribute.TargetTypes)
                {
                    if (autoMapToAttribute.Direction.HasFlag(AutoMapDirection.To))
                    {
                        configuration.CreateMap(type, targetType);
                    }

                    if (autoMapToAttribute.Direction.HasFlag(AutoMapDirection.From))
                    {
                        configuration.CreateMap(targetType, type);
                    }
                }
            }
        }
    }
}