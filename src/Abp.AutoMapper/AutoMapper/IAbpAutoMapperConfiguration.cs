using System;
using System.Collections.Generic;
using AutoMapper;

namespace Abp.AutoMapper
{
    /// <summary>
    /// Abp AutoMapper配置接口
    /// </summary>
    public interface IAbpAutoMapperConfiguration
    {
        List<Action<IMapperConfigurationExpression>> Configurators { get; }
    }
}