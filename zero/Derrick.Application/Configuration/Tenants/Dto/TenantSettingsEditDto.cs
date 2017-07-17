using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Derrick.Configuration.Host.Dto;

namespace Derrick.Configuration.Tenants.Dto
{
    /// <summary>
    /// 商户设置编辑Dto
    /// </summary>
    public class TenantSettingsEditDto
    {
        /// <summary>
        /// 常规设置编辑Dto
        /// </summary>
        public GeneralSettingsEditDto General { get; set; }

        /// <summary>
        /// 商户用户管理设置编辑Dto
        /// </summary>
        [Required]
        public TenantUserManagementSettingsEditDto UserManagement { get; set; }

        /// <summary>
        /// 邮件设置编辑Dto
        /// </summary>
        public EmailSettingsEditDto Email { get; set; }

        /// <summary>
        /// Ldap设置编辑Dto
        /// </summary>
        public LdapSettingsEditDto Ldap { get; set; }

        /// <summary>
        /// 安全社会自编辑Dto
        /// </summary>
        [Required]
        public SecuritySettingsEditDto Security { get; set; }

        /// <summary>
        /// This validation is done for single-tenant applications.
        /// 这个验证是为单个商户应用程序做的。
        /// Because, these settings can only be set by tenant in a single-tenant application.
        /// 因为，这些设置只能被但商户应用程序设置。
        /// </summary>
        public void ValidateHostSettings()
        {
            var validationErrors = new List<ValidationResult>();
            if (General == null)
            {
                validationErrors.Add(new ValidationResult("General settings can not be null", new[] { "General" }));
            }
            else
            {
                if (General.WebSiteRootAddress.IsNullOrEmpty())
                {
                    validationErrors.Add(new ValidationResult("General.WebSiteRootAddress can not be null or empty", new[] { "WebSiteRootAddress" }));
                }
            }

            if (Email == null)
            {
                validationErrors.Add(new ValidationResult("Email settings can not be null", new[] { "Email" }));
            }

            if (validationErrors.Count > 0)
            {
                throw new AbpValidationException("Method arguments are not valid! See ValidationErrors for details.", validationErrors);
            }
        }
    }
}