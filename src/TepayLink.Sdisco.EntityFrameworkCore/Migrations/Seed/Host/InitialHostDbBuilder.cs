using TepayLink.Sdisco.EntityFrameworkCore;

namespace TepayLink.Sdisco.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly SdiscoDbContext _context;

        public InitialHostDbBuilder(SdiscoDbContext context)
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
