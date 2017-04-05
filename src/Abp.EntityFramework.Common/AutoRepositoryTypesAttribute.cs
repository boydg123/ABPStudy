using System;

namespace Abp.EntityFramework
{
    /// <summary>
    /// Used to define auto-repository types for entities.This can be used for DbContext types.
    /// 用于定义实体的自动仓储类型，这可以被数据库上下文类型使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoRepositoryTypesAttribute : Attribute
    {
        /// <summary>
        /// 仓储接口
        /// </summary>
        public Type RepositoryInterface { get; private set; }

        /// <summary>
        /// 具有主键的仓储接口
        /// </summary>
        public Type RepositoryInterfaceWithPrimaryKey { get; private set; }

        /// <summary>
        /// 仓储实现
        /// </summary>
        public Type RepositoryImplementation { get; private set; }

        /// <summary>
        /// 具有主键的仓储实现
        /// </summary>
        public Type RepositoryImplementationWithPrimaryKey { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repositoryInterface">仓储接口</param>
        /// <param name="repositoryInterfaceWithPrimaryKey">具有主键的仓储接口</param>
        /// <param name="repositoryImplementation">仓储实现</param>
        /// <param name="repositoryImplementationWithPrimaryKey">具有主键的仓储实现</param>
        public AutoRepositoryTypesAttribute(
            Type repositoryInterface,
            Type repositoryInterfaceWithPrimaryKey,
            Type repositoryImplementation,
            Type repositoryImplementationWithPrimaryKey)
        {
            RepositoryInterface = repositoryInterface;
            RepositoryInterfaceWithPrimaryKey = repositoryInterfaceWithPrimaryKey;
            RepositoryImplementation = repositoryImplementation;
            RepositoryImplementationWithPrimaryKey = repositoryImplementationWithPrimaryKey;
        }
    }
}