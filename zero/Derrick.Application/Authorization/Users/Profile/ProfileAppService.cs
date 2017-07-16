using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Extensions;
using Abp.IO;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Derrick.Authorization.Users.Profile.Dto;
using Derrick.Configuration;
using Derrick.Security;
using Derrick.Storage;
using Derrick.Timing;
using Newtonsoft.Json;

namespace Derrick.Authorization.Users.Profile
{
    /// <summary>
    /// <see cref="IProfileAppService"/>实现，用户资料APP服务
    /// </summary>
    [AbpAuthorize]
    public class ProfileAppService : AbpZeroTemplateAppServiceBase, IProfileAppService
    {
        /// <summary>
        /// APP文件夹
        /// </summary>
        private readonly IAppFolders _appFolders;
        /// <summary>
        /// 二进制对象管理
        /// </summary>
        private readonly IBinaryObjectManager _binaryObjectManager;
        /// <summary>
        /// 时区服务
        /// </summary>
        private readonly ITimeZoneService _timeZoneService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appFolders">APP文件夹</param>
        /// <param name="binaryObjectManager">二进制对象管理</param>
        /// <param name="timezoneService">时区服务</param>
        public ProfileAppService(
            IAppFolders appFolders,
            IBinaryObjectManager binaryObjectManager,
            ITimeZoneService timezoneService)
        {
            _appFolders = appFolders;
            _binaryObjectManager = binaryObjectManager;
            _timeZoneService = timezoneService;
        }
        /// <summary>
        /// 为编辑时获取当前用户资料
        /// </summary>
        /// <returns></returns>
        public async Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit()
        {
            var user = await GetCurrentUserAsync();
            var userProfileEditDto = user.MapTo<CurrentUserProfileEditDto>();

            if (Clock.SupportsMultipleTimezone)
            {
                userProfileEditDto.Timezone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);

                var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                if (userProfileEditDto.Timezone == defaultTimeZoneId)
                {
                    userProfileEditDto.Timezone = string.Empty;
                }
            }

            return userProfileEditDto;
        }
        /// <summary>
        /// 更新当前用户资料
        /// </summary>
        /// <param name="input">当前用户资料编辑Dto</param>
        /// <returns></returns>
        public async Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input)
        {
            var user = await GetCurrentUserAsync();
            input.MapTo(user);
            CheckErrors(await UserManager.UpdateAsync(user));

            if (Clock.SupportsMultipleTimezone)
            {
                if (input.Timezone.IsNullOrEmpty())
                {
                    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, input.Timezone);
                }
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input">修改密码Input</param>
        /// <returns></returns>
        public async Task ChangePassword(ChangePasswordInput input)
        {
            await CheckPasswordComplexity(input.NewPassword);

            var user = await GetCurrentUserAsync();
            CheckErrors(await UserManager.ChangePasswordAsync(user.Id, input.CurrentPassword, input.NewPassword));
        }
        /// <summary>
        /// 更新用户资料图片
        /// </summary>
        /// <param name="input">更新用户资料图片Input</param>
        /// <returns></returns>
        public async Task UpdateProfilePicture(UpdateProfilePictureInput input)
        {
            var tempProfilePicturePath = Path.Combine(_appFolders.TempFileDownloadFolder, input.FileName);

            byte[] byteArray;

            using (var fsTempProfilePicture = new FileStream(tempProfilePicturePath, FileMode.Open))
            {
                using (var bmpImage = new Bitmap(fsTempProfilePicture))
                {
                    var width = input.Width == 0 ? bmpImage.Width : input.Width;
                    var height = input.Height == 0 ? bmpImage.Height : input.Height;
                    var bmCrop = bmpImage.Clone(new Rectangle(input.X, input.Y, width, height), bmpImage.PixelFormat);

                    using (var stream = new MemoryStream())
                    {
                        bmCrop.Save(stream, bmpImage.RawFormat);
                        stream.Close();
                        byteArray = stream.ToArray();
                    }
                }
            }

            if (byteArray.LongLength > 102400) //100 KB
            {
                throw new UserFriendlyException(L("ResizedProfilePicture_Warn_SizeLimit"));
            }

            var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());

            if (user.ProfilePictureId.HasValue)
            {
                await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
            await _binaryObjectManager.SaveAsync(storedFile);

            user.ProfilePictureId = storedFile.Id;

            FileHelper.DeleteIfExists(tempProfilePicturePath);
        }
        /// <summary>
        /// 获取密码复杂度设置
        /// </summary>
        /// <returns></returns>
        public async Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting()
        {
            var settingValue = await SettingManager.GetSettingValueAsync(AppSettings.Security.PasswordComplexity);
            var setting = JsonConvert.DeserializeObject<PasswordComplexitySetting>(settingValue);

            return new GetPasswordComplexitySettingOutput
            {
                Setting = setting
            };
        }
        /// <summary>
        /// 修改密码复杂度
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns></returns>
        private async Task CheckPasswordComplexity(string password)
        {
            var passwordComplexitySettingValue = await SettingManager.GetSettingValueAsync(AppSettings.Security.PasswordComplexity);
            var passwordComplexitySetting = JsonConvert.DeserializeObject<PasswordComplexitySetting>(passwordComplexitySettingValue);
            var passwordComplexityChecker = new PasswordComplexityChecker();
            var passwordValid = passwordComplexityChecker.Check(passwordComplexitySetting, password);
            if (!passwordValid)
            {
                throw new UserFriendlyException(L("PasswordComplexityNotSatisfied"));
            }
        }
    }
}