using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Abp
{
    /// <summary>
    /// 检查
    /// </summary>
    [DebuggerStepThrough]
    public static class Check
    {
        /// <summary>
        /// 检查对象是否为Null
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(T value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }
    }
}
