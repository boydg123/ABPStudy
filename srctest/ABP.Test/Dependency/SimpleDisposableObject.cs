using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Test.Dependency
{
    /// <summary>
    /// 简单释放对象
    /// </summary>
    public class SimpleDisposableObject : IDisposable
    {
        public int MyData { get; set; }

        public int DisposeCount { get; set; }

        public SimpleDisposableObject()
        {

        }

        public SimpleDisposableObject(int myData)
        {
            MyData = myData;
        }
        void IDisposable.Dispose()
        {
            DisposeCount++;
        }

        public int GetMyData()
        {
            return MyData;
        }
    }
}
