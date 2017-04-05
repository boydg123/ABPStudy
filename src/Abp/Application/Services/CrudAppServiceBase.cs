using System.Linq;
using System.Linq.Dynamic;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.ObjectMapping;

namespace Abp.Application.Services
{
    /// <summary>
    /// This is a common base class for CrudAppService and AsyncCrudAppService classes.Inherit either from CrudAppService or AsyncCrudAppService, not from this class.
    /// 这是<see cref="CrudAppService"/> 和 <see cref="AsyncCrudAppService"/>的基类，需要继承自CrudAppService or AsyncCrudAppService，无法继承此类
    /// </summary>
    public abstract class CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> : ApplicationService
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// 实体仓储
        /// </summary>
        protected readonly IRepository<TEntity, TPrimaryKey> Repository;

        /// <summary>
        /// 获取权限名称
        /// </summary>
        protected virtual string GetPermissionName { get; set; }

        /// <summary>
        /// 获取所有的权限名称
        /// </summary>
        protected virtual string GetAllPermissionName { get; set; }

        /// <summary>
        /// 创建权限名称
        /// </summary>
        protected virtual string CreatePermissionName { get; set; }

        /// <summary>
        /// 更新权限名称
        /// </summary>
        protected virtual string UpdatePermissionName { get; set; }

        /// <summary>
        /// 删除权限名称
        /// </summary>
        protected virtual string DeletePermissionName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository">实体仓储</param>
        protected CrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository)
        {
            Repository = repository;
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// 如果需要，应该用于排序
        /// </summary>
        /// <param name="query">The query. / 查询</param>
        /// <param name="input">The input. / 输入</param>
        protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetAllInput input)
        {
            //Try to sort query if available 尝试排序查询是否可用
            var sortInput = input as ISortedResultRequest;
            if (sortInput != null)
            {
                if (!sortInput.Sorting.IsNullOrWhiteSpace())
                {
                    return query.OrderBy(sortInput.Sorting);
                }
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// 如果需要用于分页
        /// </summary>
        /// <param name="query">The query. / 查询</param>
        /// <param name="input">The input. / 输入</param>
        protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetAllInput input)
        {
            //Try to use paging if available
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        /// <summary>
        /// This method should create <see cref="IQueryable{TEntity}"/> based on given input.It should filter query if needed, but should not do sorting or paging.
        /// 此方法应该基于提供的输入对象创建<see cref="IQueryable{TEntity}"/>，如果需要它应该过滤查询，但是不能排序以及分页。
        /// Sorting should be done in <see cref="ApplySorting"/> and paging should be done in <see cref="ApplyPaging"/> methods.
        /// 排序应该完成在 <see cref="ApplySorting"/>方法，分页应该完成在<see cref="ApplyPaging"/>方法。
        /// </summary>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> CreateFilteredQuery(TGetAllInput input)
        {
            return Repository.GetAll();
        }

        /// <summary>
        /// Maps <see cref="TEntity"/> to <see cref="TEntityDto"/>.It uses <see cref="IObjectMapper"/> by default.It can be overrided for custom mapping.
        /// 映射<see cref="TEntity"/>到<see cref="TEntityDto"/>,他默认使用<see cref="IObjectMapper"/>，它可以被重写于自定义映射
        /// </summary>
        protected virtual TEntityDto MapToEntityDto(TEntity entity)
        {
            return ObjectMapper.Map<TEntityDto>(entity);
        }

        /// <summary>
        /// Maps <see cref="TEntityDto"/> to <see cref="TEntity"/> to create a new entity.It uses <see cref="IObjectMapper"/> by default.It can be overrided for custom mapping.
        /// 映射<see cref="TEntityDto"/>到<see cref="TEntity"/>来创建一个新的实体，他默认使用<see cref="IObjectMapper"/>，它可以被重写于自定义映射
        /// </summary>
        protected virtual TEntity MapToEntity(TCreateInput createInput)
        {
            return ObjectMapper.Map<TEntity>(createInput);
        }

        /// <summary>
        /// Maps <see cref="TUpdateInput"/> to <see cref="TEntity"/> to update the entity.It uses <see cref="IObjectMapper"/> by default.It can be overrided for custom mapping.
        /// 映射<see cref="TEntityDto"/>到<see cref="TEntity"/>来更新实体，他默认使用<see cref="IObjectMapper"/>，它可以被重写于自定义映射
        /// </summary>
        protected virtual void MapToEntity(TUpdateInput updateInput, TEntity entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="permissionName">权限名称</param>
        protected virtual void CheckPermission(string permissionName)
        {
            if (!string.IsNullOrEmpty(permissionName))
            {
                PermissionChecker.Authorize(permissionName);
            }
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        protected virtual void CheckGetPermission()
        {
            CheckPermission(GetPermissionName);
        }

        /// <summary>
        /// 检查所有权限
        /// </summary>
        protected virtual void CheckGetAllPermission()
        {
            CheckPermission(GetAllPermissionName);
        }

        /// <summary>
        /// 检查创建权限
        /// </summary>
        protected virtual void CheckCreatePermission()
        {
            CheckPermission(CreatePermissionName);
        }

        /// <summary>
        /// 检查更新权限
        /// </summary>
        protected virtual void CheckUpdatePermission()
        {
            CheckPermission(UpdatePermissionName);
        }

        /// <summary>
        /// 检查删除权限
        /// </summary>
        protected virtual void CheckDeletePermission()
        {
            CheckPermission(DeletePermissionName);
        }
    }
}
