using System;

namespace Abp.AutoMapper
{
    /// <summary>
    /// AutoMap 方向(来/去)
    /// </summary>
    [Flags]
    public enum AutoMapDirection
    {
        From,
        To
    }
}