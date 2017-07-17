using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Editions.Dto;

namespace Derrick.Editions
{
    /// <summary>
    /// 版本服务
    /// </summary>
    public interface IEditionAppService : IApplicationService
    {
        /// <summary>
        /// 获取版本列表
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<EditionListDto>> GetEditions();

        /// <summary>
        /// 为编辑获取版本
        /// </summary>
        /// <param name="input">空ID Dto</param>
        /// <returns></returns>
        Task<GetEditionForEditOutput> GetEditionForEdit(NullableIdDto input);

        /// <summary>
        /// 创建或更新版本
        /// </summary>
        /// <param name="input">创建或更新版本Dto</param>
        /// <returns></returns>
        Task CreateOrUpdateEdition(CreateOrUpdateEditionDto input);

        /// <summary>
        /// 删除版本
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task DeleteEdition(EntityDto input);
        
        /// <summary>
        /// 获取Combobox版本集合
        /// </summary>
        /// <param name="selectedEditionId">选中版本的ID</param>
        /// <returns></returns>
        Task<List<ComboboxItemDto>> GetEditionComboboxItems(int? selectedEditionId = null);
    }
}