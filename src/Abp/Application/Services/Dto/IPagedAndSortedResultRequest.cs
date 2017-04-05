namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// This interface is defined to standardize to request a paged and sorted result.
    /// 该接口定义为标准化以请求分页和排序结果
    /// </summary>
    public interface IPagedAndSortedResultRequest : IPagedResultRequest, ISortedResultRequest
    {
        
    }
}