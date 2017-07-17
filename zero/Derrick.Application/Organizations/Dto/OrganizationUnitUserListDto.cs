using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Derrick.Authorization.Users;

namespace Derrick.Organizations.Dto
{
    /// <summary>
    /// 组织架构用户列表Dto
    /// </summary>
    [AutoMapFrom(typeof(User))]
    public class OrganizationUnitUserListDto : EntityDto<long>
    {
        /// <summary>
        /// 名称
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
        /// 邮件地址
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// 照片ID
        /// </summary>
        public Guid? ProfilePictureId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddedTime { get; set; }
    }
}