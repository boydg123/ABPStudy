using Abp.Application.Services.Dto;

namespace Abp.Application.Services
{
    /// <summary>
    /// 增删该查应用服务接口
    /// </summary>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    public interface ICrudAppService<TEntityDto>
        : ICrudAppService<TEntityDto, int>
        where TEntityDto : IEntityDto<int>
    {

    }

    /// <summary>
    /// 增删该查应用服务接口
    /// </summary>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    public interface ICrudAppService<TEntityDto, TPrimaryKey>
        : ICrudAppService<TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    /// <summary>
    /// 增删该查应用服务接口
    /// </summary>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput>
        : ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    /// <summary>
    /// 增删该查应用服务接口
    /// </summary>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建输入对象</typeparam>
    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput>
        : ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {

    }

    /// <summary>
    /// 增删该查应用服务接口
    /// </summary>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建输入对象</typeparam>
    /// <typeparam name="TUpdateInput">更新输入对象</typeparam>
    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput>
        : ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {

    }

    /// <summary>
    /// 增删该查应用服务接口
    /// </summary>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建输入对象</typeparam>
    /// <typeparam name="TUpdateInput">更新输入对象</typeparam>
    /// <typeparam name="TGetInput">获取输入对象</typeparam>
    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput>
    : ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey> 
        where TGetInput : IEntityDto<TPrimaryKey>
    {

    }

    /// <summary>
    /// 增删该查应用服务接口
    /// </summary>
    /// <typeparam name="TEntityDto">实体DTO对象</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    /// <typeparam name="TGetAllInput">获取所有输入对象</typeparam>
    /// <typeparam name="TCreateInput">创建输入对象</typeparam>
    /// <typeparam name="TUpdateInput">更新输入对象</typeparam>
    /// <typeparam name="TGetInput">获取输入对象</typeparam>
    /// <typeparam name="TDeleteInput">删除输入对象</typeparam>
    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput, in TDeleteInput>
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
        TEntityDto Get(TGetInput input);

        /// <summary>
        /// 获取所有输入对象
        /// </summary>
        /// <param name="input">输入对象</param>
        /// <returns></returns>
        PagedResultDto<TEntityDto> GetAll(TGetAllInput input);

        /// <summary>
        /// 创建实体DTO对象
        /// </summary>
        /// <param name="input">创建输入对象</param>
        /// <returns></returns>
        TEntityDto Create(TCreateInput input);

        /// <summary>
        /// 更新实体DTO
        /// </summary>
        /// <param name="input">更新输入对象</param>
        /// <returns></returns>
        TEntityDto Update(TUpdateInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">删除输入对象</param>
        /// <returns></returns>
        void Delete(TDeleteInput input);
    }
}
