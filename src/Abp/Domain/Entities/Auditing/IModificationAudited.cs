namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities that is wanted to store modification information (who and when modified lastly).
    /// 需存储修改信息的实体需实现此接口(实体最后修改者/修改时间)
    /// Properties are automatically set when updating the <see cref="IEntity"/>.
    /// 当实体<see cref="IEntity"/>修改时该属性自动赋值
    /// </summary>
    public interface IModificationAudited : IHasModificationTime
    {
        /// <summary>
        /// Last modifier user for this entity. / 实体的最后修改人ID
        /// </summary>
        long? LastModifierUserId { get; set; }
    }

    /// <summary>
    /// Adds navigation properties to <see cref="IModificationAudited"/> interface for user.
    /// 为用户添加 <see cref="IModificationAudited"/> 接口的导航属性
    /// </summary>
    /// <typeparam name="TUser">Type of the user / 用户类型</typeparam>
    public interface IModificationAudited<TUser> : IModificationAudited
        where TUser : IEntity<long>
    {
        /// <summary>
        /// Reference to the last modifier user of this entity.
        /// 实体最后修改人的引用
        /// </summary>
        TUser LastModifierUser { get; set; }
    }
}