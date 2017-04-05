using System;

namespace Abp
{
    /// <summary>
    /// Used to generate Ids.
    /// 用于生成IDS
    /// </summary>
    public interface IGuidGenerator
    {
        /// <summary>
        /// Creates a GUID.
        /// 创建一个GUID
        /// </summary>
        Guid Create();
    }
}
