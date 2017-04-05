using System.Collections.Generic;

namespace Abp.Application.Services
{
    /// <summary>
    /// 避免重复横切关注点接口
    /// </summary>
    public interface IAvoidDuplicateCrossCuttingConcerns
    {
        /// <summary>
        /// 避免重复横切关注点
        /// </summary>
        List<string> AppliedCrossCuttingConcerns { get; }
    }
}