using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ZooAuthentication.DataLayer
{
  public class Function
  {
    public Function()
    {
    }

    public string Name { get; set; } = string.Empty;

    public List<SqlParameter> Parameters { get; set; } = new List<SqlParameter>();

    public SqlParameter this[string name]
    {
      get
      {
        var param = Parameters.FirstOrDefault(p => p.ParameterName == name);
        if (param == null)
        {
          param = new SqlParameter(name, null);
          Parameters.Add(param);
        }

        return param;
      }
    }

    public Function Copy(Function original)
    {
      var result = new Function { Name = original.Name };
      foreach (var param in original.Parameters)
        result.Parameters.Add(new SqlParameter(param.ParameterName, param.Value));

      return result;
    }

    public string ToSqlSource()
    {
      var result = new StringBuilder($"EXECUTE {Name}{Environment.NewLine}");

      foreach (var param in Parameters)
      {
        if (param.Value == null || param.Value == DBNull.Value)
        {
          result.Append($"NULL,{Environment.NewLine}");
          continue;
        }

        switch (param.SqlDbType)
        {
          case SqlDbType.BigInt:
          case SqlDbType.Int:
          case SqlDbType.Bit:
          case SqlDbType.Decimal:
          case SqlDbType.Float:
          case SqlDbType.Real:
          case SqlDbType.TinyInt:
            result.Append($"{param.Value},{Environment.NewLine}");
            break;
          case SqlDbType.Binary:
          case SqlDbType.VarBinary:
            result.Append("0x");

            foreach (var b in (byte[])param.Value)
              result.Append($"{b:X2}");

            result.Append($",{Environment.NewLine}");
            break;
          default:
            result.Append($"'{param.Value}',{Environment.NewLine}");
            break;
        }
      }

      return result.ToString().TrimEnd(new[] { ',' }) + ";";
    }

    public int ExecuteNonQuery()
    {
      return new Database().ExecuteNonQuery(this);
    }

    public List<T> ExecuteData<T>()
      where T : class, new()
    {
      return new Database().ExecuteData<T>(this);
    }

    public virtual List<object> Map(DataTable records)
    {
      return null;
    }
  }
}
