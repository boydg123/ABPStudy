namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities which wanted to store deletion information (who and when deleted).
    /// 需存储删除信息的实体需实现此接口(实体删除者/删除时间)
    /// </summary>
    public interface IDeletionAudited : IHasDeletionTime
    {
        /// <summary>
        /// Which user deleted this entity? / 删除实体的用户ID
        /// </summary>
        long? DeleterUserId { get; set; }
    }

    /// <summary>
    /// Adds navigation properties to <see cref="IDeletionAudited"/> interface for user.
    /// 为用户添加 <see cref="IDeletionAudited"/> 接口的导航属性
    /// </summary>
    /// <typeparam name="TUser">Type of the user / 用户类型</typeparam>
    public interface IDeletionAudited<TUser> : IDeletionAudited
        where TUser : IEntity<long>
    {
        /// <summary>
        /// Reference to the deleter user of this entity.
        /// 删除实体用户的引用
        /// </summary>
        TUser DeleterUser { get; set; }
    }
}