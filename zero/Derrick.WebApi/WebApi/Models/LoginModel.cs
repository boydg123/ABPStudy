using System.ComponentModel.DataAnnotations;

namespace Derrick.WebApi.Models
{
    /// <summary>
    /// 登录模型
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        public string TenancyName { get; set; }

        /// <summary>
        /// 用户名或邮箱
        /// </summary>
        [Required]
        public string UsernameOrEmailAddress { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
