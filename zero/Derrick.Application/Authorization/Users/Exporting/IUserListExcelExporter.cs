using System.Collections.Generic;
using Derrick.Authorization.Users.Dto;
using Derrick.Dto;

namespace Derrick.Authorization.Users.Exporting
{
    /// <summary>
    /// 用户列表Excel导出器
    /// </summary>
    public interface IUserListExcelExporter
    {
        /// <summary>
        /// 将用户列表导出到Excel
        /// </summary>
        /// <param name="userListDtos">用户dto列表</param>
        /// <returns></returns>
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}