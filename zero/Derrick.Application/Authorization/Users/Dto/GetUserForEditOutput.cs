using System;

namespace Derrick.Authorization.Users.Dto
{
    /// <summary>
    /// 编辑用户Output
    /// </summary>
    public class GetUserForEditOutput
    {
        /// <summary>
        /// 资料照片ID
        /// </summary>
        public Guid? ProfilePictureId { get; set; }
        /// <summary>
        /// 编辑用户Dto
        /// </summary>
        public UserEditDto User { get; set; }
        /// <summary>
        /// 用户角色Dto
        /// </summary>
        public UserRoleDto[] Roles { get; set; }
    }
}