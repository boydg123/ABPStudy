using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Common.Dto;

namespace Derrick.Common
{
    /// <summary>
    /// 通用Lookup服务
    /// </summary>
    public interface ICommonLookupAppService : IApplicationService
    {
        /// <summary>
        /// 为Combobox获取版本
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<ComboboxItemDto>> GetEditionsForCombobox();
        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="input">查找用户Input</param>
        /// <returns></returns>
        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);
        /// <summary>
        /// 获取默认版本名称
        /// </summary>
        /// <returns></returns>
        string GetDefaultEditionName();
    }
}