using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace Abp.Configuration
{
    /// <summary>
    /// Implements <see cref="ISettingStore"/>.
    /// <see cref="ISettingStore"/>的实现
    /// </summary>
    public class SettingStore : ISettingStore, ITransientDependency
    {
        /// <summary>
        /// 设置仓储引用
        /// </summary>
        private readonly IRepository<Setting, long> _settingRepository;
        /// <summary>
        /// 工作单元引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SettingStore(
            IRepository<Setting, long> settingRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _settingRepository = settingRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// 获取所有设置信息集合
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<List<SettingInfo>> GetAllListAsync(int? tenantId, long? userId)
        {
            /* Combined SetTenantId and DisableFilter for backward compatibility.
             * SetTenantId switches database (for tenant) if needed.
             * DisableFilter and Where condition ensures to work even if tenantId is null for single db approach.
             */
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    return
                        (await _settingRepository.GetAllListAsync(s => s.UserId == userId && s.TenantId == tenantId))
                        .Select(s => s.ToSettingInfo())
                        .ToList();
                }
            }
        }
        /// <summary>
        /// 获取设置信息或Null
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="name">设置名称</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<SettingInfo> GetSettingOrNullAsync(int? tenantId, long? userId, string name)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    return (await _settingRepository.FirstOrDefaultAsync(s => s.UserId == userId && s.Name == name && s.TenantId == tenantId))
                    .ToSettingInfo();
                }
            }
        }
        /// <summary>
        /// 删除设置
        /// </summary>
        /// <param name="settingInfo">设置信息</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task DeleteAsync(SettingInfo settingInfo)
        {
            using (_unitOfWorkManager.Current.SetTenantId(settingInfo.TenantId))
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    await _settingRepository.DeleteAsync(
                    s => s.UserId == settingInfo.UserId && s.Name == settingInfo.Name && s.TenantId == settingInfo.TenantId
                    );
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }
            }
        }
        /// <summary>
        /// 创建设置
        /// </summary>
        /// <param name="settingInfo">设置信息</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task CreateAsync(SettingInfo settingInfo)
        {
            using (_unitOfWorkManager.Current.SetTenantId(settingInfo.TenantId))
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    await _settingRepository.InsertAsync(settingInfo.ToSetting());
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }
            }
        }
        /// <summary>
        /// 修改设置
        /// </summary>
        /// <param name="settingInfo">设置信息</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task UpdateAsync(SettingInfo settingInfo)
        {
            using (_unitOfWorkManager.Current.SetTenantId(settingInfo.TenantId))
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    var setting = await _settingRepository.FirstOrDefaultAsync(
                        s => s.TenantId == settingInfo.TenantId &&
                             s.UserId == settingInfo.UserId &&
                             s.Name == settingInfo.Name
                        );

                    if (setting != null)
                    {
                        setting.Value = settingInfo.Value;
                    }

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }
            }
        }
    }
}