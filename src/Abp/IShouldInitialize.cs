using Castle.Core;

namespace Abp
{
    /// <summary>
    /// Defines interface for objects those should be Initialized before using it.If the object resolved using dependency injection, <see cref="IInitializable.Initialize"/>.method is automatically called just after creation of the object.
    /// 为使用前需要初始化的对象定义的接口。如果使用依赖注册进行对象解析，方法<see cref="IInitializable.Initialize"/>会在对象创建后自动调用
    /// </summary>
    public interface IShouldInitialize : IInitializable
    {
        
    }
}