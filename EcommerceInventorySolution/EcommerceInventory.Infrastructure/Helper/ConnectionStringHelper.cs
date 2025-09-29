using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Infrastructure.Helper;
public static class ConnectionStringHelper
{
    public static string GetPostgreConnectionString(IConfiguration configuration)
    {
        string connectionStringTemp = configuration.GetConnectionString("PostgreConnection")!;
        return connectionStringTemp
            .Replace("$POSTGRE_HOST", Environment.GetEnvironmentVariable("POSTGRE_HOST") ?? "localhost")
            .Replace("$POSTGRE_PORT", Environment.GetEnvironmentVariable("POSTGRE_PORT") ?? "5432")
            .Replace("$POSTGRE_DB", Environment.GetEnvironmentVariable("POSTGRE_DB") ?? "EcommerceInventory")
            .Replace("$POSTGRE_USER", Environment.GetEnvironmentVariable("POSTGRE_USER") ?? "postgres")
            .Replace("$POSTGRE_PASSWORD", Environment.GetEnvironmentVariable("POSTGRE_PASSWORD") ?? "godika");
    }
}
