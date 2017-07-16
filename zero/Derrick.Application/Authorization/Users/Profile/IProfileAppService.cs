using System.Threading.Tasks;
using Abp.Application.Services;
using Derrick.Authorization.Users.Profile.Dto;

namespace Derrick.Authorization.Users.Profile
{
    /// <summary>
    /// 用户资料APP服务
    /// </summary>
    public interface IProfileAppService : IApplicationService
    {
        /// <summary>
        /// 为编辑时获取当前用户资料
        /// </summary>
        /// <returns></returns>
        Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit();
        /// <summary>
        /// 更新当前用户资料
        /// </summary>
        /// <param name="input">当前用户资料编辑Dto</param>
        /// <returns></returns>
        Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input">修改密码Input</param>
        /// <returns></returns>
        Task ChangePassword(ChangePasswordInput input);
        /// <summary>
        /// 更新用户资料图片
        /// </summary>
        /// <param name="input">更新用户资料图片Input</param>
        /// <returns></returns>
        Task UpdateProfilePicture(UpdateProfilePictureInput input);
        /// <summary>
        /// 获取密码复杂度设置
        /// </summary>
        /// <returns></returns>
        Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting();
    }
}
