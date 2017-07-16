using System.Collections.Generic;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Derrick.Authorization.Users.Dto;
using Derrick.DataExporting.Excel.EpPlus;
using Derrick.Dto;

namespace Derrick.Authorization.Users.Exporting
{
    /// <summary>
    /// <see cref="IUserListExcelExporter"/>实现，用户列表Excel导出器
    /// </summary>
    public class UserListExcelExporter : EpPlusExcelExporterBase, IUserListExcelExporter
    {
        /// <summary>
        /// 时区转换器
        /// </summary>
        private readonly ITimeZoneConverter _timeZoneConverter;
        /// <summary>
        /// Abp Session引用
        /// </summary>
        private readonly IAbpSession _abpSession;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="timeZoneConverter">时区转换器</param>
        /// <param name="abpSession">Abp Session引用</param>
        public UserListExcelExporter(
            ITimeZoneConverter timeZoneConverter, 
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }
        /// <summary>
        /// 将用户列表导出到Excel
        /// </summary>
        /// <param name="userListDtos">用户dto列表</param>
        /// <returns></returns>
        public FileDto ExportToFile(List<UserListDto> userListDtos)
        {
            return CreateExcelPackage(
                "UserList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Users"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Surname"),
                        L("UserName"),
                        L("PhoneNumber"),
                        L("EmailAddress"),
                        L("EmailConfirm"),
                        L("Roles"),
                        L("LastLoginTime"),
                        L("Active"),
                        L("CreationTime")
                        );

                    AddObjects(
                        sheet, 2, userListDtos,
                        _ => _.Name,
                        _ => _.Surname,
                        _ => _.UserName,
                        _ => _.PhoneNumber,
                        _ => _.EmailAddress,
                        _ => _.IsEmailConfirmed,
                        _ => _.Roles.Select(r => r.RoleName).JoinAsString(", "),
                        _ => _timeZoneConverter.Convert(_.LastLoginTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.IsActive,
                        _ => _timeZoneConverter.Convert(_.CreationTime, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    //Formatting cells

                    var lastLoginTimeColumn = sheet.Column(8);
                    lastLoginTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";

                    var creationTimeColumn = sheet.Column(10);
                    creationTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";

                    for (var i = 1; i <= 10; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
