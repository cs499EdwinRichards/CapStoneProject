using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooAuthentication.Models
{
  public class AuthenticateResult
  {
    public AuthenticateResult()
    {
    }

    public bool Success { get; set; }

    public string RoleGreeting { get; set; }

    public string RoleTasks { get; set; }
  }
}
