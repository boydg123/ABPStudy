using System.Linq;
using System.Reflection;
using Abp.Application.Features;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.MultiTenancy;
using Abp.Reflection;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;

namespace Abp.Zero
{
    /// <summary>
    /// ABP zero core module.
    /// ABP Zero Core模块
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpZeroCoreModule : AbpModule
    {
        /// <summary>
        /// 模块初始化前
        /// </summary>
        public override void PreInitialize()
        {
            IocManager.Register<IRoleManagementConfig, RoleManagementConfig>();
            IocManager.Register<IUserManagementConfig, UserManagementConfig>();
            IocManager.Register<ILanguageManagementConfig, LanguageManagementConfig>();
            IocManager.Register<IAbpZeroEntityTypes, AbpZeroEntityTypes>();
            IocManager.Register<IAbpZeroConfig, AbpZeroConfig>();

            Configuration.Settings.Providers.Add<AbpZeroSettingProvider>();

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    AbpZeroConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(), "Abp.Zero.Zero.Localization.Source"
                        )));

            IocManager.IocContainer.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }
        /// <summary>
        /// 模块初始化
        /// </summary>
        public override void Initialize()
        {
            FillMissingEntityTypes();

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.Register<IMultiTenantLocalizationDictionary, MultiTenantLocalizationDictionary>(DependencyLifeStyle.Transient); //could not register conventionally

            RegisterTenantCache();
        }
        /// <summary>
        /// 组件注册
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        private void Kernel_ComponentRegistered(string key, Castle.MicroKernel.IHandler handler)
        {
            if (typeof(IAbpZeroFeatureValueStore).IsAssignableFrom(handler.ComponentModel.Implementation) && !IocManager.IsRegistered<IAbpZeroFeatureValueStore>())
            {
                IocManager.IocContainer.Register(
                    Component.For<IAbpZeroFeatureValueStore>().ImplementedBy(handler.ComponentModel.Implementation).Named("AbpZeroFeatureValueStore").LifestyleTransient()
                    );
            }
        }
        /// <summary>
        /// 填充Miss实体类型
        /// </summary>
        private void FillMissingEntityTypes()
        {
            using (var entityTypes = IocManager.ResolveAsDisposable<IAbpZeroEntityTypes>())
            {
                if (entityTypes.Object.User != null &&
                    entityTypes.Object.Role != null &&
                    entityTypes.Object.Tenant != null)
                {
                    return;
                }

                using (var typeFinder = IocManager.ResolveAsDisposable<ITypeFinder>())
                {
                    var types = typeFinder.Object.FindAll();
                    entityTypes.Object.Tenant = types.FirstOrDefault(t => typeof(AbpTenantBase).IsAssignableFrom(t) && !t.IsAbstract);
                    entityTypes.Object.Role = types.FirstOrDefault(t => typeof(AbpRoleBase).IsAssignableFrom(t) && !t.IsAbstract);
                    entityTypes.Object.User = types.FirstOrDefault(t => typeof(AbpUserBase).IsAssignableFrom(t) && !t.IsAbstract);
                }
            }
        }
        /// <summary>
        /// 注册商户缓存
        /// </summary>
        private void RegisterTenantCache()
        {
            if (IocManager.IsRegistered<ITenantCache>())
            {
                return;
            }

            using (var entityTypes = IocManager.ResolveAsDisposable<IAbpZeroEntityTypes>())
            {
                var implType = typeof (TenantCache<,>)
                    .MakeGenericType(entityTypes.Object.Tenant, entityTypes.Object.User);

                IocManager.Register(typeof (ITenantCache), implType, DependencyLifeStyle.Transient);
            }
        }
    }
}
