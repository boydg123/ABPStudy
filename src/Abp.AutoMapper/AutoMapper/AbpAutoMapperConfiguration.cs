using System;
using System.Collections.Generic;
using AutoMapper;

namespace Abp.AutoMapper
{
    /// <summary>
    /// <see cref="IAbpAutoMapperConfiguration"/>基本实现
    /// </summary>
    public class AbpAutoMapperConfiguration : IAbpAutoMapperConfiguration
    {
        /// <summary>
        /// 映射器配置列表
        /// </summary>
        public List<Action<IMapperConfigurationExpression>> Configurators { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AbpAutoMapperConfiguration()
        {
            Configurators = new List<Action<IMapperConfigurationExpression>>();
        }
    }
}