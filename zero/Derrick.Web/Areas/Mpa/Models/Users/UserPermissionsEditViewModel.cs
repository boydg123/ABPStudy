using Abp.AutoMapper;
using Derrick.Authorization.Users;
using Derrick.Authorization.Users.Dto;
using Derrick.Web.Areas.Mpa.Models.Common;

namespace Derrick.Web.Areas.Mpa.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; private set; }

        public UserPermissionsEditViewModel(GetUserPermissionsForEditOutput output, User user)
        {
            User = user;
            output.MapTo(this);
        }
    }
}