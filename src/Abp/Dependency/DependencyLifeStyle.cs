namespace Abp.Dependency
{
    /// <summary>
    /// Lifestyles of types used in dependency injection system.
    /// 在依赖注入系统中类型的生命周期
    /// </summary>
    public enum DependencyLifeStyle
    {
        /// <summary>
        /// Singleton object. Created a single object on first resolving and same instance is used for subsequent resolves.
        /// 单例，在第一次解析时创建对象，之后的解析将返回相同的对象
        /// </summary>
        Singleton,

        /// <summary>
        /// Transient object. Created one object for every resolving.
        /// 实时, 为每一次解析请求，创建一个新的对象
        /// </summary>
        Transient
    }
}