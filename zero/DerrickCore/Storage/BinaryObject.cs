using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Derrick.Storage
{
    /// <summary>
    /// 二进制对象
    /// </summary>
    [Table("AppBinaryObjects")]
    public class BinaryObject : Entity<Guid>, IMayHaveTenant
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 字节
        /// </summary>
        [Required]
        public virtual byte[] Bytes { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BinaryObject()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="bytes">字节</param>
        public BinaryObject(int? tenantId, byte[] bytes)
            : this()
        {
            TenantId = tenantId;
            Bytes = bytes;
        }
    }
}
