using System;
using Abp.Extensions;

namespace Abp.Runtime.Validation
{
    /// <summary>
    /// 数字值验证器
    /// </summary>
    [Serializable]
    [Validator("NUMERIC")]
    public class NumericValueValidator : ValueValidatorBase
    {
        /// <summary>
        /// 最小值
        /// </summary>
        public int MinValue
        {
            get { return (this["MinValue"] ?? "0").To<int>(); }
            set { this["MinValue"] = value; }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxValue
        {
            get { return (this["MaxValue"] ?? "0").To<int>(); }
            set { this["MaxValue"] = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public NumericValueValidator()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public NumericValueValidator(int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// 是否通过
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is int)
            {
                return IsValidInternal((int)value);
            }

            if (value is string)
            {
                int intValue;
                if (int.TryParse(value as string, out intValue))
                {
                    return IsValidInternal(intValue);
                }
            }

            return false;
        }

        protected virtual bool IsValidInternal(int value)
        {
            return value.IsBetween(MinValue, MaxValue);
        }
    }
}