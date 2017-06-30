namespace Abp.Authorization
{
    /// <summary>
    /// Represents a permission <see cref="Name"/> with <see cref="IsGranted"/> information.
    /// 代表一个权限<see cref="Name"/>是否<see cref="IsGranted"/>的相关信息
    /// </summary>
    public class PermissionGrantInfo
    {
        /// <summary>
        /// Name of the permission.
        /// 权限名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Is this permission granted Prohibited?
        /// 此权限是否准许
        /// </summary>
        public bool IsGranted { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="PermissionGrantInfo"/>.
        /// 构造函数
        /// </summary>
        /// <param name="name">权限名称</param>
        /// <param name="isGranted">此权限是否准许</param>
        public PermissionGrantInfo(string name, bool isGranted)
        {
            Name = name;
            IsGranted = isGranted;
        }
    }
}