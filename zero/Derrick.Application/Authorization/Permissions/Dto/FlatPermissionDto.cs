using Abp.AutoMapper;

namespace Derrick.Authorization.Permissions.Dto
{
    /// <summary>
    /// 统一权限Dto
    /// </summary>
    [AutoMapFrom(typeof(Abp.Authorization.Permission))] 
    public class FlatPermissionDto
    {
        /// <summary>
        /// 父Name
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否默认授予
        /// </summary>
        public bool IsGrantedByDefault { get; set; }
    }
}