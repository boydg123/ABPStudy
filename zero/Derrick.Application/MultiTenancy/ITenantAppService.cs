using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.MultiTenancy.Dto;

namespace Derrick.MultiTenancy
{
    /// <summary>
    /// 商户服务
    /// </summary>
    public interface ITenantAppService : IApplicationService
    {
        /// <summary>
        /// 获取商户列表(带分页)
        /// </summary>
        /// <param name="input">获取商户Input</param>
        /// <returns></returns>
        Task<PagedResultDto<TenantListDto>> GetTenants(GetTenantsInput input);
        /// <summary>
        /// 创建商户
        /// </summary>
        /// <param name="input">创建商户Input</param>
        /// <returns></returns>
        Task CreateTenant(CreateTenantInput input);
        /// <summary>
        /// 获取编辑商户
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task<TenantEditDto> GetTenantForEdit(EntityDto input);
        /// <summary>
        /// 更新商户
        /// </summary>
        /// <param name="input">商户编辑Dto</param>
        /// <returns></returns>
        Task UpdateTenant(TenantEditDto input);
        /// <summary>
        /// 删除商户
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task DeleteTenant(EntityDto input);
        /// <summary>
        /// 获取编辑时商户功能
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task<GetTenantFeaturesForEditOutput> GetTenantFeaturesForEdit(EntityDto input);
        /// <summary>
        /// 更新商户功能
        /// </summary>
        /// <param name="input">更新商户功能Input</param>
        /// <returns></returns>
        Task UpdateTenantFeatures(UpdateTenantFeaturesInput input);
        /// <summary>
        /// 重置商户指定的功能
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task ResetTenantSpecificFeatures(EntityDto input);
    }
}
