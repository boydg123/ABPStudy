using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Localization;
using Abp.Modules;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Web.Configuration;

namespace Abp.Web
{
    /// <summary>
    /// This class is used to simplify starting of ABP system using <see cref="AbpBootstrapper"/> class..
    /// 此类使用<see cref="AbpBootstrapper"/>类简化启动ABP系统。
    /// Inherit from this class in global.asax instead of <see cref="HttpApplication"/> to be able to start ABP system.
    /// 继承此类能够启动ABP系统，在Global.asax中代替<see cref="HttpApplication"/>
    /// </summary>
    /// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="AbpModule"/>. / 启动模块的应用程序依赖其他使用的模块，应该来自<see cref="AbpModule"/></typeparam>
    public abstract class AbpWebApplication<TStartupModule> : HttpApplication
        where TStartupModule : AbpModule
    {
        /// <summary>
        /// Gets a reference to the <see cref="AbpBootstrapper"/> instance.
        /// 获取一个<see cref="AbpBootstrapper"/>实例的引用
        /// </summary>
        public static AbpBootstrapper AbpBootstrapper { get; } = AbpBootstrapper.Create<TStartupModule>();

        /// <summary>
        /// ABP Web本地化配置
        /// </summary>
        private static IAbpWebLocalizationConfiguration _webLocalizationConfiguration;

        /// <summary>
        /// This method is called by ASP.NET system on web application's startup.
        /// 在web应用程序启动时，通过ASP.NET系统调用此方法
        /// </summary>
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            ThreadCultureSanitizer.Sanitize();

            AbpBootstrapper.Initialize();

            _webLocalizationConfiguration = AbpBootstrapper.IocManager.Resolve<IAbpWebLocalizationConfiguration>();
        }

        /// <summary>
        /// This method is called by ASP.NET system on web application shutdown.
        /// 在web应用程序关闭时，通过ASP.NET系统调用此方法
        /// </summary>
        protected virtual void Application_End(object sender, EventArgs e)
        {
            AbpBootstrapper.Dispose();
        }

        /// <summary>
        /// This method is called by ASP.NET system when a session starts.
        /// 当一个会话启动时，通过ASP.NET系统调用此方法
        /// </summary>
        protected virtual void Session_Start(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This method is called by ASP.NET system when a session ends.
        /// 当一个会话结束时，通过ASP.NET系统调用此方法
        /// </summary>
        protected virtual void Session_End(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This method is called by ASP.NET system when a request starts.
        /// 当一个请求开始时，通过ASP.NET系统调用此方法
        /// </summary>
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            SetCurrentCulture();
        }

        /// <summary>
        /// 设置当前文化信息
        /// </summary>
        protected virtual void SetCurrentCulture()
        {
            var globalizationSection = WebConfigurationManager.GetSection("globalization") as GlobalizationSection;
            if (globalizationSection != null &&
                !globalizationSection.UICulture.IsNullOrEmpty() &&
                !string.Equals(globalizationSection.UICulture, "auto", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var langCookie = Request.Cookies[_webLocalizationConfiguration.CookieName];
            if (langCookie != null && GlobalizationHelper.IsValidCultureCode(langCookie.Value))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(langCookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCookie.Value);
                return;
            }

            var defaultLanguage = AbpBootstrapper.IocManager.Using<ISettingManager, string>(settingManager => settingManager.GetSettingValue(LocalizationSettingNames.DefaultLanguage));
            if (!defaultLanguage.IsNullOrEmpty() && GlobalizationHelper.IsValidCultureCode(defaultLanguage))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(defaultLanguage);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(defaultLanguage);
                Response.SetCookie(new HttpCookie(_webLocalizationConfiguration.CookieName, defaultLanguage));
                return;
            }

            if (!Request.UserLanguages.IsNullOrEmpty())
            {
                var firstValidLanguage = Request?.UserLanguages?.FirstOrDefault(GlobalizationHelper.IsValidCultureCode);
                if (firstValidLanguage != null)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(firstValidLanguage);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(firstValidLanguage);
                    Response.SetCookie(new HttpCookie(_webLocalizationConfiguration.CookieName, firstValidLanguage));
                }
            }
        }

        /// <summary>
        /// This method is called by ASP.NET system when a request ends.
        /// 当一个请求结束时，通过ASP.NET系统调用此方法
        /// </summary>
        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {

        }

        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected virtual void Application_Error(object sender, EventArgs e)
        {

        }
    }
}
