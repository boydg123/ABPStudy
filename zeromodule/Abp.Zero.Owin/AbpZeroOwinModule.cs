using System.Reflection;
using Abp.Authorization.Users;
using Abp.Modules;
using Abp.Zero;
using Abp.Configuration.Startup;
using Abp.Owin;

namespace Abp
{
    /// <summary>
    /// ABP Zero Owin模块
    /// </summary>
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class AbpZeroOwinModule : AbpModule
    {
        /// <summary>
        /// 初始化前
        /// </summary>
        public override void PreInitialize()
        {
            Configuration.ReplaceService<IUserTokenProviderAccessor, OwinUserTokenProviderAccessor>();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
