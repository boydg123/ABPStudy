using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Security;
using Derrick.Authorization;
using Derrick.Editions.Dto;
using Derrick.MultiTenancy.Dto;

namespace Derrick.MultiTenancy
{
    /// <summary>
    /// 商户服务实现
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_Tenants)]
    public class TenantAppService : AbpZeroTemplateAppServiceBase, ITenantAppService
    {
        /// <summary>
        /// 商户管理器
        /// </summary>
        private readonly TenantManager _tenantManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantManager"></param>
        public TenantAppService(
            TenantManager tenantManager)
        {
            _tenantManager = tenantManager;
        }
        /// <summary>
        /// 获取商户列表(带分页)
        /// </summary>
        /// <param name="input">获取商户Input</param>
        /// <returns></returns>
        public async Task<PagedResultDto<TenantListDto>> GetTenants(GetTenantsInput input)
        {
            var query = TenantManager.Tenants
                .Include(t => t.Edition)
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    t =>
                        t.Name.Contains(input.Filter) ||
                        t.TenancyName.Contains(input.Filter)
                );

            var tenantCount = await query.CountAsync();
            var tenants = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<TenantListDto>(
                tenantCount,
                tenants.MapTo<List<TenantListDto>>()
                );
        }
        /// <summary>
        /// 创建商户
        /// </summary>
        /// <param name="input">创建商户Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Tenants_Create)]
        [UnitOfWork(IsDisabled = true)]
        public async Task CreateTenant(CreateTenantInput input)
        {
            await _tenantManager.CreateWithAdminUserAsync(input.TenancyName,
                input.Name,
                input.AdminPassword,
                input.AdminEmailAddress,
                input.ConnectionString,
                input.IsActive,
                input.EditionId,
                input.ShouldChangePasswordOnNextLogin,
                input.SendActivationEmail);
        }
        /// <summary>
        /// 获取编辑商户
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Tenants_Edit)]
        public async Task<TenantEditDto> GetTenantForEdit(EntityDto input)
        {
            var tenantEditDto = (await TenantManager.GetByIdAsync(input.Id)).MapTo<TenantEditDto>();
            tenantEditDto.ConnectionString = SimpleStringCipher.Instance.Decrypt(tenantEditDto.ConnectionString);
            return tenantEditDto;
        }
        /// <summary>
        /// 更新商户
        /// </summary>
        /// <param name="input">商户编辑Dto</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Tenants_Edit)]
        public async Task UpdateTenant(TenantEditDto input)
        {
            input.ConnectionString = SimpleStringCipher.Instance.Encrypt(input.ConnectionString);
            var tenant = await TenantManager.GetByIdAsync(input.Id);
            input.MapTo(tenant);
            CheckErrors(await TenantManager.UpdateAsync(tenant));
        }
        /// <summary>
        /// 删除商户
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Tenants_Delete)]
        public async Task DeleteTenant(EntityDto input)
        {
            var tenant = await TenantManager.GetByIdAsync(input.Id);
            CheckErrors(await TenantManager.DeleteAsync(tenant));
        }
        /// <summary>
        /// 获取编辑时商户功能
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task<GetTenantFeaturesForEditOutput> GetTenantFeaturesForEdit(EntityDto input)
        {
            var features = FeatureManager.GetAll();
            var featureValues = await TenantManager.GetFeatureValuesAsync(input.Id);

            return new GetTenantFeaturesForEditOutput
            {
                Features = features.MapTo<List<FlatFeatureDto>>().OrderBy(f => f.DisplayName).ToList(),
                FeatureValues = featureValues.Select(fv => new NameValueDto(fv)).ToList()
            };
        }
        /// <summary>
        /// 更新商户功能
        /// </summary>
        /// <param name="input">更新商户功能Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task UpdateTenantFeatures(UpdateTenantFeaturesInput input)
        {
            await TenantManager.SetFeatureValuesAsync(input.Id, input.FeatureValues.Select(fv => new NameValue(fv.Name, fv.Value)).ToArray());
        }
        /// <summary>
        /// 重置商户指定的功能
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task ResetTenantSpecificFeatures(EntityDto input)
        {
            await TenantManager.ResetAllFeaturesAsync(input.Id);
        }
    }
}