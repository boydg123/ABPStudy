using System;

namespace Abp.EntityFramework
{
    /// <summary>
    /// Used to mark a DbContext as default for a multi db context project.
    /// 用来标记多数据库上下文项目中的默认数据库上下文
    /// </summary>
    public class DefaultDbContextAttribute : Attribute
    {
        
    }
}