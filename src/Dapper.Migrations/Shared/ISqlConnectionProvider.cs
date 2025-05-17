using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace WB.Dapper.Migrations.Shared
{
    public interface ISqlConnectionProvider
    {
        DbConnection GetConnection();
    }
}
