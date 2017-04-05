using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;

namespace Abp.Application.Services
{
    /// <summary>
    /// 增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    public abstract class CrudAppService<TEntity, TEntityDto>
        : CrudAppService<TEntity, TEntityDto, int>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
    {
        protected CrudAppService(IRepository<TEntity, int> repository)
            : base(repository)
        {

        }
    }

    /// <summary>
    /// 增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey>
        : CrudAppService<TEntity, TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    /// <summary>
    /// 增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput>
        : CrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    /// <summary>
    /// 增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建对象</typeparam>
    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
        : CrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {
        protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    /// <summary>
    /// 增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建对象</typeparam>
    /// <typeparam name="TUpdateInput">更新对象</typeparam>
    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        : CrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    /// <summary>
    /// 增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建对象</typeparam>
    /// <typeparam name="TUpdateInput">更新对象</typeparam>
    /// <typeparam name="TGetInput">获取对象</typeparam>
    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
    : CrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {
        protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    /// <summary>
    /// 增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建对象</typeparam>
    /// <typeparam name="TUpdateInput">更新对象</typeparam>
    /// <typeparam name="TGetInput">获取对象</typeparam>
    /// <typeparam name="TDeleteInput">删除对象</typeparam>
    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
       : CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>,
        ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
           where TEntity : class, IEntity<TPrimaryKey>
           where TEntityDto : IEntityDto<TPrimaryKey>
           where TUpdateInput : IEntityDto<TPrimaryKey>
           where TGetInput : IEntityDto<TPrimaryKey>
           where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }

        /// <summary>
        /// 获取实体DTO对象
        /// </summary>
        /// <param name="input">获取输入对象</param>
        /// <returns></returns>
        public virtual TEntityDto Get(TGetInput input)
        {
            CheckGetPermission();

            var entity = GetEntityById(input.Id);
            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 获取分页结果DTO
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual PagedResultDto<TEntityDto> GetAll(TGetAllInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);

            var totalCount = query.Count();

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = query.ToList();

            return new PagedResultDto<TEntityDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
        }

        /// <summary>
        /// 创建实体DTO对象
        /// </summary>
        /// <param name="input">创建输入对象</param>
        /// <returns></returns>
        public virtual TEntityDto Create(TCreateInput input)
        {
            CheckCreatePermission();

            var entity = MapToEntity(input);

            Repository.Insert(entity);
            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 更新实体DTO
        /// </summary>
        /// <param name="input">更新输入对象</param>
        /// <returns></returns>
        public virtual TEntityDto Update(TUpdateInput input)
        {
            CheckUpdatePermission();

            var entity = GetEntityById(input.Id);

            MapToEntity(input, entity);
            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">删除输入对象</param>
        /// <returns></returns>
        public virtual void Delete(TDeleteInput input)
        {
            CheckDeletePermission();

            Repository.Delete(input.Id);
        }

        /// <summary>
        /// 通过ID获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual TEntity GetEntityById(TPrimaryKey id)
        {
            return Repository.Get(id);
        }
    }
}
