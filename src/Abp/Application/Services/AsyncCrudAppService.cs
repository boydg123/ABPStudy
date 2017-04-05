using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Linq;

namespace Abp.Application.Services
{
    /// <summary>
    /// 异步增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    public abstract class AsyncCrudAppService<TEntity, TEntityDto>
        : AsyncCrudAppService<TEntity, TEntityDto, int>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
    {
        protected AsyncCrudAppService(IRepository<TEntity, int> repository)
            : base(repository)
        {

        }
    }

    /// <summary>
    /// 异步增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    /// <summary>
    /// 异步增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    /// <summary>
    /// 异步增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建输入对象</typeparam>
    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TGetAllInput : IPagedAndSortedResultRequest
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
       where TCreateInput : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    /// <summary>
    /// 异步增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建输入对象</typeparam>
    /// <typeparam name="TUpdateInput">更新输入对象</typeparam>
    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    /// <summary>
    /// 异步增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建输入对象</typeparam>
    /// <typeparam name="TUpdateInput">更新输入对象</typeparam>
    /// <typeparam name="TGetInput">获取输入对象</typeparam>
    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
    : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {

        }
    }

    /// <summary>
    /// 异步增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建输入对象</typeparam>
    /// <typeparam name="TUpdateInput">更新输入对象</typeparam>
    /// <typeparam name="TGetInput">获取输入对象</typeparam>
    /// <typeparam name="TDeleteInput">删除输入对象</typeparam>
    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
       : CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>,
        IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
           where TEntity : class, IEntity<TPrimaryKey>
           where TEntityDto : IEntityDto<TPrimaryKey>
           where TUpdateInput : IEntityDto<TPrimaryKey>
           where TGetInput : IEntityDto<TPrimaryKey>
           where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// 异步查询执行者
        /// </summary>
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        protected AsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            :base(repository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        /// <summary>
        /// 获取实体DTO对象
        /// </summary>
        /// <param name="input">获取输入对象</param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> Get(TGetInput input)
        {
            CheckGetPermission();

            var entity = await GetEntityByIdAsync(input.Id);
            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 获取分页结果DTO
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

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
        public virtual async Task<TEntityDto> Create(TCreateInput input)
        {
            CheckCreatePermission();

            var entity = MapToEntity(input);

            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 更新实体DTO
        /// </summary>
        /// <param name="input">更新输入对象</param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> Update(TUpdateInput input)
        {
            CheckUpdatePermission();

            var entity = await GetEntityByIdAsync(input.Id);

            MapToEntity(input, entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">删除输入对象</param>
        /// <returns></returns>
        public virtual Task Delete(TDeleteInput input)
        {
            CheckDeletePermission();

            return Repository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 通过ID获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
        {
            return Repository.GetAsync(id);
        }
    }
}
