using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Abp.Application.Services
{
    /// <summary>
    /// 异步增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    public interface IAsyncCrudAppService<TEntityDto>
        : IAsyncCrudAppService<TEntityDto, int>
        where TEntityDto : IEntityDto<int>
    {

    }

    /// <summary>
    /// 异步增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey>
        : IAsyncCrudAppService<TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    /// <summary>
    /// 异步增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput>
        : IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    /// <summary>
    /// 异步增删该查应用服务
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建输入对象</typeparam>
    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput>
        : IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {

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
    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput>
        : IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {

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
    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput>
        : IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {

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
    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput, in TDeleteInput>
        : IApplicationService
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// 获取实体DTO对象
        /// </summary>
        /// <param name="input">获取输入对象</param>
        /// <returns></returns>
        Task<TEntityDto> Get(TGetInput input);

        /// <summary>
        /// 获取分页结果DTO
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input);

        /// <summary>
        /// 创建实体DTO对象
        /// </summary>
        /// <param name="input">创建输入对象</param>
        /// <returns></returns>
        Task<TEntityDto> Create(TCreateInput input);

        /// <summary>
        /// 更新实体DTO
        /// </summary>
        /// <param name="input">更新输入对象</param>
        /// <returns></returns>
        Task<TEntityDto> Update(TUpdateInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">删除输入对象</param>
        /// <returns></returns>
        Task Delete(TDeleteInput input);
    }
}
