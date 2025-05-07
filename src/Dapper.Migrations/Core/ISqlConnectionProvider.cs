using Microsoft.Data.SqlClient;

namespace WB.Dapper.Migrations.Core
{
    public interface ISqlConnectionProvider
    {
        SqlConnection GetConnection();
    }
}
