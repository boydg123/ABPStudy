using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Derrick.Dto
{
    /// <summary>
    /// 文件Dto
    /// </summary>
    public class FileDto
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [Required]
        public string FileName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [Required]
        public string FileType { get; set; }

        /// <summary>
        /// 文件Token
        /// </summary>
        [Required]
        public string FileToken { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FileDto()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="fileType">文件类型</param>
        public FileDto(string fileName, string fileType)
        {
            FileName = fileName;
            FileType = fileType;
            FileToken = Guid.NewGuid().ToString("N");
        }
    }
}