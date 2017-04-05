using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Timing;
using Castle.Core.Logging;
using Newtonsoft.Json;

namespace Abp.Auditing
{
    /// <summary>
    /// 审计辅助类
    /// </summary>
    public class AuditingHelper : IAuditingHelper, ITransientDependency
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// ABP Session引用
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// 审计存储
        /// </summary>
        public IAuditingStore AuditingStore { get; set; }

        /// <summary>
        /// 审计信息提供者
        /// </summary>
        private readonly IAuditInfoProvider _auditInfoProvider;

        /// <summary>
        /// 审计配置
        /// </summary>
        private readonly IAuditingConfiguration _configuration;

        /// <summary>
        /// 工作单元管理器
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="auditInfoProvider">审计信息提供者</param>
        /// <param name="configuration">_configuration</param>
        /// <param name="unitOfWorkManager">工作单元管理器</param>
        public AuditingHelper(
            IAuditInfoProvider auditInfoProvider, 
            IAuditingConfiguration configuration, 
            IUnitOfWorkManager unitOfWorkManager)
        {
            _auditInfoProvider = auditInfoProvider;
            _configuration = configuration;
            _unitOfWorkManager = unitOfWorkManager;

            AbpSession = NullAbpSession.Instance;
            Logger = NullLogger.Instance;
            AuditingStore = SimpleLogAuditingStore.Instance;
        }

        /// <summary>
        /// 是否需要保存审计
        /// </summary>
        /// <param name="methodInfo">方法信息</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false)
        {
            if (!_configuration.IsEnabled)
            {
                return false;
            }

            if (!_configuration.IsEnabledForAnonymousUsers && (AbpSession?.UserId == null))
            {
                return false;
            }

            if (methodInfo == null)
            {
                return false;
            }

            if (!methodInfo.IsPublic)
            {
                return false;
            }

            if (methodInfo.IsDefined(typeof(AuditedAttribute), true))
            {
                return true;
            }

            if (methodInfo.IsDefined(typeof(DisableAuditingAttribute), true))
            {
                return false;
            }

            var classType = methodInfo.DeclaringType;
            if (classType != null)
            {
                if (classType.IsDefined(typeof(AuditedAttribute), true))
                {
                    return true;
                }

                if (classType.IsDefined(typeof(DisableAuditingAttribute), true))
                {
                    return false;
                }

                if (_configuration.Selectors.Any(selector => selector.Predicate(classType)))
                {
                    return true;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 创建审计信息
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="arguments">参数</param>
        /// <returns></returns>
        public AuditInfo CreateAuditInfo(MethodInfo method, object[] arguments)
        {
            return CreateAuditInfo(method, CreateArgumentsDictionary(method, arguments));
        }

        /// <summary>
        /// 创建审计信息
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="arguments">参数</param>
        /// <returns></returns>
        public AuditInfo CreateAuditInfo(MethodInfo method, IDictionary<string, object> arguments)
        {
            var auditInfo = new AuditInfo
            {
                TenantId = AbpSession.TenantId,
                UserId = AbpSession.UserId,
                ImpersonatorUserId = AbpSession.ImpersonatorUserId,
                ImpersonatorTenantId = AbpSession.ImpersonatorTenantId,
                ServiceName = method.DeclaringType != null
                    ? method.DeclaringType.FullName
                    : "",
                MethodName = method.Name,
                Parameters = ConvertArgumentsToJson(arguments),
                ExecutionTime = Clock.Now
            };

            _auditInfoProvider.Fill(auditInfo);

            return auditInfo;
        }

        /// <summary>
        /// 保存审计信息
        /// </summary>
        /// <param name="auditInfo">审计信息</param>
        public void Save(AuditInfo auditInfo)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                AuditingStore.Save(auditInfo);
                uow.Complete();
            }
        }

        /// <summary>
        /// 异步保存审计信息
        /// </summary>
        /// <param name="auditInfo">审计信息</param>
        /// <returns></returns>
        public async Task SaveAsync(AuditInfo auditInfo)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                await AuditingStore.SaveAsync(auditInfo);
                await uow.CompleteAsync();
            }
        }

        /// <summary>
        /// 将参数转换成Json
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private string ConvertArgumentsToJson(IDictionary<string, object> arguments)
        {
            try
            {
                if (arguments.IsNullOrEmpty())
                {
                    return "{}";
                }

                var dictionary = new Dictionary<string, object>();

                foreach (var argument in arguments)
                {
                    if (argument.Value != null && _configuration.IgnoredTypes.Any(t => t.IsInstanceOfType(argument.Value)))
                    {
                        dictionary[argument.Key] = null;
                    }
                    else
                    {
                        dictionary[argument.Key] = argument.Value;
                    }
                }

                return Serialize(dictionary);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString(), ex);
                return "{}";
            }
        }

        /// <summary>
        /// 创建参数字典
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="arguments">参数</param>
        /// <returns></returns>
        private static Dictionary<string, object> CreateArgumentsDictionary(MethodInfo method, object[] arguments)
        {
            var parameters = method.GetParameters();
            var dictionary = new Dictionary<string, object>();

            for (var i = 0; i < parameters.Length; i++)
            {
                dictionary[parameters[i].Name] = arguments[i];
            }

            return dictionary;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            var options = new JsonSerializerSettings
            {
                ContractResolver = new AuditingContractResolver()
            };

            return JsonConvert.SerializeObject(obj, options);
        }
    }
}