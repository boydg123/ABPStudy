using Abp.Configuration;
using Abp.Dependency;
using Abp.Extensions;
using Derrick.Configuration;

namespace Derrick.Web
{
    /// <summary>
    /// <see cref="IWebUrlService"/>实现，站点Url服务
    /// </summary>
    public class WebUrlService : IWebUrlService, ITransientDependency
    {
        /// <summary>
        /// 商户名称占位符
        /// </summary>
        public const string TenancyNamePlaceHolder = "{TENANCY_NAME}";

        /// <summary>
        /// 设置管理器
        /// </summary>
        private readonly ISettingManager _settingManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settingManager">设置管理器</param>
        public WebUrlService(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        /// <summary>
        /// 获取站点根地址
        /// </summary>
        /// <param name="tenancyName">商户名称</param>
        /// <returns></returns>
        public string GetSiteRootAddress(string tenancyName = null)
        {
            var siteRootFormat = _settingManager.GetSettingValue(AppSettings.General.WebSiteRootAddress).EnsureEndsWith('/');

            if (!siteRootFormat.Contains(TenancyNamePlaceHolder))
            {
                return siteRootFormat;
            }

            if (siteRootFormat.Contains(TenancyNamePlaceHolder + "."))
            {
                siteRootFormat = siteRootFormat.Replace(TenancyNamePlaceHolder + ".", TenancyNamePlaceHolder);
            }

            if (tenancyName.IsNullOrEmpty())
            {
                return siteRootFormat.Replace(TenancyNamePlaceHolder, "");
            }

            return siteRootFormat.Replace(TenancyNamePlaceHolder, tenancyName + ".");
        }
    }
}