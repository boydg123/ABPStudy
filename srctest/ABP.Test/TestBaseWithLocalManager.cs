using Abp.Dependency;
using System;

namespace Abp.Test
{
    /// <summary>
    /// 测试基类
    /// </summary>
    public abstract class TestBaseWithLocalManager : IDisposable
    {
        protected IIocManager localIocManager;
        protected TestBaseWithLocalManager()
        {
            localIocManager = new IocManager();
        }
        public void Dispose()
        {
            localIocManager.Dispose();
        }
    }
}
