using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Synchronizes a user's information to user account.
    /// 同步一个用户信息到用户帐号
    /// </summary>
    public class UserAccountSynchronizer :
        IEventHandler<EntityCreatedEventData<AbpUserBase>>,
        IEventHandler<EntityDeletedEventData<AbpUserBase>>,
        IEventHandler<EntityUpdatedEventData<AbpUserBase>>,
        ITransientDependency
    {
        /// <summary>
        /// 用户帐号仓储
        /// </summary>
        private readonly IRepository<UserAccount, long> _userAccountRepository;
        /// <summary>
        /// 工作单元管理引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserAccountSynchronizer(
            IRepository<UserAccount, long> userAccountRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _userAccountRepository = userAccountRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// 处理用户的创建事件
        /// </summary>
        [UnitOfWork]
        public virtual void HandleEvent(EntityCreatedEventData<AbpUserBase> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                _userAccountRepository.Insert(new UserAccount
                {
                    TenantId = eventData.Entity.TenantId,
                    UserName = eventData.Entity.UserName,
                    UserId = eventData.Entity.Id,
                    EmailAddress = eventData.Entity.EmailAddress,
                    LastLoginTime = eventData.Entity.LastLoginTime
                });
            }
        }

        /// <summary>
        /// Handles deletion event of user
        /// 处理用户的删除事件
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<AbpUserBase> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                var userAccount =
                    _userAccountRepository.FirstOrDefault(
                        ua => ua.TenantId == eventData.Entity.TenantId && ua.UserId == eventData.Entity.Id);
                if (userAccount != null)
                {
                    _userAccountRepository.Delete(userAccount);
                }
            }
        }

        /// <summary>
        /// Handles update event of user
        /// 处理用户的修改事件
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityUpdatedEventData<AbpUserBase> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                var userAccount = _userAccountRepository.FirstOrDefault(ua => ua.TenantId == eventData.Entity.TenantId && ua.UserId == eventData.Entity.Id);
                if (userAccount != null)
                {
                    userAccount.UserName = eventData.Entity.UserName;
                    userAccount.EmailAddress = eventData.Entity.EmailAddress;
                    userAccount.LastLoginTime = eventData.Entity.LastLoginTime;
                    _userAccountRepository.Update(userAccount);
                }
            }
        }
    }
}