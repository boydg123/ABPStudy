using Abp.Events.Bus;

namespace ABP.Test.Events.Bus
{
    /// <summary>
    /// 简单时间数据对象
    /// </summary>
    public class MySimpleEventData : EventData
    {
        public int Value { get; set; }
        public MySimpleEventData(int value)
        {
            Value = value;
        }
    }
}
