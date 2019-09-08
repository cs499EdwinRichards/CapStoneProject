using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ZooAuthentication.DataLayer
{
  public class Database
  {
    public Database()
    {
    }

    public static string ConnectionString { get; set; } = string.Empty;

    public int ExecuteNonQuery(Function function)
    {
      int result = -1;

      if (string.IsNullOrWhiteSpace(ConnectionString))
        throw new InvalidOperationException("The database connection string has not been specified");

      if (string.IsNullOrWhiteSpace(function.Name))
        throw new ArgumentException("The stored procedure name has not been specified");

      using (var connection = new SqlConnection(ConnectionString))
      {
        using (var command = new SqlCommand(function.Name, connection))
        {
          command.CommandType = CommandType.StoredProcedure;

          foreach (var param in function.Parameters)
            command.Parameters.Add(param);

          connection.Open();

          result = command.ExecuteNonQuery();
        }
      }

      return result;
    }

    public List<T> ExecuteData<T>(Function function)
      where T : class, new()
    {
      var result = new List<T>();

      if (string.IsNullOrWhiteSpace(ConnectionString))
        throw new InvalidOperationException("The database connection string has not been specified");

      if (string.IsNullOrWhiteSpace(function.Name))
        throw new ArgumentException("The stored procedure name has not been specified");

      using (var data = new DataTable(function.Name))
      {
        using (var connection = new SqlConnection(ConnectionString))
        {
          using (var command = new SqlCommand(function.Name, connection))
          {
            command.CommandType = CommandType.StoredProcedure;

            foreach (var param in function.Parameters)
              command.Parameters.Add(param);

            using (var adapter = new SqlDataAdapter(command))
            {
              adapter.Fill(data);
            }

            var mapped = function.Map(data);

            if (mapped != null)
              return mapped.ConvertAll(r => r as T);
          }
        }
      }

      return result;
    }
  }
}
