using System.Reflection;

namespace Abp.Dependency
{
    /// <summary>
    /// This class is used to pass needed objects on conventional registration process.
    /// 用于传递约定的注册过程需要的对象
    /// </summary>
    internal class ConventionalRegistrationContext : IConventionalRegistrationContext
    {
        /// <summary>
        /// Gets the registering Assembly.
        /// 用于获取注册的程序集
        /// </summary>
        public Assembly Assembly { get; private set; }

        /// <summary>
        /// Reference to the IOC Container to register types.
        /// 引用用于注册的IOC容器
        /// </summary>
        public IIocManager IocManager { get; private set; }

        /// <summary>
        /// Registration configuration.
        /// 注册配置
        /// </summary>
        public ConventionalRegistrationConfig Config { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="assembly">用于获取注册的程序集</param>
        /// <param name="iocManager">引用用于注册的IOC容器</param>
        /// <param name="config">注册配置</param>
        internal ConventionalRegistrationContext(Assembly assembly, IIocManager iocManager, ConventionalRegistrationConfig config)
        {
            Assembly = assembly;
            IocManager = iocManager;
            Config = config;
        }
    }
}