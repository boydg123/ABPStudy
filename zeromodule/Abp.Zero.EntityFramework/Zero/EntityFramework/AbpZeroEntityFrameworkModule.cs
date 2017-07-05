using System.Reflection;
using Abp.Domain.Uow;
using Abp.EntityFramework;
using Abp.Modules;
using Abp.MultiTenancy;
using Castle.MicroKernel.Registration;

namespace Abp.Zero.EntityFramework
{
    /// <summary>
    /// Entity framework integration module for ASP.NET Boilerplate Zero.
    /// ABP Zero EF整合模块
    /// </summary>
    [DependsOn(typeof(AbpZeroCoreModule), typeof(AbpEntityFrameworkModule))]
    public class AbpZeroEntityFrameworkModule : AbpModule
    {
        /// <summary>
        /// 初始化前
        /// </summary>
        public override void PreInitialize()
        {
            Configuration.ReplaceService(typeof(IConnectionStringResolver), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IConnectionStringResolver, IDbPerTenantConnectionStringResolver>()
                        .ImplementedBy<DbPerTenantConnectionStringResolver>()
                        .LifestyleTransient()
                    );
            });
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
