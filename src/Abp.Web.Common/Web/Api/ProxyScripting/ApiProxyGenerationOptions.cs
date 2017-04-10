using System.Collections.Generic;
using Abp.Collections.Extensions;

namespace Abp.Web.Api.ProxyScripting
{
    /// <summary>
    /// API代理生成选项
    /// </summary>
    public class ApiProxyGenerationOptions
    {
        /// <summary>
        /// 生成类型
        /// </summary>
        public string GeneratorType { get; set; }

        /// <summary>
        /// 用户缓存
        /// </summary>
        public bool UseCache { get; set; }

        /// <summary>
        /// 模块列表
        /// </summary>
        public string[] Modules { get; set; }

        /// <summary>
        /// 控制器集合
        /// </summary>
        public string[] Controllers { get; set; }

        /// <summary>
        /// Action集合
        /// </summary>
        public string[] Actions { get; set; }

        /// <summary>
        /// 属性集合
        /// </summary>
        public IDictionary<string, string> Properties { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="generatorType">生成类型</param>
        /// <param name="useCache"></param>
        public ApiProxyGenerationOptions(string generatorType, bool useCache = true)
        {
            GeneratorType = generatorType;
            UseCache = useCache;

            Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// 是否是部分请求
        /// </summary>
        /// <returns></returns>
        public bool IsPartialRequest()
        {
            return !(Modules.IsNullOrEmpty() && Controllers.IsNullOrEmpty() && Actions.IsNullOrEmpty());
        }
    }
}