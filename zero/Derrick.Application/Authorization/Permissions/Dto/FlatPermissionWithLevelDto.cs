namespace Derrick.Authorization.Permissions.Dto
{
    /// <summary>
    /// 统一权限级别Dto
    /// </summary>
    public class FlatPermissionWithLevelDto : FlatPermissionDto
    {
        /// <summary> 
        /// 级别
        /// </summary>
        public int Level { get; set; }
    }
}