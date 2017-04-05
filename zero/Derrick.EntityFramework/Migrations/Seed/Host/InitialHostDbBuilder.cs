using Derrick.EntityFramework;

namespace Derrick.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly AbpZeroTemplateDbContext _context;

        public InitialHostDbBuilder(AbpZeroTemplateDbContext context)
        {
            _context = context;
        }

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
