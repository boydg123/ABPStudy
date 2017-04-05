using System.Collections.Generic;
using Derrick.Authorization.Users.Dto;

namespace Derrick.Web.Areas.Mpa.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}