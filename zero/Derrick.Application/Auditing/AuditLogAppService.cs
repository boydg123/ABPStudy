using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Derrick.Auditing.Dto;
using Derrick.Auditing.Exporting;
using Derrick.Authorization;
using Derrick.Authorization.Users;
using Derrick.Dto;

namespace Derrick.Auditing
{
    /// <summary>
    /// <see cref="IAuditLogAppService"/>实现，审计日志APP服务
    /// </summary>
    [DisableAuditing]
    [AbpAuthorize(AppPermissions.Pages_Administration_AuditLogs)]
    public class AuditLogAppService : AbpZeroTemplateAppServiceBase, IAuditLogAppService
    {
        /// <summary>
        /// 审计日志仓储
        /// </summary>
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly IRepository<User, long> _userRepository;
        /// <summary>
        /// 审计日志列表Excel导出器
        /// </summary>
        private readonly IAuditLogListExcelExporter _auditLogListExcelExporter;
        /// <summary>
        /// 命名空间剥离器
        /// </summary>
        private readonly INamespaceStripper _namespaceStripper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="auditLogRepository">审计日志仓储</param>
        /// <param name="userRepository">用户仓储</param>
        /// <param name="auditLogListExcelExporter">审计日志列表Excel导出器</param>
        /// <param name="namespaceStripper">命名空间剥离器</param>
        public AuditLogAppService(
            IRepository<AuditLog, long> auditLogRepository, 
            IRepository<User, long> userRepository, 
            IAuditLogListExcelExporter auditLogListExcelExporter, 
            INamespaceStripper namespaceStripper)
        {
            _auditLogRepository = auditLogRepository;
            _userRepository = userRepository;
            _auditLogListExcelExporter = auditLogListExcelExporter;
            _namespaceStripper = namespaceStripper;
        }

        /// <summary>
        /// 获取审计日志列表(带分页)
        /// </summary>
        /// <param name="input">审计日志输入对象</param>
        /// <returns></returns>
        public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        {
            var query = CreateAuditLogAndUsersQuery(input);

            var resultCount = await query.CountAsync();
            var results = await query
                .AsNoTracking()
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var auditLogListDtos = ConvertToAuditLogListDtos(results);

            return new PagedResultDto<AuditLogListDto>(resultCount, auditLogListDtos);
        }

        /// <summary>
        /// 获取导出到Excel审计日志
        /// </summary>
        /// <param name="input">审计日志输入对象</param>
        /// <returns></returns>
        public async Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input)
        {
            var auditLogs = await CreateAuditLogAndUsersQuery(input)
                        .AsNoTracking()
                        .OrderByDescending(al => al.AuditLog.ExecutionTime)
                        .ToListAsync();

            var auditLogListDtos = ConvertToAuditLogListDtos(auditLogs);

            return _auditLogListExcelExporter.ExportToFile(auditLogListDtos);
        }

        /// <summary>
        /// 将用户审计日志对象列表导出到设计日志Dto列表
        /// </summary>
        /// <param name="results">用户审计日志对象</param>
        /// <returns></returns>
        private List<AuditLogListDto> ConvertToAuditLogListDtos(List<AuditLogAndUser> results)
        {
            return results.Select(
                result =>
                {
                    var auditLogListDto = result.AuditLog.MapTo<AuditLogListDto>();
                    auditLogListDto.UserName = result.User == null ? null : result.User.UserName;
                    auditLogListDto.ServiceName = _namespaceStripper.StripNameSpace(auditLogListDto.ServiceName);
                    return auditLogListDto;
                }).ToList();
        }

        /// <summary>
        /// 创建审计日志和用户查询
        /// </summary>
        /// <param name="input">审计日志输入对象</param>
        /// <returns></returns>
        private IQueryable<AuditLogAndUser> CreateAuditLogAndUsersQuery(GetAuditLogsInput input)
        {
            var query = from auditLog in _auditLogRepository.GetAll()
                join user in _userRepository.GetAll() on auditLog.UserId equals user.Id into userJoin
                from joinedUser in userJoin.DefaultIfEmpty()
                where auditLog.ExecutionTime >= input.StartDate && auditLog.ExecutionTime <= input.EndDate
                select new AuditLogAndUser {AuditLog = auditLog, User = joinedUser};

            query = query
                .WhereIf(!input.UserName.IsNullOrWhiteSpace(), item => item.User.UserName.Contains(input.UserName))
                .WhereIf(!input.ServiceName.IsNullOrWhiteSpace(), item => item.AuditLog.ServiceName.Contains(input.ServiceName))
                .WhereIf(!input.MethodName.IsNullOrWhiteSpace(), item => item.AuditLog.MethodName.Contains(input.MethodName))
                .WhereIf(!input.BrowserInfo.IsNullOrWhiteSpace(), item => item.AuditLog.BrowserInfo.Contains(input.BrowserInfo))
                .WhereIf(input.MinExecutionDuration.HasValue && input.MinExecutionDuration > 0, item => item.AuditLog.ExecutionDuration >= input.MinExecutionDuration.Value)
                .WhereIf(input.MaxExecutionDuration.HasValue && input.MaxExecutionDuration < int.MaxValue, item => item.AuditLog.ExecutionDuration <= input.MaxExecutionDuration.Value)
                .WhereIf(input.HasException == true, item => item.AuditLog.Exception != null && item.AuditLog.Exception != "")
                .WhereIf(input.HasException == false, item => item.AuditLog.Exception == null || item.AuditLog.Exception == "");
            return query;
        }
    }
}
