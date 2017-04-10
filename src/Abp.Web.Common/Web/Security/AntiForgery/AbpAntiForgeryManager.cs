using System;
using Abp.Dependency;
using Castle.Core.Logging;

namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// ABP防伪管理器实现
    /// </summary>
    public class AbpAntiForgeryManager : IAbpAntiForgeryManager, IAbpAntiForgeryValidator, ITransientDependency
    {
        /// <summary>
        /// 日志记录器引用
        /// </summary>
        public ILogger Logger { protected get; set; }

        /// <summary>
        /// ABP防伪配置器
        /// </summary>
        public IAbpAntiForgeryConfiguration Configuration { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">ABP防伪配置器</param>
        public AbpAntiForgeryManager(IAbpAntiForgeryConfiguration configuration)
        {
            Configuration = configuration;
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 生成令牌
        /// </summary>
        public virtual string GenerateToken()
        {
            return Guid.NewGuid().ToString("D");
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="cookieValue"></param>
        /// <param name="tokenValue"></param>
        /// <returns></returns>
        public virtual bool IsValid(string cookieValue, string tokenValue)
        {
            return cookieValue == tokenValue;
        }
    }
}