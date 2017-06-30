using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json.Serialization;

namespace Abp.Web.SignalR
{
    /// <summary>
    /// Uses CamelCasePropertyNamesContractResolver instead of DefaultContractResolver for SignalR communication. 
    /// 为SignalR通信使用驼峰命名规则解析代替默认规则解析
    /// </summary>
    public class AbpSignalRContractResolver : IContractResolver
    {
        /// <summary>
        /// List of ignored assemblies.It contains only the SignalR's own assembly.
        /// 忽略的程序集列表，它只包含SignalR自己的程序集
        /// If you don't want that your assembly's types are automatically camel cased while sending to the client, then add it to this list.
        /// 如果你不希望你的程序集类型在发送到客户机时自动装箱，则将其添加到该列表中。
        /// </summary>
        public static List<Assembly> IgnoredAssemblies { get; private set; }

        /// <summary>
        /// 驼峰命名规则解析器
        /// </summary>
        private readonly IContractResolver _camelCaseContractResolver;

        /// <summary>
        /// 默认契约解析器
        /// </summary>
        private readonly IContractResolver _defaultContractSerializer;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static AbpSignalRContractResolver()
        {
            IgnoredAssemblies = new List<Assembly>
            {
                typeof (Connection).Assembly
            };
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AbpSignalRContractResolver()
        {
            _defaultContractSerializer = new DefaultContractResolver();
            _camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        /// <summary>
        /// 解析类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonContract ResolveContract(Type type)
        {
            if (IgnoredAssemblies.Contains(type.Assembly))
            {
                return _defaultContractSerializer.ResolveContract(type);
            }

            return _camelCaseContractResolver.ResolveContract(type);
        }
    }
}
