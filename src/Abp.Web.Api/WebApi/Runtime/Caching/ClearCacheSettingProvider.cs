using System.Collections.Generic;
using Abp.Configuration;

namespace Abp.WebApi.Runtime.Caching
{
    /// <summary>
    /// 清除缓存设置提供者
    /// </summary>
    public class ClearCacheSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(ClearCacheSettingNames.Password, "123qweasdZXC")
            };
        }
    }
}
