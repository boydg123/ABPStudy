using System.ComponentModel.DataAnnotations;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// Simply implements <see cref="ILimitedResultRequest"/>.
    /// <see cref="ILimitedResultRequest"/>的实现
    /// </summary>
    public class LimitedResultRequestDto : ILimitedResultRequest
    {
        /// <summary>
        /// 结果集的最大项数
        /// </summary>
        [Range(1, int.MaxValue)]
        public virtual int MaxResultCount { get; set; } = 10;
    }
}