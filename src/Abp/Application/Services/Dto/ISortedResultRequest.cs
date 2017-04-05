namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// This interface is defined to standardize to request a sorted result.
    /// 引接口定义请求一个有序的结果集
    /// </summary>
    public interface ISortedResultRequest
    {
        /// <summary>
        /// Sorting information.Should include sorting field and optionally a direction (ASC or DESC)Can contain more than one field separated by comma (,).
        /// 排序信息.应该指示排序的字段和方向（ASC 或者 DESC）能包含多个排序字段，使用逗号（,)分隔
        /// </summary>
        /// <example>
        /// 例如:
        /// "Name"
        /// "Name DESC"
        /// "Name ASC, Age DESC"
        /// </example>
        string Sorting { get; set; }
    }
}