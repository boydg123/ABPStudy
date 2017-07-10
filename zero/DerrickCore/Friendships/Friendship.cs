using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace Derrick.Friendships
{
    /// <summary>
    /// 好友
    /// </summary>
    [Table("AppFriendships")]
    public class Friendship : Entity<long>, IHasCreationTime, IMayHaveTenant
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 朋友用户ID
        /// </summary>
        public long FriendUserId { get; set; }
        /// <summary>
        /// 朋友商户ID
        /// </summary>
        public int? FriendTenantId { get; set; }
        /// <summary>
        /// 朋友用户名
        /// </summary>
        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string FriendUserName { get; set; }
        /// <summary>
        /// 朋友商户名
        /// </summary>
        public string FriendTenancyName { get; set; }
        /// <summary>
        /// 朋友照片ID
        /// </summary>
        public Guid? FriendProfilePictureId { get; set; }
        /// <summary>
        /// 友谊状态
        /// </summary>
        public FriendshipState State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="user">用户标识</param>
        /// <param name="probableFriend">可能的朋友标识</param>
        /// <param name="probableFriendTenancyName">可能的朋友商户名称</param>
        /// <param name="probableFriendUserName">可能的朋友用户名</param>
        /// <param name="probableFriendProfilePictureId">可能的朋友照片ID</param>
        /// <param name="state">友谊状态</param>
        public Friendship(UserIdentifier user, UserIdentifier probableFriend, string probableFriendTenancyName, string probableFriendUserName, Guid? probableFriendProfilePictureId, FriendshipState state)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (probableFriend == null)
            {
                throw new ArgumentNullException(nameof(probableFriend));
            }

            if (!Enum.IsDefined(typeof(FriendshipState), state))
            {
                throw new InvalidEnumArgumentException(nameof(state), (int)state, typeof(FriendshipState));
            }

            UserId = user.UserId;
            TenantId = user.TenantId;
            FriendUserId = probableFriend.UserId;
            FriendTenantId = probableFriend.TenantId;
            FriendTenancyName = probableFriendTenancyName;
            FriendUserName = probableFriendUserName;
            State = state;
            FriendProfilePictureId = probableFriendProfilePictureId;

            CreationTime = Clock.Now;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        protected Friendship()
        {

        }
    }
}
