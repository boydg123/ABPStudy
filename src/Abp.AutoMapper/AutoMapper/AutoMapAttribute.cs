using System;

namespace Abp.AutoMapper
{
    /// <summary>
    /// AutoMap 特性
    /// </summary>
    public class AutoMapAttribute : Attribute
    {
        public Type[] TargetTypes { get; private set; }

        public virtual AutoMapDirection Direction
        {
            get { return AutoMapDirection.From | AutoMapDirection.To; }
        }

        public AutoMapAttribute(params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }
    }
}