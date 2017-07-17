using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Derrick.Authorization.Users;
using Derrick.Friendships;

namespace Derrick.Chat.Dto
{
    /// <summary>
    /// 聊天用户Dto
    /// </summary>
    [AutoMapFrom(typeof(User))]
    public class ChatUserDto : EntityDto<long>
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 照片ID
        /// </summary>
        public Guid? ProfilePictureId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 商户名
        /// </summary>
        public string TenancyName { get; set; }
        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int UnreadMessageCount { get; set; }
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline { get; set; }
        /// <summary>
        /// 好友状态
        /// </summary>
        public FriendshipState State { get; set; }
    }
}