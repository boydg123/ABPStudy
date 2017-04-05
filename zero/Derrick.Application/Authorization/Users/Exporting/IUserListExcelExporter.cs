using System.Collections.Generic;
using Derrick.Authorization.Users.Dto;
using Derrick.Dto;

namespace Derrick.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}