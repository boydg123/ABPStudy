using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;

namespace Abp.Web.Settings
{
    /// <summary>
    /// This class is used to build setting script.
    /// 此类用于生成设置脚本
    /// </summary>
    public class SettingScriptManager : ISettingScriptManager, ISingletonDependency
    {
        /// <summary>
        /// 设置定义管理器
        /// </summary>
        private readonly ISettingDefinitionManager _settingDefinitionManager;

        /// <summary>
        /// 设置管理器
        /// </summary>
        private readonly ISettingManager _settingManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settingDefinitionManager">设置定义管理器</param>
        /// <param name="settingManager">设置管理器</param>
        public SettingScriptManager(ISettingDefinitionManager settingDefinitionManager, ISettingManager settingManager)
        {
            _settingDefinitionManager = settingDefinitionManager;
            _settingManager = settingManager;
        }

        /// <summary>
        /// 获取包含设置值的Javascript脚本
        /// </summary>
        public async Task<string> GetScriptAsync()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    abp.setting = abp.setting || {};");
            script.AppendLine("    abp.setting.values = {");

            var settingDefinitions = _settingDefinitionManager
                .GetAllSettingDefinitions()
                .Where(sd => sd.IsVisibleToClients);

            var added = 0;
            foreach (var settingDefinition in settingDefinitions)
            {
                if (added > 0)
                {
                    script.AppendLine(",");
                }
                else
                {
                    script.AppendLine();
                }

                var settingValue = await _settingManager.GetSettingValueAsync(settingDefinition.Name);

                script.Append("        '" +
                              settingDefinition.Name .Replace("'", @"\'") + "': " +
                              (settingValue == null ? "null" : "'" + settingValue.Replace(@"\", @"\\").Replace("'", @"\'") + "'"));

                ++added;
            }

            script.AppendLine();
            script.AppendLine("    };");

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}