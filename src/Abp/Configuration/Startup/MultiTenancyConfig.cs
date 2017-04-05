namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Used to configure multi-tenancy.
    /// 用于配置多租赁
    /// </summary>
    internal class MultiTenancyConfig : IMultiTenancyConfig
    {
        /// <summary>
        /// Is multi-tenancy enabled?Default value: false.
        /// 是否启用多租户，默认值false
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}