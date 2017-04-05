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
    /// ��Ƹ�����
    /// </summary>
    public class AuditingHelper : IAuditingHelper, ITransientDependency
    {
        /// <summary>
        /// ��־��¼��
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// ABP Session����
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// ��ƴ洢
        /// </summary>
        public IAuditingStore AuditingStore { get; set; }

        /// <summary>
        /// �����Ϣ�ṩ��
        /// </summary>
        private readonly IAuditInfoProvider _auditInfoProvider;

        /// <summary>
        /// �������
        /// </summary>
        private readonly IAuditingConfiguration _configuration;

        /// <summary>
        /// ������Ԫ������
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="auditInfoProvider">�����Ϣ�ṩ��</param>
        /// <param name="configuration">_configuration</param>
        /// <param name="unitOfWorkManager">������Ԫ������</param>
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
        /// �Ƿ���Ҫ�������
        /// </summary>
        /// <param name="methodInfo">������Ϣ</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
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
        /// ���������Ϣ
        /// </summary>
        /// <param name="method">����</param>
        /// <param name="arguments">����</param>
        /// <returns></returns>
        public AuditInfo CreateAuditInfo(MethodInfo method, object[] arguments)
        {
            return CreateAuditInfo(method, CreateArgumentsDictionary(method, arguments));
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="method">����</param>
        /// <param name="arguments">����</param>
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
        /// ���������Ϣ
        /// </summary>
        /// <param name="auditInfo">�����Ϣ</param>
        public void Save(AuditInfo auditInfo)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                AuditingStore.Save(auditInfo);
                uow.Complete();
            }
        }

        /// <summary>
        /// �첽���������Ϣ
        /// </summary>
        /// <param name="auditInfo">�����Ϣ</param>
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
        /// ������ת����Json
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
        /// ���������ֵ�
        /// </summary>
        /// <param name="method">����</param>
        /// <param name="arguments">����</param>
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
        /// ���л�
        /// </summary>
        /// <param name="obj">���л�����</param>
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