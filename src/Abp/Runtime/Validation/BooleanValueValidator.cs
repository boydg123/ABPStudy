using System;

namespace Abp.Runtime.Validation
{
    /// <summary>
    /// Bool值验证器
    /// </summary>
    [Serializable]
    [Validator("BOOLEAN")]
    public class BooleanValueValidator : ValueValidatorBase
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is bool)
            {
                return true;
            }

            bool b;
            return bool.TryParse(value.ToString(), out b);
        }
    }
}