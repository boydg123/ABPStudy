namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// This interface is defined to standardize to set "Total Count of Items" to a DTO for long type.
    /// 此接口为DTO标准化定义long类型的“总项数”
    /// </summary>
    public interface IHasLongTotalCount
    {
        /// <summary>
        /// Total count of Items.
        /// 总项数
        /// </summary>
        long TotalCount { get; set; }
    }
}