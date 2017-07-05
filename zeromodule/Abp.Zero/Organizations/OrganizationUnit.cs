using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;

namespace Abp.Organizations
{
    /// <summary>
    /// Represents an organization unit (OU).
    /// 标识一个组织架构(OU)
    /// </summary>
    [Table("AbpOrganizationUnits")]
    public class OrganizationUnit : FullAuditedEntity<long>, IMayHaveTenant
    {
        /// <summary>
        /// <see cref="DisplayName"/>属性的最大长度
        /// </summary>
        public const int MaxDisplayNameLength = 128;

        /// <summary>
        /// OU的最大层级
        /// </summary>
        public const int MaxDepth = 16;

        /// <summary>
        /// OU编码字段亮点之前的长度
        /// </summary>
        public const int CodeUnitLength = 5;

        /// <summary>
        /// <see cref="Code"/>属性的最大长度
        /// </summary>
        public const int MaxCodeLength = MaxDepth * (CodeUnitLength + 1) - 1;

        /// <summary>
        /// 商户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 父<see cref="OrganizationUnit"/>,如果此OU是根则为Null
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual OrganizationUnit Parent { get; set; }

        /// <summary>
        /// Parent <see cref="OrganizationUnit"/> Id.Null, if this OU is root.
        /// 父<see cref="OrganizationUnit"/>的ID，如果此OU是根则为Null
        /// </summary>
        public virtual long? ParentId { get; set; }

        /// <summary>
        /// Hierarchical Code of this organization unit.Example: "00001.00042.00005".
        /// 当前组织的分层代码。例如：00001.00042.00005
        /// This is a unique code for a Tenant.It's changeable if OU hierarch is changed.
        /// 在同一商户是唯一的，如果OU改变则它也是变化的
        /// </summary>
        [Required]
        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        /// <summary>
        /// Display name of this role.
        /// 组织的显示名称
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Children of this OU.
        /// 子组织列表
        /// </summary>
        public virtual ICollection<OrganizationUnit> Children { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public OrganizationUnit()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">商户ID或Null(如果是宿主商户)</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="parentId">父ID或Null(如果ou是根)</param>
        public OrganizationUnit(int? tenantId, string displayName, long? parentId = null)
        {
            TenantId = tenantId;
            DisplayName = displayName;
            ParentId = parentId;
        }

        /// <summary>
        /// 通过给定的数字创建code。例如：数字是 4,2，则返回"00004.00002"
        /// </summary>
        /// <param name="numbers">Numbers</param>
        public static string CreateCode(params int[] numbers)
        {
            if (numbers.IsNullOrEmpty())
            {
                return null;
            }

            return numbers.Select(number => number.ToString(new string('0', CodeUnitLength))).JoinAsString(".");
        }

        /// <summary>
        /// 给父code追加一个子code，例如：父code = "00001"，子code = "00042" 则返回"00001.00042"
        /// </summary>
        /// <param name="parentCode">Parent code. Can be null or empty if parent is a root.</param>
        /// <param name="childCode">Child code.</param>
        public static string AppendCode(string parentCode, string childCode)
        {
            if (childCode.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return childCode;
            }

            return parentCode + "." + childCode;
        }

        /// <summary>
        /// Gets relative code to the parent.Example: if code = "00019.00055.00001" and parentCode = "00019" then returns "00055.00001".
        /// 获取父组织的相关OU Code，例如：Code为："00019.00055.00001" 并且 父Code为="00019" 则返回"00055.00001"
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="parentCode">父Code</param>
        public static string GetRelativeCode(string code, string parentCode)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return code;
            }

            if (code.Length == parentCode.Length)
            {
                return null;
            }

            return code.Substring(parentCode.Length + 1);
        }

        /// <summary>
        /// Calculates next code for given code.Example: if code = "00019.00055.00001" returns "00019.00055.00002".
        /// 根据给定code计算下一个code。例如：code="00019.00055.00001" 返回"00019.00055.00002"
        /// </summary>
        /// <param name="code">The code. / code</param>
        public static string CalculateNextCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var parentCode = GetParentCode(code);
            var lastUnitCode = GetLastUnitCode(code);

            return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
        }

        /// <summary>
        /// Gets the last unit code.Example: if code = "00019.00055.00001" returns "00001".
        /// 获取最后的组织code，例如：code为"00019.00055.00001" 返回"00001"
        /// </summary>
        /// <param name="code">The code.</param>
        public static string GetLastUnitCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            return splittedCode[splittedCode.Length - 1];
        }

        /// <summary>
        /// Gets parent code.Example: if code = "00019.00055.00001" returns "00019.00055".
        /// 获取父Code，例如：code为"00019.00055.00001" 则返回"00019.00055"
        /// </summary>
        /// <param name="code">The code.</param>
        public static string GetParentCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            if (splittedCode.Length == 1)
            {
                return null;
            }

            return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
        }
    }
}
