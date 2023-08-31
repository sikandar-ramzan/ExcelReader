using System;
using System.Data;
using System.Data.SqlClient;
namespace ExcelReaderAPI.Services
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetDBConnString()
        {
            var dbConnString = _connectionString;
            return dbConnString;
        }

        /* public DataTable ExecuteStoredProcedure(string storedProcedureName, SqlParameter[] parameters = null)
         {
             using (var connection = new SqlConnection(_connectionString))
             {
                 using (var command = new SqlCommand(storedProcedureName, connection))
                 {
                     command.CommandType = CommandType.StoredProcedure;

                     if (parameters != null)
                     {
                         command.Parameters.AddRange(parameters);
                     }

                     connection.Open();

                     var dataTable = new DataTable();
                     using (var reader = command.ExecuteReader())
                     {
                         dataTable.Load(reader);
                     }

                     return dataTable;
                 }
             }
         }*/

    }
}
