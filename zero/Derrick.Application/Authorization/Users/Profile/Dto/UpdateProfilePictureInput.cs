using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Derrick.Authorization.Users.Profile.Dto
{
    /// <summary>
    /// 更新用户资料照片Input
    /// </summary>
    public class UpdateProfilePictureInput
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string FileName { get; set; }
        /// <summary>
        /// X坐标
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y坐标
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
    }
}