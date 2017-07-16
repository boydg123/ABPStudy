using System;
using Abp.Application.Services.Dto;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// 链接用户Dto
    /// </summary>
    public class LinkedUserDto : EntityDto<long>
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string TenancyName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 获取显示的登录名
        /// </summary>
        /// <param name="multiTenancyEnabled">是否开启多商户</param>
        /// <returns></returns>
        public object GetShownLoginName(bool multiTenancyEnabled)
        {
            if (!multiTenancyEnabled)
            {
                return Username;
            }

            return string.IsNullOrEmpty(TenancyName)
                ? ".\\" + Username
                : TenancyName + "\\" + Username;
        }
    }
}