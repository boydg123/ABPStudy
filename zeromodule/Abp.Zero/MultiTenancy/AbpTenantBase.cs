using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Runtime.Security;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Base class for tenants.
    /// 商户的基类
    /// </summary>
    [Table("AbpTenants")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public abstract class AbpTenantBase : FullAuditedEntity<int>
    {
        /// <summary>
        /// <see cref="TenancyName"/>属性的最大长度
        /// </summary>
        public const int MaxTenancyNameLength = 64;

        /// <summary>
        /// <see cref="ConnectionString"/>属性的最大长度
        /// </summary>
        public const int MaxConnectionStringLength = 1024;

        /// <summary>
        /// Tenancy name. This property is the UNIQUE name of this Tenant.It can be used as subdomain name in a web application.
        /// 商户名。此属性在商户中是唯一的，它可以在web应用程序中用作子域名
        /// </summary>
        [Required]
        [StringLength(MaxTenancyNameLength)]
        public virtual string TenancyName { get; set; }

        /// <summary>
        /// ENCRYPTED connection string of the tenant database.Can be null if this tenant is stored in host database.Use <see cref="SimpleStringCipher"/> to encrypt/decrypt this.
        /// 商户的加密连接字符串。如果商户存储在宿主数据库中则可以为null，使用<see cref="SimpleStringCipher"/>加密/解密
        /// </summary>
        [StringLength(MaxConnectionStringLength)]
        public virtual string ConnectionString { get; set; }
    }
}