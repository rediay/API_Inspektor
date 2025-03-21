using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<DataTable> ExecuteStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters)
    {
        var dataTable = new DataTable();

        try
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (var sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddRange(parameters);
                    sqlCommand.CommandTimeout = 3600;

                    using (var dataReader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                    {
                        dataTable.Load(dataReader);
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Manejo de excepción adecuado: puedes registrar el error o lanzar una excepción personalizada
            throw new Exception("Error al ejecutar el procedimiento almacenado", ex);
        }

        return dataTable;
    }
}