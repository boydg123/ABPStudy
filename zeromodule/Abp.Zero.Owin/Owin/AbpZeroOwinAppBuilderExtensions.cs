using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Extensions;
using Microsoft.Owin.Security.DataProtection;
using Owin;

namespace Abp.Owin
{
    /// <summary>
    /// ABP Zero Owin App Builder扩展
    /// </summary>
    public static class AbpZeroOwinAppBuilderExtensions
    {
        /// <summary>
        /// 注册数据保护提供程序
        /// </summary>
        /// <param name="app">APP</param>
        public static void RegisterDataProtectionProvider(this IAppBuilder app)
        {
            if (!IocManager.Instance.IsRegistered<IUserTokenProviderAccessor>())
            {
                throw new AbpException("IUserTokenProviderAccessor is not registered!");
            }

            var providerAccessor = IocManager.Instance.Resolve<IUserTokenProviderAccessor>();
            if (!(providerAccessor is OwinUserTokenProviderAccessor))
            {
                throw new AbpException($"IUserTokenProviderAccessor should be instance of {nameof(OwinUserTokenProviderAccessor)}!");
            }

            providerAccessor.As<OwinUserTokenProviderAccessor>().DataProtectionProvider = app.GetDataProtectionProvider();
        }
    }
}