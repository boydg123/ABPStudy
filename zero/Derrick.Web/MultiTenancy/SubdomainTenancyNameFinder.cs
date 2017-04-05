using System;
using System.Web;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Text;
using Derrick.Configuration;
using Derrick.MultiTenancy;

namespace Derrick.Web.MultiTenancy
{
    /// <summary>
    /// Finds current tenant's tenancyname from subdomain of current URL.
    /// </summary>
    public class SubdomainTenancyNameFinder : ITenancyNameFinder, ITransientDependency
    {
        private readonly ISettingManager _settingManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        public SubdomainTenancyNameFinder(ISettingManager settingManager, IMultiTenancyConfig multiTenancyConfig)
        {
            _settingManager = settingManager;
            _multiTenancyConfig = multiTenancyConfig;
        }

        public string GetCurrentTenancyNameOrNull()
        {
            if (!_multiTenancyConfig.IsEnabled)
            {
                return Tenant.DefaultTenantName;
            }

            var siteRootFormat = _settingManager.GetSettingValue(AppSettings.General.WebSiteRootAddress).EnsureEndsWith('/');

            if (!siteRootFormat.Contains(WebUrlService.TenancyNamePlaceHolder))
            {
                //Web site does not support subdomain tenant name
                return null;
            }

            if (HttpContext.Current == null || HttpContext.Current.Request.Url == null)
            {
                //Can not find current URL
                return null;
            }

            var currentRootAddress = GetCurrentSiteRootAddress().EnsureEndsWith('/');
            
            string[] values;
            if (!FormattedStringValueExtracter.IsMatch(currentRootAddress, siteRootFormat, out values, true))
            {
                return null;
            }

            if (values.Length <= 0)
            {
                return null;
            }
            
            if (string.Equals(values[0], "www", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return values[0];
        }

        private static string GetCurrentSiteRootAddress()
        {
            var url = HttpContext.Current.Request.Url;
            return url.Scheme + System.Uri.SchemeDelimiter + url.Host + (url.IsDefaultPort ? "" : ":" + url.Port)
                + HttpContext.Current.Request.ApplicationPath;
        }
    }
}