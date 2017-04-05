namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// This interface is defined to standardize to return a page of items to clients.
    /// 此接口定义返回客户端结果集的一页
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="IListResult{T}.Items"/> list / <see cref="IListResult{T}.Items"/>中项的类型</typeparam>
    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {

    }
}