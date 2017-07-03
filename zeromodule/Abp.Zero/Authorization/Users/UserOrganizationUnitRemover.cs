using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Removes the user from all organization units when a user is deleted.
    /// 当用户是已删除状态，则移除用户的所有组织架构信息
    /// </summary>
    public class UserOrganizationUnitRemover : 
        IEventHandler<EntityDeletedEventData<AbpUserBase>>, 
        ITransientDependency
    {
        /// <summary>
        /// 用户组织仓储
        /// </summary>
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        /// <summary>
        /// 工作单元引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userOrganizationUnitRepository">用户组织仓储</param>
        /// <param name="unitOfWorkManager">工作单元引用</param>
        public UserOrganizationUnitRemover(
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, 
            IUnitOfWorkManager unitOfWorkManager)
        {
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// 处理用户删除事件
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<AbpUserBase> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(eventData.Entity.TenantId))
            {
                _userOrganizationUnitRepository.Delete(
                    uou => uou.UserId == eventData.Entity.Id
                );
            }
        }
    }
}