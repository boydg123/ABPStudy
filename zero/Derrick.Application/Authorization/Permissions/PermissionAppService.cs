using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Derrick.Authorization.Permissions.Dto;

namespace Derrick.Authorization.Permissions
{
    /// <summary>
    /// <see cref="IPermissionAppService"/>实现，权限服务
    /// </summary>
    public class PermissionAppService : AbpZeroTemplateAppServiceBase, IPermissionAppService
    {
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        public ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();
            var rootPermissions = permissions.Where(p => p.Parent == null);

            var result = new List<FlatPermissionWithLevelDto>();

            foreach (var rootPermission in rootPermissions)
            {
                var level = 0;
                AddPermission(rootPermission, permissions, result, level);
            }

            return new ListResultDto<FlatPermissionWithLevelDto>
            {
                Items = result
            };
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="permission">权限实体</param>
        /// <param name="allPermissions">所有权限</param>
        /// <param name="result">权限结果</param>
        /// <param name="level">级别</param>
        private void AddPermission(Permission permission, IReadOnlyList<Permission> allPermissions, List<FlatPermissionWithLevelDto> result, int level)
        {
            var flatPermission = permission.MapTo<FlatPermissionWithLevelDto>();
            flatPermission.Level = level;
            result.Add(flatPermission);

            if (permission.Children == null)
            {
                return;
            }

            var children = allPermissions.Where(p => p.Parent != null && p.Parent.Name == permission.Name).ToList();

            foreach (var childPermission in children)
            {
                AddPermission(childPermission, allPermissions, result, level + 1);
            }
        }
    }
}