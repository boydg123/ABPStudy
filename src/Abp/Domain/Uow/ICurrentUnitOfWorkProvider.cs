namespace Abp.Domain.Uow
{
    /// <summary>
    /// Used to get/set current <see cref="IUnitOfWork"/>. 
    /// 用于获取或设置当前 <see cref="IUnitOfWork"/>. 
    /// </summary>
    public interface ICurrentUnitOfWorkProvider
    {
        /// <summary>
        /// Gets/sets current <see cref="IUnitOfWork"/>.
        /// 获取/设置当前 <see cref="IUnitOfWork"/>.
        /// </summary>
        IUnitOfWork Current { get; set; }
    }
}