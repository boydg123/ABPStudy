using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Zero;

namespace Abp.Organizations
{
    /// <summary>
    /// Performs domain logic for Organization Units.
    /// 为组织单元执行域逻辑
    /// </summary>
    public class OrganizationUnitManager : DomainService
    {
        /// <summary>
        /// 组织架构仓储引用
        /// </summary>
        protected IRepository<OrganizationUnit, long> OrganizationUnitRepository { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="organizationUnitRepository">组织架构仓储引用</param>
        public OrganizationUnitManager(IRepository<OrganizationUnit, long> organizationUnitRepository)
        {
            OrganizationUnitRepository = organizationUnitRepository;

            LocalizationSourceName = AbpZeroConsts.LocalizationSourceName;
        }
        /// <summary>
        /// 创建组织架构
        /// </summary>
        /// <param name="organizationUnit">组织架构</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task CreateAsync(OrganizationUnit organizationUnit)
        {
            organizationUnit.Code = await GetNextChildCodeAsync(organizationUnit.ParentId);
            await ValidateOrganizationUnitAsync(organizationUnit);
            await OrganizationUnitRepository.InsertAsync(organizationUnit);
        }
        /// <summary>
        /// 更新组织架构
        /// </summary>
        /// <param name="organizationUnit">组织架构</param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(OrganizationUnit organizationUnit)
        {
            await ValidateOrganizationUnitAsync(organizationUnit);
            await OrganizationUnitRepository.UpdateAsync(organizationUnit);
        }
        /// <summary>
        /// 获取下一个子code
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <returns></returns>
        public virtual async Task<string> GetNextChildCodeAsync(long? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild == null)
            {
                var parentCode = parentId != null ? await GetCodeAsync(parentId.Value) : null;
                return OrganizationUnit.AppendCode(parentCode, OrganizationUnit.CreateCode(1));
            }

            return OrganizationUnit.CalculateNextCode(lastChild.Code);
        }
        /// <summary>
        /// 获取最后一个子组织或null(没有则返回null)
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <returns></returns>
        public virtual async Task<OrganizationUnit> GetLastChildOrNullAsync(long? parentId)
        {
            var children = await OrganizationUnitRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }
        /// <summary>
        /// 获取组织Code
        /// </summary>
        /// <param name="id">组织ID</param>
        /// <returns></returns>
        public virtual async Task<string> GetCodeAsync(long id)
        {
            return (await OrganizationUnitRepository.GetAsync(id)).Code;
        }
        /// <summary>
        /// 删除组织
        /// </summary>
        /// <param name="id">组织ID</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task DeleteAsync(long id)
        {
            var children = await FindChildrenAsync(id, true);

            foreach (var child in children)
            {
                await OrganizationUnitRepository.DeleteAsync(child);
            }

            await OrganizationUnitRepository.DeleteAsync(id);
        }
        /// <summary>
        /// 移动组织
        /// </summary>
        /// <param name="id">组织ID</param>
        /// <param name="parentId">父组织ID</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task MoveAsync(long id, long? parentId)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);
            if (organizationUnit.ParentId == parentId)
            {
                return;
            }

            //Should find children before Code change
            var children = await FindChildrenAsync(id, true);

            //Store old code of OU
            var oldCode = organizationUnit.Code;

            //Move OU
            organizationUnit.Code = await GetNextChildCodeAsync(parentId);
            organizationUnit.ParentId = parentId;

            await ValidateOrganizationUnitAsync(organizationUnit);

            //Update Children Codes
            foreach (var child in children)
            {
                child.Code = OrganizationUnit.AppendCode(organizationUnit.Code, OrganizationUnit.GetRelativeCode(child.Code, oldCode));
            }
        }
        /// <summary>
        /// 查找子组织列表
        /// </summary>
        /// <param name="parentId">父组织</param>
        /// <param name="recursive">是否递归</param>
        /// <returns></returns>
        public async Task<List<OrganizationUnit>> FindChildrenAsync(long? parentId, bool recursive = false)
        {
            if (!recursive)
            {
                return await OrganizationUnitRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            }

            if (!parentId.HasValue)
            {
                return await OrganizationUnitRepository.GetAllListAsync();
            }

            var code = await GetCodeAsync(parentId.Value);

            return await OrganizationUnitRepository.GetAllListAsync(
                ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value
            );
        }
        /// <summary>
        /// 验证组织
        /// </summary>
        /// <param name="organizationUnit">组织架构</param>
        /// <returns></returns>
        protected virtual async Task ValidateOrganizationUnitAsync(OrganizationUnit organizationUnit)
        {
            var siblings = (await FindChildrenAsync(organizationUnit.ParentId))
                .Where(ou => ou.Id != organizationUnit.Id)
                .ToList();

            if (siblings.Any(ou => ou.DisplayName == organizationUnit.DisplayName))
            {
                throw new UserFriendlyException(L("OrganizationUnitDuplicateDisplayNameWarning", organizationUnit.DisplayName));
            }
        }
    }
}