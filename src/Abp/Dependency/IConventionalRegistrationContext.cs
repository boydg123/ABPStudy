using System.Reflection;

namespace Abp.Dependency
{
    /// <summary>
    /// Used to pass needed objects on conventional registration process.
    /// 用于传递约定的注册过程需要的对象
    /// </summary>
    public interface IConventionalRegistrationContext
    {
        /// <summary>
        /// Gets the registering Assembly.
        /// 用于获取注册的程序集
        /// </summary>
        Assembly Assembly { get; }

        /// <summary>
        /// Reference to the IOC Container to register types.
        /// 引用用于注册的IOC容器
        /// </summary>
        IIocManager IocManager { get; }

        /// <summary>
        /// Registration configuration.
        /// 注册配置
        /// </summary>
        ConventionalRegistrationConfig Config { get; }
    }
}