namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// This interface is defined to standardize to request a limited result.
    /// 此接口定义请求一个有限结果集
    /// </summary>
    public interface ILimitedResultRequest
    {
        /// <summary>
        /// Max expected result count.
        /// 结果集的最大项数
        /// </summary>
        int MaxResultCount { get; set; }
    }
}