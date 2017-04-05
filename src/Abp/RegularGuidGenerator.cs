using System;
using Abp.Dependency;

namespace Abp
{
    /// <summary>
    /// Implements <see cref="IGuidGenerator"/> by using <see cref="Guid.NewGuid"/>.
    /// 使用<see cref="Guid.NewGuid"/>实现<see cref="IGuidGenerator"/>
    /// </summary>
    public class RegularGuidGenerator : IGuidGenerator, ITransientDependency
    {
        public virtual Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}