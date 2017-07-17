using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.MultiTenancy;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.UI;
using Derrick.Authorization.Users;
using Derrick.Chat;
using Derrick.Friendships.Dto;

namespace Derrick.Friendships
{
    /// <summary>
    /// 好友服务实现
    /// </summary>
    [AbpAuthorize]
    public class FriendshipAppService : AbpZeroTemplateAppServiceBase, IFriendshipAppService
    {
        /// <summary>
        /// 好友管理器
        /// </summary>
        private readonly IFriendshipManager _friendshipManager;
        /// <summary>
        /// 在线客户端管理器
        /// </summary>
        private readonly IOnlineClientManager _onlineClientManager;
        /// <summary>
        /// 聊天沟通器
        /// </summary>
        private readonly IChatCommunicator _chatCommunicator;
        /// <summary>
        /// 商户缓存
        /// </summary>
        private readonly ITenantCache _tenantCache;
        /// <summary>
        /// 聊天功能检查器
        /// </summary>
        private readonly IChatFeatureChecker _chatFeatureChecker;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="friendshipManager">好友管理</param>
        /// <param name="onlineClientManager">在线客户端管理</param>
        /// <param name="chatCommunicator">聊天沟通</param>
        /// <param name="tenantCache">商户缓存</param>
        /// <param name="chatFeatureChecker">聊天功能检查</param>
        public FriendshipAppService(
            IFriendshipManager friendshipManager,
            IOnlineClientManager onlineClientManager,
            IChatCommunicator chatCommunicator,
            ITenantCache tenantCache,
            IChatFeatureChecker chatFeatureChecker)
        {
            _friendshipManager = friendshipManager;
            _onlineClientManager = onlineClientManager;
            _chatCommunicator = chatCommunicator;
            _tenantCache = tenantCache;
            _chatFeatureChecker = chatFeatureChecker;
        }

        /// <summary>
        /// 创建好友请求
        /// </summary>
        /// <param name="input">创建好友请求Input</param>
        /// <returns></returns>
        public async Task<FriendDto> CreateFriendshipRequest(CreateFriendshipRequestInput input)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            var probableFriend = new UserIdentifier(input.TenantId, input.UserId);

            _chatFeatureChecker.CheckChatFeatures(userIdentifier.TenantId, probableFriend.TenantId);

            if (_friendshipManager.GetFriendshipOrNull(userIdentifier, probableFriend) != null)
            {
                throw new UserFriendlyException(L("YouAlreadySentAFriendshipRequestToThisUser"));
            }

            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId());

            User probableFriendUser;
            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                probableFriendUser = (await UserManager.FindByIdAsync(input.UserId));
            }

            var friendTenancyName = probableFriend.TenantId.HasValue ? _tenantCache.Get(probableFriend.TenantId.Value).TenancyName : null;
            var sourceFriendship = new Friendship(userIdentifier, probableFriend, friendTenancyName, probableFriendUser.UserName, probableFriendUser.ProfilePictureId, FriendshipState.Accepted);
            _friendshipManager.CreateFriendship(sourceFriendship);

            var userTenancyName = user.TenantId.HasValue ? _tenantCache.Get(user.TenantId.Value).TenancyName : null;
            var targetFriendship = new Friendship(probableFriend, userIdentifier, userTenancyName, user.UserName, user.ProfilePictureId, FriendshipState.Accepted);
            _friendshipManager.CreateFriendship(targetFriendship);

            var clients = _onlineClientManager.GetAllByUserId(probableFriend);
            if (clients.Any())
            {
                var isFriendOnline = _onlineClientManager.IsOnline(sourceFriendship.ToUserIdentifier());
                _chatCommunicator.SendFriendshipRequestToClient(clients, targetFriendship, false, isFriendOnline);
            }

            var senderClients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (senderClients.Any())
            {
                var isFriendOnline = _onlineClientManager.IsOnline(targetFriendship.ToUserIdentifier());
                _chatCommunicator.SendFriendshipRequestToClient(senderClients, sourceFriendship, true, isFriendOnline);
            }

            var sourceFriendshipRequest = sourceFriendship.MapTo<FriendDto>();
            sourceFriendshipRequest.IsOnline = _onlineClientManager.GetAllByUserId(probableFriend).Any();

            return sourceFriendshipRequest;
        }
        /// <summary>
        /// 通过用户名创建好友请求
        /// </summary>
        /// <param name="input">通过用户名创建好友请求Input</param>
        /// <returns></returns>
        public async Task<FriendDto> CreateFriendshipRequestByUserName(CreateFriendshipRequestByUserNameInput input)
        {
            var probableFriend = await GetUserIdentifier(input.TenancyName, input.UserName);
            return await CreateFriendshipRequest(new CreateFriendshipRequestInput
            {
                TenantId = probableFriend.TenantId,
                UserId = probableFriend.UserId
            });
        }
        /// <summary>
        /// 阻止用户
        /// </summary>
        /// <param name="input">阻止用户Input</param>
        public void BlockUser(BlockUserInput input)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            var friendIdentifier = new UserIdentifier(input.TenantId, input.UserId);
            _friendshipManager.BanFriend(userIdentifier, friendIdentifier);

            var clients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (clients.Any())
            {
                _chatCommunicator.SendUserStateChangeToClients(clients, friendIdentifier, FriendshipState.Blocked);
            }
        }
        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="input">解锁用户Input</param>
        public void UnblockUser(UnblockUserInput input)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            var friendIdentifier = new UserIdentifier(input.TenantId, input.UserId);
            _friendshipManager.AcceptFriendshipRequest(userIdentifier, friendIdentifier);

            var clients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (clients.Any())
            {
                _chatCommunicator.SendUserStateChangeToClients(clients, friendIdentifier, FriendshipState.Accepted);
            }
        }
        /// <summary>
        /// 接受好友请求
        /// </summary>
        /// <param name="input">接受好友请求Input</param>
        public void AcceptFriendshipRequest(AcceptFriendshipRequestInput input)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            var friendIdentifier = new UserIdentifier(input.TenantId, input.UserId);
            _friendshipManager.AcceptFriendshipRequest(userIdentifier, friendIdentifier);

            var clients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (clients.Any())
            {
                _chatCommunicator.SendUserStateChangeToClients(clients, friendIdentifier, FriendshipState.Blocked);
            }
        }
        /// <summary>
        /// 获取用户标识
        /// </summary>
        /// <param name="tenancyName">商户名</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        private async Task<UserIdentifier> GetUserIdentifier(string tenancyName, string userName)
        {
            int? tenantId = null;
            if (!tenancyName.Equals("."))
            {
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    var tenant = await TenantManager.FindByTenancyNameAsync(tenancyName);
                    if (tenant == null)
                    {
                        throw new UserFriendlyException("There is no such tenant !");
                    }

                    tenantId = tenant.Id;
                }
            }

            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var user = await UserManager.FindByNameOrEmailAsync(userName);
                if (user == null)
                {
                    throw new UserFriendlyException("There is no such user !");
                }

                return user.ToUserIdentifier();
            }
        }
    }
}