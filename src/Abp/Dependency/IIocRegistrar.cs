using System;
using System.Reflection;

namespace Abp.Dependency
{
    /// <summary>
    /// Define interface for classes those are used to register dependencies.
    /// 用于注册依赖注入类的接口
    /// </summary>
    public interface IIocRegistrar
    {
        /// <summary>
        /// Adds a dependency registrar for conventional registration.
        /// 添加一个用于约定注册的依赖注册器
        /// </summary>
        /// <param name="registrar">dependency registrar / 依赖注册器</param>
        void AddConventionalRegistrar(IConventionalDependencyRegistrar registrar);

        /// <summary>
        /// Registers types of given assembly by all conventional registrars. See <see cref="IocManager.AddConventionalRegistrar"/> method.
        /// 通过所有的约定注册器，注册给定程序集的类型，查看方法<see cref="IocManager.AddConventionalRegistrar"/>
        /// </summary>
        /// <param name="assembly">Assembly to register / 注册的程序集</param>
        void RegisterAssemblyByConvention(Assembly assembly);

        /// <summary>
        /// Registers types of given assembly by all conventional registrars. See <see cref="IocManager.AddConventionalRegistrar"/> method.
        /// 通过所有的约定注册器，注册给定程序集的类型，查看方法<see cref="IocManager.AddConventionalRegistrar"/>
        /// </summary>
        /// <param name="assembly">Assembly to register / 注册的程序集</param>
        /// <param name="config">Additional configuration / 额外的配置</param>
        void RegisterAssemblyByConvention(Assembly assembly, ConventionalRegistrationConfig config);

        /// <summary>
        /// Registers a type as self registration.
        /// 自注册一个注册类型
        /// </summary>
        /// <typeparam name="T">Type of the class / 注册类的类型</typeparam>
        /// <param name="lifeStyle">Lifestyle of the objects of this type / 对象的生命周期</param>
        void Register<T>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where T : class;

        /// <summary>
        /// Registers a type as self registration.
        /// 自注册一个注册类型
        /// </summary>
        /// <param name="type">Type of the class / 注册类的类型</param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type / 对象的生命周期</param>
        void Register(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton);

        /// <summary>
        /// Registers a type with it's implementation.
        /// 注册一个类型和它的实现
        /// </summary>
        /// <typeparam name="TType">Registering type / 注册类的类型</typeparam>
        /// <typeparam name="TImpl">The type that implements <see cref="TType"/> / 类 <see cref="TType"/>的实现</typeparam>
        /// <param name="lifeStyle">Lifestyle of the objects of this type / 对象的生命周期</param>
        void Register<TType, TImpl>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where TType : class
            where TImpl : class, TType;

        /// <summary>
        /// Registers a type with it's implementation.
        /// 注册一个类型和它的实现
        /// </summary>
        /// <param name="type">Type of the class / 注册类的类型</param>
        /// <param name="impl">The type that implements <paramref name="type"/> / 类 <paramref name="type"/>的实现</param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type / 对象的生命周期</param>
        void Register(Type type, Type impl, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton);

        /// <summary>
        /// Checks whether given type is registered before.
        /// 检查给定的类型是否已经注册
        /// </summary>
        /// <param name="type">Type to check / 需要检查的类型</param>
        bool IsRegistered(Type type);

        /// <summary>
        /// Checks whether given type is registered before.
        /// 检查给定的类型是否已经注册
        /// </summary>
        /// <typeparam name="TType">Type to check / 需要检查的类型</typeparam>
        bool IsRegistered<TType>();
    }
}