namespace ZooAuthentication.Models
{
  /// <summary>
  /// Represents a credential from the file of valid credentials for authentication
  /// </summary>
  internal class Credential
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="Credential"/> class
    /// </summary>
    /// <param name="user">The user name for this credential</param>
    /// <param name="pass">The MD5 hash of the correct password</param>
    /// <param name="role">The role assigned to this user</param>
    public Credential(string user = null, string pass = null, string role = null)
    {
      User = user;
      Pass = pass;
      Role = role;
    }

    /// <summary>
    /// Gets the user name for this <see cref="Credential"/>
    /// </summary>
    public string User { get; private set; }

    /// <summary>
    /// Gets the MD5 hash of the password for this <see cref="Credential"/>
    /// </summary>
    public string Pass { get; private set; }

    /// <summary>
    /// Gets the assigned to this <see cref="Credential"/>
    /// </summary>
    public string Role { get; private set; }
  }
}
