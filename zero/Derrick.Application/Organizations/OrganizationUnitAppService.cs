using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Derrick.Authorization;
using Derrick.Organizations.Dto;
using System.Linq.Dynamic;

namespace Derrick.Organizations
{
    /// <summary>
    /// 组织架构服务实现
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits)]
    public class OrganizationUnitAppService : AbpZeroTemplateAppServiceBase, IOrganizationUnitAppService
    {
        /// <summary>
        /// 组织架构管理器
        /// </summary>
        private readonly OrganizationUnitManager _organizationUnitManager;
        /// <summary>
        /// 组织架构仓储
        /// </summary>
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        /// <summary>
        /// 用户组织架构仓储
        /// </summary>
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="organizationUnitManager">组织架构管理</param>
        /// <param name="organizationUnitRepository">组织架构仓储</param>
        /// <param name="userOrganizationUnitRepository">用户组织架构仓储</param>
        public OrganizationUnitAppService(
            OrganizationUnitManager organizationUnitManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository)
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
        }
        /// <summary>
        /// 获取组织架构
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnits()
        {
            var query =
                from ou in _organizationUnitRepository.GetAll()
                join uou in _userOrganizationUnitRepository.GetAll() on ou.Id equals uou.OrganizationUnitId into g
                select new { ou, memberCount = g.Count() };

            var items = await query.ToListAsync();

            return new ListResultDto<OrganizationUnitDto>(
                items.Select(item =>
                {
                    var dto = item.ou.MapTo<OrganizationUnitDto>();
                    dto.MemberCount = item.memberCount;
                    return dto;
                }).ToList());
        }
        /// <summary>
        /// 获取组织架构用户
        /// </summary>
        /// <param name="input">获取组织架构用户Input</param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input)
        {
            var query = from uou in _userOrganizationUnitRepository.GetAll()
                        join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
                        join user in UserManager.Users on uou.UserId equals user.Id
                        where uou.OrganizationUnitId == input.Id
                        select new { uou, user };

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<OrganizationUnitUserListDto>(
                totalCount,
                items.Select(item =>
                {
                    var dto = item.user.MapTo<OrganizationUnitUserListDto>();
                    dto.AddedTime = item.uou.CreationTime;
                    return dto;
                }).ToList());
        }
        /// <summary>
        /// 创建组织架构
        /// </summary>
        /// <param name="input">创建组织架构Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input)
        {
            var organizationUnit = new OrganizationUnit(AbpSession.TenantId, input.DisplayName, input.ParentId);

            await _organizationUnitManager.CreateAsync(organizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return organizationUnit.MapTo<OrganizationUnitDto>();
        }
        /// <summary>
        /// 更新组织架构
        /// </summary>
        /// <param name="input">更新组织架构Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(input.Id);

            organizationUnit.DisplayName = input.DisplayName;

            await _organizationUnitManager.UpdateAsync(organizationUnit);

            return await CreateOrganizationUnitDto(organizationUnit);
        }
        /// <summary>
        /// 转移组织架构
        /// </summary>
        /// <param name="input">转移组织架构Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input)
        {
            await _organizationUnitManager.MoveAsync(input.Id, input.NewParentId);

            return await CreateOrganizationUnitDto(
                await _organizationUnitRepository.GetAsync(input.Id)
                );
        }
        /// <summary>
        /// 删除组织架构
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task DeleteOrganizationUnit(EntityDto<long> input)
        {
            await _organizationUnitManager.DeleteAsync(input.Id);
        }
        /// <summary>
        /// 添加用户到组织架构
        /// </summary>
        /// <param name="input">用户到组织架构Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task AddUserToOrganizationUnit(UserToOrganizationUnitInput input)
        {
            await UserManager.AddToOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }
        /// <summary>
        /// 从组织架构移除用户
        /// </summary>
        /// <param name="input">用户到组织架构Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task RemoveUserFromOrganizationUnit(UserToOrganizationUnitInput input)
        {
            await UserManager.RemoveFromOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }
        /// <summary>
        /// 判断用户是否在组织中
        /// </summary>
        /// <param name="input">用户到组织架构Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task<bool> IsInOrganizationUnit(UserToOrganizationUnitInput input)
        {
            return await UserManager.IsInOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }
        /// <summary>
        /// 创建组织架构Dto
        /// </summary>
        /// <param name="organizationUnit">组织架构</param>
        /// <returns></returns>
        private async Task<OrganizationUnitDto> CreateOrganizationUnitDto(OrganizationUnit organizationUnit)
        {
            var dto = organizationUnit.MapTo<OrganizationUnitDto>();
            dto.MemberCount = await _userOrganizationUnitRepository.CountAsync(uou => uou.OrganizationUnitId == organizationUnit.Id);
            return dto;
        }
    }
}