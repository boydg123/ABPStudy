using System;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;

namespace Derrick.Authorization.Users.Dto
{
    /// <summary>
    /// 用户尝试登录Dto
    /// </summary>
    [AutoMap(typeof(UserLoginAttempt))]
    public class UserLoginAttemptDto
    {
        /// <summary>
        /// 商户名
        /// </summary>
        public string TenancyName { get; set; }
        /// <summary>
        /// 用户名或邮箱
        /// </summary>
        public string UserNameOrEmail { get; set; }
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIpAddress { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string BrowserInfo { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
