using Derrick.EntityFramework;

namespace Derrick.Migrations.Seed.Host
{
    /// <summary>
    /// 初始宿主
    /// </summary>
    public class InitialHostDbBuilder
    {
        /// <summary>
        /// DB上下文
        /// </summary>
        private readonly AbpZeroTemplateDbContext _context;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">DB上下文</param>
        public InitialHostDbBuilder(AbpZeroTemplateDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 创建
        /// </summary>
        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
