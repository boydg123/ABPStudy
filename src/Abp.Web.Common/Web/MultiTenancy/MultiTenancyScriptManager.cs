using System;
using System.Globalization;
using System.Text;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Extensions;
using Abp.MultiTenancy;

namespace Abp.Web.MultiTenancy
{
    /// <summary>
    /// 多租户脚本管理器默认实现
    /// </summary>
    public class MultiTenancyScriptManager : IMultiTenancyScriptManager, ITransientDependency
    {
        /// <summary>
        /// 多租户配置
        /// </summary>
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="multiTenancyConfig">多租户配置</param>
        public MultiTenancyScriptManager(IMultiTenancyConfig multiTenancyConfig)
        {
            _multiTenancyConfig = multiTenancyConfig;
        }

        /// <summary>
        /// 获取多租户客户端脚本
        /// </summary>
        /// <returns></returns>
        public string GetScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(abp){");
            script.AppendLine();

            script.AppendLine("    abp.multiTenancy = abp.multiTenancy || {};");
            script.AppendLine("    abp.multiTenancy.isEnabled = " + _multiTenancyConfig.IsEnabled.ToString().ToLower(CultureInfo.InvariantCulture) + ";");

            script.AppendLine();
            script.Append("})(abp);");

            return script.ToString();
        }
    }
}