using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ICMSDemo.Configuration;
using ICMSDemo.Web;

namespace ICMSDemo.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class ICMSDemoDbContextFactory : IDesignTimeDbContextFactory<ICMSDemoDbContext>
    {
        public ICMSDemoDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ICMSDemoDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            ICMSDemoDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ICMSDemoConsts.ConnectionStringName));

            return new ICMSDemoDbContext(builder.Options);
        }
    }
}