using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Abp.Extensions;

namespace Abp.Runtime.Validation
{
    /// <summary>
    /// 字符串值验证器
    /// </summary>
    [Serializable]
    [Validator("STRING")]
    public class StringValueValidator : ValueValidatorBase
    {
        /// <summary>
        /// 允许为null
        /// </summary>
        public bool AllowNull
        {
            get { return (this["AllowNull"] ?? "false").To<bool>(); }
            set { this["AllowNull"] = value.ToString().ToLower(CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// 最小长度
        /// </summary>
        public int MinLength
        {
            get { return (this["MinLength"] ?? "0").To<int>(); }
            set { this["MinLength"] = value; }
        }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength
        {
            get { return (this["MaxLength"] ?? "0").To<int>(); }
            set { this["MaxLength"] = value; }
        }

        /// <summary>
        /// 正则表达式
        /// </summary>
        public string RegularExpression
        {
            get { return this["RegularExpression"] as string; }
            set { this["RegularExpression"] = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public StringValueValidator()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="minLength">最小长度</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="regularExpression">正则表达式</param>
        /// <param name="allowNull">允许为null</param>
        public StringValueValidator(int minLength = 0, int maxLength = 0, string regularExpression = null, bool allowNull = false)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            RegularExpression = regularExpression;
            AllowNull = allowNull;
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return AllowNull;
            }

            if (!(value is string))
            {
                return false;
            }

            var strValue = value as string;
            
            if (MinLength > 0 && strValue.Length < MinLength)
            {
                return false;
            }

            if (MaxLength > 0 && strValue.Length > MaxLength)
            {
                return false;
            }

            if (!RegularExpression.IsNullOrEmpty())
            {
                return Regex.IsMatch(strValue, RegularExpression);
            }

            return true;
        }
    }
}