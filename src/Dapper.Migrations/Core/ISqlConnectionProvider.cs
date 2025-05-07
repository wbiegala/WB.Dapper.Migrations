using Microsoft.Data.SqlClient;

namespace WB.Dapper.Migrations.Core
{
    internal interface ISqlConnectionProvider
    {
        SqlConnection GetConnection();
    }
}
