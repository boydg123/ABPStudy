using System.ComponentModel.DataAnnotations;
using Abp.Application.Editions;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Represents a Tenant of the application.
    /// 代表一个应用程序的商户
    /// </summary>
    public abstract class AbpTenant<TUser> : AbpTenantBase, IFullAudited<TUser>, IPassivable
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// "Default".
        /// 默认商户名称
        /// </summary>
        public const string DefaultTenantName = "Default";

        /// <summary>
        /// "^[a-zA-Z][a-zA-Z0-9_-]{1,}$".
        /// 商户名称正则
        /// </summary>
        public const string TenancyNameRegex = "^[a-zA-Z][a-zA-Z0-9_-]{1,}$";

        /// <summary>
        /// <see cref="Name"/>属性的最大长度
        /// </summary>
        public const int MaxNameLength = 128;

        /// <summary>
        /// 商户的当前版本
        /// </summary>
        public virtual Edition Edition { get; set; }
        /// <summary>
        /// 版本ID
        /// </summary>
        public virtual int? EditionId { get; set; }

        /// <summary>
        /// 商户的显示名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Is this tenant active?If as tenant is not active, no user of this tenant can use the application.
        /// 商户是否激活，如果商户未激活，则没有用户能使用当前应用程序
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Reference to the creator user of this entity.
        /// 创建人
        /// </summary>
        public virtual TUser CreatorUser { get; set; }

        /// <summary>
        /// Reference to the last modifier user of this entity.
        /// 最后修改人
        /// </summary>
        public virtual TUser LastModifierUser { get; set; }

        /// <summary>
        /// Reference to the deleter user of this entity.
        /// 删除者
        /// </summary>
        public virtual TUser DeleterUser { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected AbpTenant()
        {
            IsActive = true;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenancyName">商户的唯一名称</param>
        /// <param name="name">商户的显示名</param>
        protected AbpTenant(string tenancyName, string name)
            : this()
        {
            TenancyName = tenancyName;
            Name = name;
        }
    }
}
