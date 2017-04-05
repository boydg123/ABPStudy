namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface ads <see cref="IDeletionAudited"/> to <see cref="IAudited"/> for a fully audited entity.
    /// 此接口为一个拥有全部审计的实体添加<see cref="IDeletionAudited"/>到<see cref="IAudited"/>
    /// </summary>
    public interface IFullAudited : IAudited, IDeletionAudited
    {
        
    }

    /// <summary>
    /// Adds navigation properties to <see cref="IFullAudited"/> interface for user.
    /// 为用户添加 <see cref="IFullAudited"/> 接口的导航属性
    /// </summary>
    /// <typeparam name="TUser">Type of the user / 用户类型</typeparam>
    public interface IFullAudited<TUser> : IAudited<TUser>, IFullAudited, IDeletionAudited<TUser>
        where TUser : IEntity<long>
    {

    }
}