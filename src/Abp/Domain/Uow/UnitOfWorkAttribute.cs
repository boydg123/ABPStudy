using System;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// This attribute is used to indicate that declaring method is atomic and should be considered as a unit of work.
    /// 这个特性是用来表明，声明的方法是原子,应该视为一个工作单元
    /// A method that has this attribute is intercepted, a database connection is opened and a transaction is started before call the method.
    /// 拥有此特性的方法将被拦截，在调用方法之前一个数据库连接和一个事务将被开启
    /// At the end of method call, transaction is committed and all changes applied to the database if there is no exception,otherwise it's rolled back. 
    /// 如果没有异常，在方法调用结事时，事务被提交，所有的更变提交到数据库中，否则，回滚
    /// </summary>
    /// <remarks>
    /// This attribute has no effect if there is already a unit of work before calling this method, if so, it uses the same transaction.
    /// 如果在调用方法之前，已经存在工作单元，这个特性不会产生任何影响。它将使用相同的事务
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : Attribute
    {
        /// <summary>
        /// Scope option.
        /// 事物范围
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }

        /// <summary>
        /// Is this UOW transactional?Uses default value if not supplied.
        /// 此工作单元是否支持事件，默认不支持
        /// </summary>
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// Timeout of UOW As milliseconds.Uses default value if not supplied.
        /// 工作单元的超时时间（毫秒）默认不支持
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// If this UOW is transactional, this option indicated the isolation level of the transaction.Uses default value if not supplied.
        /// 如果工作单元支持事务，此属性指示事务的隔离级别.默认不支持
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// Used to prevent starting a unit of work for the method.
        /// 用来阻止为一个方法开启工作单元，如果已经存在一个工作单元，
        /// If there is already a started unit of work, this property is ignored.Default: false.
        /// 此属性将被忽略，默认值为:false
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Creates a new UnitOfWorkAttribute object.
        /// 创建一个一个工作单元特性
        /// </summary>
        public UnitOfWorkAttribute()
        {

        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.
        /// 创建一个新的<see cref="UnitOfWorkAttribute"/> 对象.
        /// </summary>
        /// <param name="isTransactional">
        /// Is this unit of work will be transactional? / 指示工作单元是否支持事务
        /// </param>
        public UnitOfWorkAttribute(bool isTransactional)
        {
            IsTransactional = isTransactional;
        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.
        /// 创建一个新的<see cref="UnitOfWorkAttribute"/> 对象.
        /// </summary>
        /// <param name="timeout">As milliseconds / 超时时间（毫秒）</param>
        public UnitOfWorkAttribute(int timeout)
        {
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.
        /// 创建一个新的<see cref="UnitOfWorkAttribute"/> 对象.
        /// </summary>
        /// <param name="isTransactional">Is this unit of work will be transactional? / 指示工作单元是否支持事各</param>
        /// <param name="timeout">As milliseconds / 超时时间（毫秒）</param>
        public UnitOfWorkAttribute(bool isTransactional, int timeout)
        {
            IsTransactional = isTransactional;
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.<see cref="IsTransactional"/> is automatically set to true.
        /// 创建一个新的<see cref="UnitOfWorkAttribute"/> 对象.<see cref="IsTransactional"/> 设置为true.
        /// </summary>
        /// <param name="isolationLevel">Transaction isolation level / 事务隔离级别</param>
        public UnitOfWorkAttribute(IsolationLevel isolationLevel)
        {
            IsTransactional = true;
            IsolationLevel = isolationLevel;
        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.<see cref="IsTransactional"/> is automatically set to true.
        /// 创建一个新的 <see cref="UnitOfWorkAttribute"/> 对象.<see cref="IsTransactional"/> 设置为true.
        /// </summary>
        /// <param name="isolationLevel">Transaction isolation level / 事务隔离级别</param>
        /// <param name="timeout">Transaction  timeout as milliseconds / 事务超时时间（毫秒）</param>
        public UnitOfWorkAttribute(IsolationLevel isolationLevel, int timeout)
        {
            IsTransactional = true;
            IsolationLevel = isolationLevel;
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.<see cref="IsTransactional"/> is automatically set to true.
        /// 创建一个新的 <see cref="UnitOfWorkAttribute"/> 对象.<see cref="IsTransactional"/> 设置为true.
        /// </summary>
        /// <param name="scope">Transaction scope / 事物范围</param>
        public UnitOfWorkAttribute(TransactionScopeOption scope)
        {
            IsTransactional = true;
            Scope = scope;
        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object. <see cref="IsTransactional"/> is automatically set to true.
        /// 创建一个新的 <see cref="UnitOfWorkAttribute"/> 对象.<see cref="IsTransactional"/> 设置为true.
        /// </summary>
        /// <param name="scope">Transaction scope / 事物范围</param>
        /// <param name="timeout">Transaction timeout as milliseconds / 事务超时时间（毫秒）</param>
        public UnitOfWorkAttribute(TransactionScopeOption scope, int timeout)
        {
            IsTransactional = true;
            Scope = scope;
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Gets UnitOfWorkAttribute for given method or null if no attribute defined.
        /// 从给定的方法中获取UnitOfWorkAttribute特性，如果没有返回Null
        /// </summary>
        /// <param name="methodInfo">Method to get attribute / 获取属性的方法</param>
        /// <returns>The UnitOfWorkAttribute object / <see cref="UnitOfWorkAttribute"/>对象</returns>
        public static UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MemberInfo methodInfo)
        {
            var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }

            if (UnitOfWorkHelper.IsConventionalUowClass(methodInfo.DeclaringType))
            {
                return new UnitOfWorkAttribute(); //Default
            }

            return null;
        }

        /// <summary>
        /// 创建工作单元选项
        /// </summary>
        /// <returns></returns>
        public UnitOfWorkOptions CreateOptions()
        {
            return new UnitOfWorkOptions
            {
                IsTransactional = IsTransactional,
                IsolationLevel = IsolationLevel,
                Timeout = Timeout,
                Scope = Scope
            };
        }
    }
}