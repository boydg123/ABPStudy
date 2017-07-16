using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Derrick.Authorization.Users.Dto;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// <see cref="IUserLoginAppService"/>实现，用户登录APP服务
    /// </summary>
    [AbpAuthorize]
    public class UserLoginAppService : AbpZeroTemplateAppServiceBase, IUserLoginAppService
    {
        /// <summary>
        /// 用户尝试登录仓储
        /// </summary>
        private readonly IRepository<UserLoginAttempt, long> _userLoginAttemptRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userLoginAttemptRepository">用户尝试登录仓储</param>
        public UserLoginAppService(IRepository<UserLoginAttempt, long> userLoginAttemptRepository)
        {
            _userLoginAttemptRepository = userLoginAttemptRepository;
        }
        /// <summary>
        /// 获取最新的用户登录尝试
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts()
        {
            var userId = AbpSession.GetUserId();

            var loginAttempts = await _userLoginAttemptRepository.GetAll()
                .Where(la => la.UserId == userId)
                .OrderByDescending(la => la.CreationTime)
                .Take(10)
                .ToListAsync();

            return new ListResultDto<UserLoginAttemptDto>(loginAttempts.MapTo<List<UserLoginAttemptDto>>());
        }
    }
}