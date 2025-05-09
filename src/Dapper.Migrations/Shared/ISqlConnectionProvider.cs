using Microsoft.Data.SqlClient;

namespace WB.Dapper.Migrations.Shared
{
    public interface ISqlConnectionProvider
    {
        SqlConnection GetConnection();
    }
}
