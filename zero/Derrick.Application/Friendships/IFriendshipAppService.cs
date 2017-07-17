using System.Threading.Tasks;
using Abp.Application.Services;
using Derrick.Friendships.Dto;

namespace Derrick.Friendships
{
    /// <summary>
    /// 好友服务
    /// </summary>
    public interface IFriendshipAppService : IApplicationService
    {
        /// <summary>
        /// 创建好友请求
        /// </summary>
        /// <param name="input">创建好友请求Input</param>
        /// <returns></returns>
        Task<FriendDto> CreateFriendshipRequest(CreateFriendshipRequestInput input);
        /// <summary>
        /// 通过用户名创建好友请求
        /// </summary>
        /// <param name="input">通过用户名创建好友请求Input</param>
        /// <returns></returns>
        Task<FriendDto> CreateFriendshipRequestByUserName(CreateFriendshipRequestByUserNameInput input);
        /// <summary>
        /// 阻止用户
        /// </summary>
        /// <param name="input">阻止用户Input</param>
        void BlockUser(BlockUserInput input);
        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="input">解锁用户Input</param>
        void UnblockUser(UnblockUserInput input);
        /// <summary>
        /// 接受好友请求
        /// </summary>
        /// <param name="input">接受好友请求Input</param>
        void AcceptFriendshipRequest(AcceptFriendshipRequestInput input);
    }
}
