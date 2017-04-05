using System.Reflection;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFramework.Repositories;
using Abp.EntityFramework.Uow;
using Abp.Modules;
using Abp.Reflection;
using Castle.MicroKernel.Registration;

namespace Abp.EntityFramework
{
    /// <summary>
    /// This module is used to implement "Data Access Layer" in EntityFramework.
    /// 这个模块是用来在EF中实现数据访问层。该模块依赖于<see cref="AbpKernelModule"/>
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpEntityFrameworkModule : AbpModule
    {
        /// <summary>
        /// 类型查找器
        /// </summary>
        private readonly ITypeFinder _typeFinder;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="typeFinder">类型查找器</param>
        public AbpEntityFrameworkModule(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }

        /// <summary>
        /// 这是应用启动调用的第一个事件，这里面的代码，会在依赖注入注册之前运行
        /// </summary>
        public override void PreInitialize()
        {
            Configuration.ReplaceService<IUnitOfWorkFilterExecuter>(() =>
            {
                IocManager.IocContainer.Register(
                    Component
                    .For<IUnitOfWorkFilterExecuter, IEfUnitOfWorkFilterExecuter>()
                    .ImplementedBy<EfDynamicFiltersUnitOfWorkFilterExecuter>()
                    .LifestyleTransient()
                );
            });
        }

        /// <summary>
        /// 这个方法用于模块的依赖注册
        /// </summary>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(
                Component.For(typeof(IDbContextProvider<>))
                    .ImplementedBy(typeof(UnitOfWorkDbContextProvider<>))
                    .LifestyleTransient()
                );

            RegisterGenericRepositoriesAndMatchDbContexes();
        }

        /// <summary>
        /// 注册那些不注册的仓储，并且匹配数据库上下文
        /// </summary>
        private void RegisterGenericRepositoriesAndMatchDbContexes()
        {
            var dbContextTypes =
                _typeFinder.Find(type =>
                    type.IsPublic &&
                    !type.IsAbstract &&
                    type.IsClass &&
                    typeof(AbpDbContext).IsAssignableFrom(type)
                    );

            if (dbContextTypes.IsNullOrEmpty())
            {
                Logger.Warn("No class found derived from AbpDbContext.");
                return;
            }

            using (var repositoryRegistrar = IocManager.ResolveAsDisposable<IEntityFrameworkGenericRepositoryRegistrar>())
            {
                foreach (var dbContextType in dbContextTypes)
                {
                    Logger.Debug("Registering DbContext: " + dbContextType.AssemblyQualifiedName);
                    repositoryRegistrar.Object.RegisterForDbContext(dbContextType, IocManager);
                }
            }

            using (var dbContextMatcher = IocManager.ResolveAsDisposable<IDbContextTypeMatcher>())
            {
                dbContextMatcher.Object.Populate(dbContextTypes);
            }
        }
    }
}
