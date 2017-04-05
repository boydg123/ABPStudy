namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities that is wanted to store creation information (who and when created).
    /// 需存储创建信息的实体需实现此接口(实体的创建者/创建时间)
    /// Creation time and creator user are automatically set when saving <see cref="Entity"/> to database.
    /// </summary>
    public interface ICreationAudited : IHasCreationTime
    {
        /// <summary>
        /// Id of the creator user of this entity. / 实体创建者ID
        /// </summary>
        long? CreatorUserId { get; set; }
    }

    /// <summary>
    /// Adds navigation properties to <see cref="ICreationAudited"/> interface for user.
    /// 为用户添加 <see cref="ICreationAudited"/> 接口的导航属性
    /// </summary>
    /// <typeparam name="TUser">Type of the user / 用户类型</typeparam>
    public interface ICreationAudited<TUser> : ICreationAudited
        where TUser : IEntity<long>
    {
        /// <summary>
        /// Reference to the creator user of this entity.
        /// 删除实体用户的引用
        /// </summary>
        TUser CreatorUser { get; set; }
    }
}