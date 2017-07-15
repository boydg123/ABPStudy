namespace Abp.Configuration
{
    /// <summary>
    /// 设置提供者上下文
    /// </summary>
    public class SettingDefinitionProviderContext
    {
        /// <summary>
        /// 设置定义管理者
        /// </summary>
        public ISettingDefinitionManager Manager { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manager"></param>
        internal SettingDefinitionProviderContext(ISettingDefinitionManager manager)
        {
            Manager = manager;
        }
    }
}