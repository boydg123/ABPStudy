using System.ComponentModel.DataAnnotations;

namespace Derrick.Friendships.Dto
{
    /// <summary>
    /// 通过用户名创建好友请求Input
    /// </summary>
    public class CreateFriendshipRequestByUserNameInput
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public string TenancyName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
    }
}