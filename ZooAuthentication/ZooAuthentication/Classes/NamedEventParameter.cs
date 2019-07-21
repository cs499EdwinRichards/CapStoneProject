using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooAuthentication.NamedEvents
{
  /// <summary>
  /// Defines a named event parameter
  /// </summary>
  public class NamedEventParameter
  {
    /// <summary>
    /// The name of the value contained within this <see cref="NamedEventParameter"/>
    /// </summary>
    public string Name
    {
      get;
      private set;
    }

    /// <summary>
    /// The value of this <see cref="NamedEventParameter"/>
    /// </summary>
    public object Value
    {
      get;
      private set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NamedEventParameter"/> class
    /// </summary>
    /// <param name="name">The name of the parameter</param>
    public NamedEventParameter(string name)
      : this(name, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NamedEventParameter"/> class
    /// </summary>
    /// <param name="name">The name of the parameter</param>
    /// <param name="value">The object contained in the parameter</param>
    public NamedEventParameter(string name, object value)
    {
      this.Name = name;
      this.Value = value;
    }
  }
}
