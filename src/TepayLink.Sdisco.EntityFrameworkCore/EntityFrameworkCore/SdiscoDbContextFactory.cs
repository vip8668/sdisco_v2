using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TepayLink.Sdisco.Configuration;
using TepayLink.Sdisco.Web;

namespace TepayLink.Sdisco.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class SdiscoDbContextFactory : IDesignTimeDbContextFactory<SdiscoDbContext>
    {
        public SdiscoDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SdiscoDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            SdiscoDbContextConfigurer.Configure(builder, configuration.GetConnectionString(SdiscoConsts.ConnectionStringName));

            return new SdiscoDbContext(builder.Options);
        }
    }
}