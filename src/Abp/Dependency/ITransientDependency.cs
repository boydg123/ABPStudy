namespace Abp.Dependency
{
    /// <summary>
    /// All classes implement this interface are automatically registered to dependency injection as transient object.
    /// 所有实现此接口的类会被自动注册到依赖注入为<see cref="DependencyLifeStyle.Transient"/>模式
    /// </summary>
    public interface ITransientDependency
    {

    }
}