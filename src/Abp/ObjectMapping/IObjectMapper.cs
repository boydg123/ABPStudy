namespace Abp.ObjectMapping
{
    /// <summary>
    /// Defines a simple interface to map objects.
    /// 定义一个简单的接口用于映射对象
    /// </summary>
    public interface IObjectMapper
    {
        /// <summary>
        /// Converts an object to another. Creates a new object of <see cref="TDestination"/>.
        /// 转换一个对象至另一个对象。创建一个新的<see cref="TDestination"/>对象。
        /// </summary>
        /// <typeparam name="TDestination">Type of the destination object / 目标对象的类型</typeparam>
        /// <param name="source">Source object / 原对象</param>
        TDestination Map<TDestination>(object source);

        /// <summary>
        /// Execute a mapping from the source object to the existing destination object
        /// 执行一个映射从原对象到一个已存在的目标对象
        /// </summary>
        /// <typeparam name="TSource">Source type / 原对象类型</typeparam>
        /// <typeparam name="TDestination">Destination type / 目标对象类型</typeparam>
        /// <param name="source">Source object / 原对象</param>
        /// <param name="destination">Destination object / 目标对象</param>
        /// <returns>Returns the same <see cref="destination"/> object after mapping operation / 映射操作后的对象</returns>
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
