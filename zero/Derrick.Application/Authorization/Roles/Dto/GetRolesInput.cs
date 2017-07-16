using Abp.Application.Services.Dto;

namespace Derrick.Authorization.Roles.Dto
{
    /// <summary>
    /// 获取角色Input
    /// </summary>
    public class GetRolesInput 
    {
        /// <summary>
        /// 权限
        /// </summary>
        public string Permission { get; set; }
    }
}
