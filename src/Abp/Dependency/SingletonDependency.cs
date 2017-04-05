using System;

namespace Abp.Dependency
{
    /// <summary>
    /// Used to get a singleton of any class which can be resolved using <see cref="IocManager.Instance"/>.
    /// 用于获取任何一个可以被<see cref="IocManager.Instance"/>解析的类。
    /// Important: Use classes by injecting wherever possible. This class is for cases that's not possible.
    /// 主要提示:尽可能使用注入来使用此类。这个类不可能作为案例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class SingletonDependency<T>
    {
        /// <summary>
        /// Gets the instance.
        /// 获取实例
        /// </summary>
        /// <value>
        /// The instance.
        /// 实例
        /// </value>
        public static T Instance { get { return LazyInstance.Value; } }
        private static readonly Lazy<T> LazyInstance;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static SingletonDependency()
        {
            LazyInstance = new Lazy<T>(() => IocManager.Instance.Resolve<T>());
        }
    }
}
