using System.Collections.Generic;

namespace Abp.Web.Models.AbpUserConfiguration
{
    /// <summary>
    /// ABP用户认证配置Dto
    /// </summary>
    public class AbpUserAuthConfigDto
    {
        public Dictionary<string,string> AllPermissions { get; set; }

        public Dictionary<string, string> GrantedPermissions { get; set; }
        
    }
}