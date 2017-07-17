using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Derrick.Authorization.Users;

namespace Derrick.Sessions.Dto
{
    /// <summary>
    /// 用户登录信息Dto
    /// </summary>
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 姓
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// 照片ID
        /// </summary>
        public string ProfilePictureId { get; set; }
    }
}
