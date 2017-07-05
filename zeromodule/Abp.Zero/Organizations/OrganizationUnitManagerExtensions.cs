using System.Collections.Generic;
using Abp.Threading;

namespace Abp.Organizations
{
    /// <summary>
    /// 组织架构管理扩展
    /// </summary>
    public static class OrganizationUnitManagerExtensions
    {
        /// <summary>
        /// 获取Code
        /// </summary>
        /// <param name="manager">组织管理</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetCode(this OrganizationUnitManager manager, long id)
        {
            return AsyncHelper.RunSync(() => manager.GetCodeAsync(id));
        }
        /// <summary>
        /// 创建组织
        /// </summary>
        /// <param name="manager">组织管理</param>
        /// <param name="organizationUnit">组织</param>
        public static void Create(this OrganizationUnitManager manager, OrganizationUnit organizationUnit)
        {
            AsyncHelper.RunSync(() => manager.CreateAsync(organizationUnit));
        }
        /// <summary>
        /// 更新组织
        /// </summary>
        /// <param name="manager">组织管理</param>
        /// <param name="organizationUnit">组织</param>
        public static void Update(this OrganizationUnitManager manager, OrganizationUnit organizationUnit)
        {
            AsyncHelper.RunSync(() => manager.UpdateAsync(organizationUnit));
        }
        /// <summary>
        /// 删除组织
        /// </summary>
        /// <param name="manager">组织管理</param>
        /// <param name="id">组织ID</param>
        public static void Delete(this OrganizationUnitManager manager, long id)
        {
            AsyncHelper.RunSync(() => manager.DeleteAsync(id));
        }
        /// <summary>
        /// 获取下一个子组织Code
        /// </summary>
        /// <param name="manager">组织管理</param>
        /// <param name="parentId">父组织ID</param>
        /// <returns></returns>
        public static string GetNextChildCode(this OrganizationUnitManager manager, long? parentId)
        {
            return AsyncHelper.RunSync(() => manager.GetNextChildCodeAsync(parentId));
        }
        /// <summary>
        /// 获取最后子组织或Null(如果没有找到则返回Null)
        /// </summary>
        /// <param name="manager">组织管理</param>
        /// <param name="parentId">父组织ID</param>
        /// <returns></returns>
        public static OrganizationUnit GetLastChildOrNull(this OrganizationUnitManager manager, long? parentId)
        {
            return AsyncHelper.RunSync(() => manager.GetLastChildOrNullAsync(parentId));
        }
        /// <summary>
        /// 移动组织
        /// </summary>
        /// <param name="manager">组织管理</param>
        /// <param name="id">组织ID</param>
        /// <param name="parentId">父组织ID</param>
        public static void Move(this OrganizationUnitManager manager, long id, long? parentId)
        {
            AsyncHelper.RunSync(() => manager.MoveAsync(id, parentId));
        }
        /// <summary>
        /// 查找子组织列表
        /// </summary>
        /// <param name="manager">组织管理</param>
        /// <param name="parentId">父组织ID</param>
        /// <param name="recursive">是否递归</param>
        /// <returns></returns>
        public static List<OrganizationUnit> FindChildren(this OrganizationUnitManager manager, long? parentId, bool recursive = false)
        {
            return AsyncHelper.RunSync(() => manager.FindChildrenAsync(parentId, recursive));
        }
    }
}