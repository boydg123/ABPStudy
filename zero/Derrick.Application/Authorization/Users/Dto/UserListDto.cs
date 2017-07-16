using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Derrick.Authorization.Users.Dto
{
    /// <summary>
    /// 用户列表Dto
    /// </summary>
    [AutoMapFrom(typeof(User))]
    public class UserListDto : EntityDto<long>, IPassivable, IHasCreationTime
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
        /// 邮箱
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 资料照片ID
        /// </summary>
        public Guid? ProfilePictureId { get; set; }
        /// <summary>
        /// 是否确认邮箱
        /// </summary>
        public bool IsEmailConfirmed { get; set; }
        /// <summary>
        /// 角色列表
        /// </summary>
        public List<UserListRoleDto> Roles { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 用户角色列表Dto
        /// </summary>
        [AutoMapFrom(typeof(UserRole))]
        public class UserListRoleDto
        {
            /// <summary>
            /// 角色ID
            /// </summary>
            public int RoleId { get; set; }
            /// <summary>
            /// 角色名
            /// </summary>
            public string RoleName { get; set; }
        }
    }
}