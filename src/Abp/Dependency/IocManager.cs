using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Abp.Dependency
{
    /// <summary>
    /// This class is used to directly perform dependency injection tasks.
    /// 这个类是用来完成依赖注入任务的
    /// </summary>
    public class IocManager : IIocManager
    {
        /// <summary>
        /// The Singleton instance.
        /// 单例实例
        /// </summary>
        public static IocManager Instance { get; private set; }

        /// <summary>
        /// Reference to the Castle Windsor Container.
        /// Castle Windsor容器的引用
        /// </summary>
        public IWindsorContainer IocContainer { get; private set; }

        /// <summary>
        /// List of all registered conventional registrars.
        /// 需要注册的约定注册列表
        /// </summary>
        private readonly List<IConventionalDependencyRegistrar> _conventionalRegistrars;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static IocManager()
        {
            Instance = new IocManager();
        }

        /// <summary>
        /// Creates a new <see cref="IocManager"/> object.
        /// 创建一个新的 <see cref="IocManager"/> 对象.
        /// Normally, you don't directly instantiate an <see cref="IocManager"/>.This may be useful for test purposes.
        /// 通常情况下，你不需要直接实例化一个 <see cref="IocManager"/>.这对于测试来说，会有帮助.
        /// </summary>
        public IocManager()
        {
            IocContainer = new WindsorContainer();
            _conventionalRegistrars = new List<IConventionalDependencyRegistrar>();

            //Register self!
            IocContainer.Register(
                Component.For<IocManager, IIocManager, IIocRegistrar, IIocResolver>().UsingFactoryMethod(() => this)
                );
        }

        /// <summary>
        /// Adds a dependency registrar for conventional registration.
        /// 添加一个约定注册的依赖注册器
        /// </summary>
        /// <param name="registrar">dependency registrar</param>
        public void AddConventionalRegistrar(IConventionalDependencyRegistrar registrar)
        {
            _conventionalRegistrars.Add(registrar);
        }

        /// <summary>
        /// Registers types of given assembly by all conventional registrars. See <see cref="AddConventionalRegistrar"/> method.
        /// 通过约定注册，注册给定程序集的类型，查看方法 <see cref="AddConventionalRegistrar"/>
        /// </summary>
        /// <param name="assembly">Assembly to register / 需要注册的程序集</param>
        public void RegisterAssemblyByConvention(Assembly assembly)
        {
            RegisterAssemblyByConvention(assembly, new ConventionalRegistrationConfig());
        }

        /// <summary>
        /// Registers types of given assembly by all conventional registrars. See <see cref="AddConventionalRegistrar"/> method.
        /// 通过约定注册，注册给定程序集的类型，查看方法 <see cref="AddConventionalRegistrar"/>
        /// </summary>
        /// <param name="assembly">Assembly to register / 要注册的程序集</param>
        /// <param name="config">Additional configuration / 额外的配置</param>
        public void RegisterAssemblyByConvention(Assembly assembly, ConventionalRegistrationConfig config)
        {
            var context = new ConventionalRegistrationContext(assembly, this, config);

            foreach (var registerer in _conventionalRegistrars)
            {
                registerer.RegisterAssembly(context);
            }

            if (config.InstallInstallers)
            {
                IocContainer.Install(FromAssembly.Instance(assembly));
            }
        }

        /// <summary>
        /// Registers a type as self registration.
        /// 自注册一个注册类型
        /// </summary>
        /// <typeparam name="TType">Type of the class / 注册类的类型</typeparam>
        /// <param name="lifeStyle">Lifestyle of the objects of this type / 对象的生命周期</param>
        public void Register<TType>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton) where TType : class
        {
            IocContainer.Register(ApplyLifestyle(Component.For<TType>(), lifeStyle));
        }

        /// <summary>
        /// Registers a type as self registration.
        /// 自注册一个注册类型
        /// </summary>
        /// <param name="type">Type of the class / 注册类的类型</param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type / 对象的生命周期</param>
        public void Register(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            IocContainer.Register(ApplyLifestyle(Component.For(type), lifeStyle));
        }

        /// <summary>
        /// Registers a type with it's implementation.
        /// 注册一个一个类型和它的实现
        /// </summary>
        /// <typeparam name="TType">Registering type / 注册类的类型</typeparam>
        /// <typeparam name="TImpl">The type that implements <see cref="TType"/> / 类 <see cref="TType"/>的实现</typeparam>
        /// <param name="lifeStyle">Lifestyle of the objects of this type / 对象的生命周期</param>
        public void Register<TType, TImpl>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where TType : class
            where TImpl : class, TType
        {
            IocContainer.Register(ApplyLifestyle(Component.For<TType, TImpl>().ImplementedBy<TImpl>(), lifeStyle));
        }

        /// <summary>
        /// Registers a type with it's implementation.
        /// 自注册一个一个类型和它的实现
        /// </summary>
        /// <param name="type">Type of the class / 注册类的类型</param>
        /// <param name="impl">The type that implements <paramref name="type"/> / 类 <paramref name="type"/>的实现</param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type / 对象的生命周期</param>
        public void Register(Type type, Type impl, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            IocContainer.Register(ApplyLifestyle(Component.For(type, impl).ImplementedBy(impl), lifeStyle));
        }

        /// <summary>
        /// Checks whether given type is registered before.
        /// 检查给定的类型是否已经注册
        /// </summary>
        /// <param name="type">Type to check / 需要检查的类型</param>
        public bool IsRegistered(Type type)
        {
            return IocContainer.Kernel.HasComponent(type);
        }

        /// <summary>
        /// Checks whether given type is registered before.
        /// 检查给定的类型是否已经注册
        /// </summary>
        /// <typeparam name="TType">Type to check / 需要检查的类型</typeparam>
        public bool IsRegistered<TType>()
        {
            return IocContainer.Kernel.HasComponent(typeof(TType));
        }

        /// <summary>
        /// Gets an object from IOC container.Returning object must be Released (see <see cref="IIocResolver.Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <typeparam name="T">Type of the object to get / 要获取对象的类型</typeparam>
        /// <returns>The instance object / 对象实例</returns>
        public T Resolve<T>()
        {
            return IocContainer.Resolve<T>();
        }

        /// <summary>
        /// Gets an object from IOC container.Returning object must be Released (see <see cref="Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <typeparam name="T">Type of the object to cast / 要转换的对象类型</typeparam>
        /// <param name="type">Type of the object to resolve / 要解决的对象类型</param>
        /// <returns>The object instance / 对象实例</returns>
        public T Resolve<T>(Type type)
        {
            return (T)IocContainer.Resolve(type);
        }

        /// <summary>
        /// Gets an object from IOC container.Returning object must be Released (see <see cref="IIocResolver.Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <typeparam name="T">Type of the object to get / 要获取对象的类型</typeparam>
        /// <param name="argumentsAsAnonymousType">Constructor arguments / 构造函数参数</param>
        /// <returns>The instance object / 对象实例</returns>
        public T Resolve<T>(object argumentsAsAnonymousType)
        {
            return IocContainer.Resolve<T>(argumentsAsAnonymousType);
        }

        /// <summary>
        /// Gets an object from IOC container.Returning object must be Released (see <see cref="IIocResolver.Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <param name="type">Type of the object to get / 要获取对象的类型</param>
        /// <returns>The instance object / 对象实例</returns>
        public object Resolve(Type type)
        {
            return IocContainer.Resolve(type);
        }

        /// <summary>
        /// Gets an object from IOC container.Returning object must be Released (see <see cref="IIocResolver.Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <param name="type">Type of the object to get / 要获取对象的类型</param>
        /// <param name="argumentsAsAnonymousType">Constructor arguments / 构造函数参数</param>
        /// <returns>The instance object / 对象实例</returns>
        public object Resolve(Type type, object argumentsAsAnonymousType)
        {
            return IocContainer.Resolve(type, argumentsAsAnonymousType);
        }

        /// <summary>
        /// 解决与此类型匹配的所有有效组件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return IocContainer.ResolveAll<T>();
        }

        /// <summary>
        /// 解决与此类型匹配的所有有效组件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="argumentsAsAnonymousType">构造函数参数</param>
        /// <returns></returns>
        public T[] ResolveAll<T>(object argumentsAsAnonymousType)
        {
            return IocContainer.ResolveAll<T>(argumentsAsAnonymousType);
        }

        /// <summary>
        /// 解决与此服务相匹配的所有有效组件
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public object[] ResolveAll(Type type)
        {
            return IocContainer.ResolveAll(type).Cast<object>().ToArray();
        }

        /// <summary>
        /// 解决所有与此服务相匹配的服务的有效组件以匹配参数
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="argumentsAsAnonymousType">构造函数参数</param>
        /// <returns></returns>
        public object[] ResolveAll(Type type, object argumentsAsAnonymousType)
        {
            return IocContainer.ResolveAll(type, argumentsAsAnonymousType).Cast<object>().ToArray();
        }

        /// <summary>
        /// Releases a pre-resolved object. See Resolve methods.
        /// 释放方法Resolve获取的对象。查看Resolve方法
        /// </summary>
        /// <param name="obj">Object to be released</param>
        public void Release(object obj)
        {
            IocContainer.Release(obj);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            IocContainer.Dispose();
        }

        /// <summary>
        /// 应用生命周期
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="registration">单类型组建注册工厂</param>
        /// <param name="lifeStyle">生命周期枚举</param>
        /// <returns></returns>
        private static ComponentRegistration<T> ApplyLifestyle<T>(ComponentRegistration<T> registration, DependencyLifeStyle lifeStyle)
            where T : class
        {
            switch (lifeStyle)
            {
                case DependencyLifeStyle.Transient:
                    return registration.LifestyleTransient();
                case DependencyLifeStyle.Singleton:
                    return registration.LifestyleSingleton();
                default:
                    return registration;
            }
        }
    }
}