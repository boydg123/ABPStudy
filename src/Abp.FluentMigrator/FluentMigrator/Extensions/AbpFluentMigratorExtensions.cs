using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FluentMigrator;
using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create.Table;

namespace Abp.FluentMigrator.Extensions
{
    /// <summary>
    /// This class is an extension for migration system to make easier to some common tasks.
    /// 此类事迁移系统的扩展，使通用任务变得更容易
    /// </summary>
    public static class AbpFluentMigratorExtensions
    {
        /// <summary>
        /// Adds an auto increment <see cref="int"/> primary key to the table.
        /// 将自动增量<see cref="int"/>主键添加到表中
        /// </summary>
        public static ICreateTableColumnOptionOrWithColumnSyntax WithIdAsInt32(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity();
        }

        /// <summary>
        /// Adds an auto increment <see cref="long"/> primary key to the table.
        /// 将自动增量<see cref="long"/>主键添加到表中
        /// </summary>
        public static ICreateTableColumnOptionOrWithColumnSyntax WithIdAsInt64(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity();
        }

        /// <summary>
        /// Adds IsDeleted column to the table. See <see cref="ISoftDelete"/>.
        /// 添加是否删除列到表中。查看<see cref="ISoftDelete"/>
        /// </summary>
        public static ICreateTableColumnOptionOrWithColumnSyntax WithIsDeletedColumn(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        /// <summary>
        /// Adds IsDeleted column to the table. See <see cref="ISoftDelete"/>.
        /// 添加是否删除列到表中。查看<see cref="ISoftDelete"/>
        /// </summary>
        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AddIsDeletedColumn(this IAlterTableAddColumnOrAlterColumnSyntax table)
        {
            return table
                .AddColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        /// <summary>
        /// Adds DeletionTime column to a table. See <see cref="IDeletionAudited"/>.
        /// 添加删除时间列到表中。查看<see cref="IDeletionAudited"/>.
        /// </summary>
        public static ICreateTableColumnOptionOrWithColumnSyntax WithDeletionTimeColumn(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithColumn("DeletionTime").AsDateTime().Nullable();
        }

        /// <summary>
        /// Adds DeletionTime column to a table. See <see cref="IDeletionAudited"/>.
        /// 添加删除时间列到表中。查看<see cref="IDeletionAudited"/>.
        /// </summary>
        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AddDeletionTimeColumn(this IAlterTableAddColumnOrAlterColumnSyntax table)
        {
            return table
                .AddColumn("DeletionTime").AsDateTime().Nullable();
        }

        /// <summary>
        /// Ads CreationTime field to the table for <see cref="IHasCreationTime"/> interface.
        /// 为接口 <see cref="IHasCreationTime"/>添加创建时间字段到表中
        /// </summary>
        public static ICreateTableColumnOptionOrWithColumnSyntax WithCreationTimeColumn(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithColumn("CreationTime").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
        }

        /// <summary>
        /// Adds CreationTime field to a table. See <see cref="IHasCreationTime"/>.
        /// 添加创建时间到表中。查看<see cref="IHasCreationTime"/>.
        /// </summary>
        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AddCreationTimeColumn(this IAlterTableAddColumnOrAlterColumnSyntax table)
        {
            return table
                .AddColumn("CreationTime").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
        }

        /// <summary>
        /// Adds LastModificationTime field to a table. See <see cref="IModificationAudited"/>.
        /// 添加最后修改时间字段到表中。查看<see cref="IModificationAudited"/>.
        /// </summary>
        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AddLastModificationTimeColumn(this IAlterTableAddColumnOrAlterColumnSyntax table)
        {
            return table
                .AddColumn("LastModificationTime").AsDateTime().Nullable();
        }

        /// <summary>
        /// Adds LastModificationTime field to a table. See <see cref="IModificationAudited"/>.
        /// 添加最后修改时间字段到表中。查看<see cref="IModificationAudited"/>.
        /// </summary>
        public static ICreateTableColumnOptionOrWithColumnSyntax WithLastModificationTimeColumn(this ICreateTableWithColumnSyntax table, bool defaultValue = true)
        {
            return table
                .WithColumn("LastModificationTime").AsDateTime().Nullable();
        }

        /// <summary>
        /// Adds IsDeleted column to the table. See <see cref="IPassivable"/>.
        /// 添加是否删除列到表中。查看<see cref="IPassivable"/>.
        /// </summary>
        public static ICreateTableColumnOptionOrWithColumnSyntax WithIsActiveColumn(this ICreateTableWithColumnSyntax table, bool defaultValue = true)
        {
            return table
                .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(defaultValue);
        }

        /// <summary>
        /// Adds IsDeleted column to the table. See <see cref="IPassivable"/>.
        /// 添加是否删除列到表中。查看<see cref="IPassivable"/>.
        /// </summary>
        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AddIsActiveColumn(this IAlterTableAddColumnOrAlterColumnSyntax table, bool defaultValue = true)
        {
            return table
                .AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(defaultValue);
        }
    }
}
