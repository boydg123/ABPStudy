using System;

namespace Abp.Test.Dependency
{
    /// <summary>
    /// 简单释放对象3
    /// </summary>
    public class SimpleDisposableObject3 : IDisposable
    {
        public int MyData { get; set; }

        public int DisposeCount { get; set; }
        public SimpleDisposableObject3()
        {

        }
        public SimpleDisposableObject3(int myData)
        {
            MyData = myData;
        }
        public void Dispose()
        {
            DisposeCount++;
        }

        public int GetMyData()
        {
            return MyData;
        }
    }
}
