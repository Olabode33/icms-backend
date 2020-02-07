using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ICMSDemo.EntityFrameworkCore
{
    public static class ICMSDemoDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ICMSDemoDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ICMSDemoDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}