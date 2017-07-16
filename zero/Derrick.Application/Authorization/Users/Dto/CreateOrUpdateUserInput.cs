using System.ComponentModel.DataAnnotations;

namespace Derrick.Authorization.Users.Dto
{
    /// <summary>
    /// 创建或更新用户Input
    /// </summary>
    public class CreateOrUpdateUserInput
    {
        /// <summary>
        /// 用户编辑Dto
        /// </summary>
        [Required]
        public UserEditDto User { get; set; }
        /// <summary>
        /// 分配角色名称
        /// </summary>
        [Required]
        public string[] AssignedRoleNames { get; set; }
        /// <summary>
        /// 是否发送激活邮箱
        /// </summary>
        public bool SendActivationEmail { get; set; }
        /// <summary>
        /// 设置随机密码
        /// </summary>
        public bool SetRandomPassword { get; set; }
    }
}