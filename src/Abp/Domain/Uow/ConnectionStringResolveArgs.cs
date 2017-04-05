using System.Collections.Generic;
using Abp.MultiTenancy;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// 连接字符串解析参数
    /// </summary>
    public class ConnectionStringResolveArgs : Dictionary<string, object>
    {
        /// <summary>
        /// 多租户双方
        /// </summary>
        public MultiTenancySides? MultiTenancySide { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="multiTenancySide"></param>
        public ConnectionStringResolveArgs(MultiTenancySides? multiTenancySide = null)
        {
            MultiTenancySide = multiTenancySide;
        }
    }
}