using System.Collections.Generic;
using System.Data;
using ZooAuthentication.DataLayer;
using ZooAuthentication.Models;

namespace ZooAuthentication.Functions
{
  class authenticate_user : Function
  {
    public authenticate_user()
    {
      Name = "authenticate_user";
    }

    public override List<object> Map(DataTable records)
    {
      var result = new List<object>();

      foreach (DataRow record in records.Rows)
      {
        var model = new AuthenticateResult
        {
          Success = record.IsNull("login_success") ? default(bool) : (bool)record["login_success"],
          RoleGreeting = record.IsNull("role_greeting") ? default(string) : (string)record["role_greeting"],
          RoleTasks = record.IsNull("role_tasks") ? default(string) : (string)record["role_tasks"]
        };

        result.Add(model);
      }

      return result;
    }
  }
}
