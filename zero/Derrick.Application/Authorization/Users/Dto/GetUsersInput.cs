using Abp.Runtime.Validation;
using Derrick.Dto;

namespace Derrick.Authorization.Users.Dto
{
    /// <summary>
    /// 获取用户Input
    /// </summary>
    public class GetUsersInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 过滤信息
        /// </summary>
        public string Filter { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public string Permission { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public int? Role { get; set; }
        /// <summary>
        /// 规范
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name,Surname";
            }
        }
    }
}