using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Removes the user from all organization units when a user is deleted.
    /// 当一个用户删除的时候，删除用户所有的角色信息
    /// </summary>
    public class UserRoleRemover :
        IEventHandler<EntityDeletedEventData<AbpUserBase>>,
        ITransientDependency
    {
        /// <summary>
        /// 用户角色仓储
        /// </summary>
        private readonly IRepository<UserRole, long> _userRoleRepository;
        /// <summary>
        /// 工作单元引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManager">工作单元引用</param>
        /// <param name="userRoleRepository">用户角色仓储</param>
        public UserRoleRemover(
            IUnitOfWorkManager unitOfWorkManager, 
            IRepository<UserRole, long> userRoleRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 处理用户删除事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<AbpUserBase> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(eventData.Entity.TenantId))
            {
                _userRoleRepository.Delete(
                    ur => ur.UserId == eventData.Entity.Id
                );
            }
        }
    }
}