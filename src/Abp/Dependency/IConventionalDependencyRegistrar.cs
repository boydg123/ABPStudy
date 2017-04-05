namespace Abp.Dependency
{
    /// <summary>
    /// This interface is used to register dependencies by conventions. 
    /// 此接口用于约定注册依赖注入
    /// </summary>
    /// <remarks>
    /// Implement this interface and register to <see cref="IocManager.AddConventionalRegistrar"/> method to be able
    /// 实现这个接口并注册到 <see cref="IocManager.AddConventionalRegistrar"/> 方法，
    /// to register classes by your own conventions.
    /// 便按你自己的约定注册类
    /// </remarks>
    public interface IConventionalDependencyRegistrar
    {
        /// <summary>
        /// Registers types of given assembly by convention.
        /// 按照约定注册给定程序集的类型
        /// </summary>
        /// <param name="context">Registration context / 约定注册上下文</param>
        void RegisterAssembly(IConventionalRegistrationContext context);
    }
}