using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Test.Events.Bus
{
    /// <summary>
    /// 事件数据对象派生对象
    /// </summary>
    public class MyDerivedEventData : MySimpleEventData
    {
        public MyDerivedEventData(int value)
            : base(value)
        {

        }
    }
}
