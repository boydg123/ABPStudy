namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities which must be audited.
    /// 审计的实体需要实现此接口
    /// Related properties automatically set when saving/updating <see cref="Entity"/> objects.
    /// 关联的属性自动被设置当 保存/修改 <see cref="Entity"/> 对象
    /// </summary>
    public interface IAudited : ICreationAudited, IModificationAudited
    {

    }

    /// <summary>
    /// Adds navigation properties to <see cref="IAudited"/> interface for user.
    /// 为用户添加 <see cref="IAudited"/> 接口的导航属性
    /// </summary>
    /// <typeparam name="TUser">Type of the user / 用户类型</typeparam>
    public interface IAudited<TUser> : IAudited, ICreationAudited<TUser>, IModificationAudited<TUser>
        where TUser : IEntity<long>
    {

    }
}