using System.Collections.Concurrent;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Json;
using Abp.Web.Api.Modeling;
using Abp.Web.Api.ProxyScripting.Configuration;
using Abp.Web.Api.ProxyScripting.Generators;

namespace Abp.Web.Api.ProxyScripting
{
    /// <summary>
    /// API异步脚本管理器
    /// </summary>
    public class ApiProxyScriptManager : IApiProxyScriptManager, ISingletonDependency
    {
        /// <summary>
        /// API描述模型提供者
        /// </summary>
        private readonly IApiDescriptionModelProvider _modelProvider;

        /// <summary>
        /// API代理脚本配置
        /// </summary>
        private readonly IApiProxyScriptingConfiguration _configuration;

        /// <summary>
        /// Ioc解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// 缓存字典
        /// </summary>
        private readonly ConcurrentDictionary<string, string> _cache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="modelProvider">API描述模型提供者</param>
        /// <param name="configuration">API代理脚本配置</param>
        /// <param name="iocResolver">Ioc解析器</param>
        public ApiProxyScriptManager(
            IApiDescriptionModelProvider modelProvider, 
            IApiProxyScriptingConfiguration configuration,
            IIocResolver iocResolver)
        {
            _modelProvider = modelProvider;
            _configuration = configuration;
            _iocResolver = iocResolver;

            _cache = new ConcurrentDictionary<string, string>();
        }

        /// <summary>
        /// 获取API代理脚本
        /// </summary>
        /// <param name="options">API代理生成选项</param>
        /// <returns></returns>
        public string GetScript(ApiProxyGenerationOptions options)
        {
            if (options.UseCache)
            {
                return _cache.GetOrAdd(CreateCacheKey(options), (key) => CreateScript(options));
            }

            return _cache[CreateCacheKey(options)] = CreateScript(options);
        }

        /// <summary>
        /// 生成脚本
        /// </summary>
        /// <param name="options">API代理生成选项</param>
        /// <returns></returns>
        private string CreateScript(ApiProxyGenerationOptions options)
        {
            var model = _modelProvider.CreateModel();

            if (options.IsPartialRequest())
            {
                model = model.CreateSubModel(options.Modules, options.Controllers, options.Actions);
            }

            var generatorType = _configuration.Generators.GetOrDefault(options.GeneratorType);
            if (generatorType == null)
            {
                throw new AbpException($"Could not find a proxy script generator with given name: {options.GeneratorType}");
            }

            using (var generator = _iocResolver.ResolveAsDisposable<IProxyScriptGenerator>(generatorType))
            {
                return generator.Object.CreateScript(model);
            }
        }

        /// <summary>
        /// 创建缓存Key
        /// </summary>
        /// <param name="options">API代理生成选项</param>
        /// <returns></returns>
        private static string CreateCacheKey(ApiProxyGenerationOptions options)
        {
            return options.ToJsonString().ToMd5();
        }
    }
}
