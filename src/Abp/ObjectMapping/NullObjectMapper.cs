using Abp.Dependency;

namespace Abp.ObjectMapping
{
    /// <summary>
    /// <see cref="NULL"/>对象映射器
    /// </summary>
    public sealed class NullObjectMapper : IObjectMapper, ISingletonDependency
    {
        /// <summary>
        /// Singleton instance.
        /// 单实例
        /// </summary>
        public static NullObjectMapper Instance { get { return SingletonInstance; } }
        private static readonly NullObjectMapper SingletonInstance = new NullObjectMapper();

        /// <summary>
        /// 转换一个对象至另一个对象。创建一个新的<see cref="TDestination"/>对象。
        /// </summary>
        /// <typeparam name="TDestination">目标对象的类型</typeparam>
        /// <param name="source">原对象</param>
        /// <returns></returns>
        public TDestination Map<TDestination>(object source)
        {
            throw new AbpException("Abp.ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }

        /// <summary>
        /// 执行一个映射从原对象到一个已存在的目标对象
        /// </summary>
        /// <typeparam name="TSource">原对象类型</typeparam>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">原对象</param>
        /// <param name="destination">目标对象</param>
        /// <returns>映射操作后的对象</returns>
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new AbpException("Abp.ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }
    }
}