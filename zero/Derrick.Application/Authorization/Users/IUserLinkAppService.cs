using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Authorization.Users.Dto;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// 用户链接APP服务
    /// </summary>
    public interface IUserLinkAppService : IApplicationService
    {
        /// <summary>
        /// 链接到用户
        /// </summary>
        /// <param name="linkToUserInput">用户连接Input信息</param>
        /// <returns></returns>
        Task LinkToUser(LinkToUserInput linkToUserInput);
        /// <summary>
        /// 获取连接用户Dto列表(带分页)
        /// </summary>
        /// <param name="input">用户链接Input</param>
        /// <returns></returns>
        Task<PagedResultDto<LinkedUserDto>> GetLinkedUsers(GetLinkedUsersInput input);
        /// <summary>
        /// 获取最近使用的链接用户
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<LinkedUserDto>> GetRecentlyUsedLinkedUsers();
        /// <summary>
        /// 用户断开链接
        /// </summary>
        /// <param name="input">断开链接用户Input</param>
        /// <returns></returns>
        Task UnlinkUser(UnlinkUserInput input);
    }
}
