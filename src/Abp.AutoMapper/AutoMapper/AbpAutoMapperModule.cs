using Abp.Localization;
using Abp.Modules;
using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Reflection;
using AutoMapper;
using Castle.MicroKernel.Registration;

namespace Abp.AutoMapper
{
    /// <summary>
    /// Abp AutoMapper模块，此模块依赖<see cref="AbpKernelModule"/>
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpAutoMapperModule : AbpModule
    {
        /// <summary>
        /// 类型查找器
        /// </summary>
        private readonly ITypeFinder _typeFinder;

        /// <summary>
        /// 是否映射之前创建
        /// </summary>
        private static bool _createdMappingsBefore;

        /// <summary>
        /// 同步对象
        /// </summary>
        private static readonly object SyncObj = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="typeFinder">类型查找器</param>
        public AbpAutoMapperModule(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }

        /// <summary>
        /// 这是应用启动调用的第一个事件，这里面的代码，会在依赖注入注册之前运行
        /// </summary>
        public override void PreInitialize()
        {
            IocManager.Register<IAbpAutoMapperConfiguration, AbpAutoMapperConfiguration>();

            Configuration.ReplaceService<ObjectMapping.IObjectMapper, AutoMapperObjectMapper>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(CreateCoreMappings);
        }

        /// <summary>
        /// 这个方法在应用启动最后被调用
        /// </summary>
        public override void PostInitialize()
        {
            CreateMappings();

            IocManager.IocContainer.Register(
                Component.For<IMapper>().Instance(Mapper.Instance).LifestyleSingleton()
            );
        }

        /// <summary>
        /// 创建映射
        /// </summary>
        public void CreateMappings()
        {
            lock (SyncObj)
            {
                //We should prevent duplicate mapping in an application, since Mapper is static.
                //我们应该防止应用程序中的重复映射,由于映射器是静态的
                if (_createdMappingsBefore)
                {
                    return;
                }

                Mapper.Initialize(configuration =>
                {
                    FindAndAutoMapTypes(configuration);
                    foreach (var configurator in Configuration.Modules.AbpAutoMapper().Configurators)
                    {
                        configurator(configuration);
                    }
                });

                _createdMappingsBefore = true;
            }
        }

        /// <summary>
        /// 查找自动映射类型
        /// </summary>
        /// <param name="configuration">映射器配置</param>
        private void FindAndAutoMapTypes(IMapperConfigurationExpression configuration)
        {
            var types = _typeFinder.Find(type =>
                    type.IsDefined(typeof(AutoMapAttribute)) ||
                    type.IsDefined(typeof(AutoMapFromAttribute)) ||
                    type.IsDefined(typeof(AutoMapToAttribute))
            );

            Logger.DebugFormat("Found {0} classes define auto mapping attributes", types.Length);
            foreach (var type in types)
            {
                Logger.Debug(type.FullName);
                configuration.CreateAbpAttributeMaps(type);
            }
        }

        /// <summary>
        /// 创建核心映射
        /// </summary>
        /// <param name="configuration">映射器配置</param>
        private void CreateCoreMappings(IMapperConfigurationExpression configuration)
        {
            var localizationContext = IocManager.Resolve<ILocalizationContext>();

            configuration.CreateMap<ILocalizableString, string>().ConvertUsing(ls => ls?.Localize(localizationContext));
            configuration.CreateMap<LocalizableString, string>().ConvertUsing(ls => ls == null ? null : localizationContext.LocalizationManager.GetString(ls));
        }
    }
}
