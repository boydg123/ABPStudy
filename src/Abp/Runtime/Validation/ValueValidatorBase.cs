using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Collections.Extensions;

namespace Abp.Runtime.Validation
{
    /// <summary>
    /// 值验证器基类
    /// </summary>
    [Serializable]
    public abstract class ValueValidatorBase : IValueValidator
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name
        {
            get
            {
                var type = GetType();
                if (type.IsDefined(typeof(ValidatorAttribute)))
                {
                    return type.GetCustomAttributes(typeof(ValidatorAttribute)).Cast<ValidatorAttribute>().First().Name;
                }

                return type.Name;
            }
        }

        /// <summary>
        /// Gets/sets arbitrary objects related to this object.Gets null if given key does not exists.
        /// 获取/设置与此对象相关的任意对象。如果给定的键不存在，将得到null。
        /// </summary>
        /// <param name="key">Key / Key</param>
        public object this[string key]
        {
            get { return Attributes.GetOrDefault(key); }
            set { Attributes[key] = value; }
        }

        /// <summary>
        /// Arbitrary objects related to this object.
        /// 此对象相关的任意对象
        /// </summary>
        public IDictionary<string, object> Attributes { get; private set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public abstract bool IsValid(object value);

        /// <summary>
        /// 构造函数
        /// </summary>
        protected ValueValidatorBase()
        {
            Attributes = new Dictionary<string, object>();
        }
    }
}