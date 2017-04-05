using Abp.Configuration;
using Castle.DynamicProxy;

namespace Abp.Dependency
{
    /// <summary>
    /// This class is used to pass configuration/options while registering classes in conventional way.
    /// 这个类用于，在使用约定注册类时传递配置/选项
    /// </summary>
    public class ConventionalRegistrationConfig : DictionaryBasedConfig
    {
        /// <summary>
        /// Install all <see cref="IInterceptor"/> implementations automatically or not.Default: true. 
        /// 是否自动安装所有的 <see cref="IInterceptor"/> 实现.默认: 是. 
        /// </summary>
        public bool InstallInstallers { get; set; }

        /// <summary>
        /// Creates a new <see cref="ConventionalRegistrationConfig"/> object.
        /// 创建一个新的 <see cref="ConventionalRegistrationConfig"/> 对象.
        /// </summary>
        public ConventionalRegistrationConfig()
        {
            InstallInstallers = true;
        }
    }
}