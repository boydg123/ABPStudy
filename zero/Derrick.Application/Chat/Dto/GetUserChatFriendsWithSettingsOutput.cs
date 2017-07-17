using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Castle.Components.DictionaryAdapter;
using Derrick.Friendships.Dto;

namespace Derrick.Chat.Dto
{
    /// <summary>
    /// 用户聊天好友以及设置Output
    /// </summary>
    public class GetUserChatFriendsWithSettingsOutput
    {
        /// <summary>
        /// 服务器时间
        /// </summary>
        public DateTime ServerTime { get; set; }
        /// <summary>
        /// 好友Dto列表
        /// </summary>
        public List<FriendDto> Friends { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetUserChatFriendsWithSettingsOutput()
        {
            Friends = new EditableList<FriendDto>();
        }
    }
}