using System.Collections.Generic;
using Abp.Dependency;
using Abp.RealTime;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// 用户注销通知者
    /// </summary>
    public interface IUserLogoutInformer
    {
        /// <summary>
        /// 通知客户端
        /// </summary>
        /// <param name="clients">客户端列表</param>
        void InformClients(IReadOnlyList<IOnlineClient> clients);
    }
}
