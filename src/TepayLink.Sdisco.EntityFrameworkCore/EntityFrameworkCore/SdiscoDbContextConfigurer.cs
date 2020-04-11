using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.EntityFrameworkCore
{
    public static class SdiscoDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<SdiscoDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<SdiscoDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}