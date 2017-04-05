using System;
using System.Globalization;
using System.Web;
using Abp.Castle.Logging.Log4Net;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Localization;
using Abp.Logging;
using Abp.Timing;
using Abp.Extensions;
using Abp.Web;
using Castle.Facilities.Logging;
using Derrick.Web.MultiTenancy;

namespace Derrick.Web
{
    public class MvcApplication : AbpWebApplication<AbpZeroTemplateWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            //Use UTC clock. Remove this to use local time for your application.
            Clock.Provider = ClockProviders.Utc;

            //Log4Net configuration
            AbpBootstrapper.IocManager.IocContainer
                .AddFacility<LoggingFacility>(f => f.UseAbpLog4Net()
                    .WithConfig("log4net.config")
                );

            base.Application_Start(sender, e);
        }

        protected override void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            base.Application_AuthenticateRequest(sender, e);

            SetTentantId();
        }

        protected override void Session_Start(object sender, EventArgs e)
        {
            RestoreUserLanguage();
            base.Session_Start(sender, e);
        }

        private void RestoreUserLanguage()
        {
            var settingManager = AbpBootstrapper.IocManager.Resolve<ISettingManager>();
            var defaultLanguage = settingManager.GetSettingValue(LocalizationSettingNames.DefaultLanguage);

            if (defaultLanguage.IsNullOrEmpty())
            {
                return;
            }

            try
            {
                CultureInfo.GetCultureInfo(defaultLanguage);
                Response.Cookies.Add(new HttpCookie("Abp.Localization.CultureName", defaultLanguage) { Expires = Clock.Now.AddYears(2) });
            }
            catch (CultureNotFoundException exception)
            {
                LogHelper.Logger.Warn(exception.Message, exception);
            }
        }

        /// <summary>
        /// This method tries to set current tenant id if current user has not login.
        /// Thus, we can get IAbpSession.TenantId later.
        /// </summary>
        private void SetTentantId()
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                return;
            }

            using (var currentTenantAccessor = AbpBootstrapper.IocManager.ResolveAsDisposable<TenantIdAccessor>())
            {
                currentTenantAccessor.Object.SetCurrentTenantId();
            }
        }
    }
}
