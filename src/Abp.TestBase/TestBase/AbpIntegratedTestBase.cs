using System;
using Abp.Dependency;
using Abp.Modules;
using Abp.Runtime.Session;
using Abp.TestBase.Runtime.Session;

namespace Abp.TestBase
{
    /// <summary>
    /// This is the base class for all tests integrated to ABP.
    /// 这是所有测试集成到ABP的基类
    /// </summary>
    public abstract class AbpIntegratedTestBase<TStartupModule> : IDisposable 
        where TStartupModule : AbpModule
    {
        /// <summary>
        /// Local <see cref="IIocManager"/> used for this test.
        /// 用于此测试的本地IOC管理器
        /// </summary>
        protected IIocManager LocalIocManager { get; }

        /// <summary>
        /// ABP启动类
        /// </summary>
        protected AbpBootstrapper AbpBootstrapper { get; }

        /// <summary>
        /// Gets Session object. Can be used to change current user and tenant in tests.
        /// 获取Session对象，可以在测试中改变当前用户和租户
        /// </summary>
        protected TestAbpSession AbpSession { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="initializeAbp">是否初始化ABP</param>
        protected AbpIntegratedTestBase(bool initializeAbp = true)
        {
            LocalIocManager = new IocManager();
            AbpBootstrapper = AbpBootstrapper.Create<TStartupModule>(LocalIocManager);

            if (initializeAbp)
            {
                InitializeAbp();
            }
        }

        /// <summary>
        /// 初始化ABP
        /// </summary>
        protected void InitializeAbp()
        {
            LocalIocManager.Register<IAbpSession, TestAbpSession>();

            PreInitialize();

            AbpBootstrapper.Initialize();

            AbpSession = LocalIocManager.Resolve<TestAbpSession>();
        }

        /// <summary>
        /// This method can be overrided to replace some services with fakes.
        /// 这个方法可以被一些服务伪造来重写或者替换
        /// </summary>
        protected virtual void PreInitialize()
        {

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            AbpBootstrapper.Dispose();
            LocalIocManager.Dispose();
        }

        /// <summary>
        /// A shortcut to resolve an object from <see cref="LocalIocManager"/>.
        /// <see cref="LocalIocManager"/>解析对象的快捷方式
        /// Also registers <typeparamref name="T"/> as transient if it's not registered before.
        /// 如果<typeparamref name="T"/>没有被注册，则注册它为时时对象
        /// </summary>
        /// <typeparam name="T">Type of the object to get / 获取对象的类型</typeparam>
        /// <returns>The object instance / 对象实例</returns>
        protected T Resolve<T>()
        {
            EnsureClassRegistered(typeof(T));
            return LocalIocManager.Resolve<T>();
        }

        /// <summary>
        /// A shortcut to resolve an object from <see cref="LocalIocManager"/>.
        /// <see cref="LocalIocManager"/>解析对象的快捷方式
        /// Also registers <typeparamref name="T"/> as transient if it's not registered before.
        /// 如果<typeparamref name="T"/>没有被注册，则注册它为时时对象
        /// </summary>
        /// <typeparam name="T">Type of the object to get / 获取对象的类型</typeparam>
        /// <param name="argumentsAsAnonymousType">Constructor arguments / 构造函数参数</param>
        /// <returns>The object instance / 对象实例</returns>
        protected T Resolve<T>(object argumentsAsAnonymousType)
        {
            EnsureClassRegistered(typeof(T));
            return LocalIocManager.Resolve<T>(argumentsAsAnonymousType);
        }

        /// <summary>
        /// A shortcut to resolve an object from <see cref="LocalIocManager"/>.
        /// <see cref="LocalIocManager"/>解析对象的快捷方式
        /// Also registers <paramref name="type"/> as transient if it's not registered before.
        /// 如果<typeparamref name="T"/>没有被注册，则注册它为时时对象
        /// </summary>
        /// <param name="type">Type of the object to get / 获取对象的类型</param>
        /// <returns>The object instance / 对象实例</returns>
        protected object Resolve(Type type)
        {
            EnsureClassRegistered(type);
            return LocalIocManager.Resolve(type);
        }

        /// <summary>
        /// A shortcut to resolve an object from <see cref="LocalIocManager"/>.
        /// <see cref="LocalIocManager"/>解析对象的快捷方式
        /// Also registers <paramref name="type"/> as transient if it's not registered before.
        /// 如果<typeparamref name="T"/>没有被注册，则注册它为时时对象
        /// </summary>
        /// <param name="type">Type of the object to get / 获取对象的类型</param>
        /// <param name="argumentsAsAnonymousType">Constructor arguments / 构造函数参数</param>
        /// <returns>The object instance / 对象实例</returns>
        protected object Resolve(Type type, object argumentsAsAnonymousType)
        {
            EnsureClassRegistered(type);
            return LocalIocManager.Resolve(type, argumentsAsAnonymousType);
        }

        /// <summary>
        /// Registers given type if it's not registered before.
        /// 注册给定类型，如果它没有被注册
        /// </summary>
        /// <param name="type">Type to check and register / 被检查和注册的类型</param>
        /// <param name="lifeStyle">Lifestyle / 生命周期</param>
        protected void EnsureClassRegistered(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
        {
            if (!LocalIocManager.IsRegistered(type))
            {
                if (!type.IsClass || type.IsAbstract)
                {
                    throw new AbpException("Can not register " + type.Name + ". It should be a non-abstract class. If not, it should be registered before.");
                }

                LocalIocManager.Register(type, lifeStyle);
            }
        }
    }
}
