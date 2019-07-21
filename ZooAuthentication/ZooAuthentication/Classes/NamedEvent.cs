using System.Collections.Generic;

namespace ZooAuthentication.NamedEvents
{
  public class NamedEvent
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="NamedEvent"/> class
    /// </summary>
    /// <param name="name">The name of the event</param>
    public NamedEvent(string name)
    {
      this.Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NamedEvent"/> class
    /// </summary>
    /// <param name="name">The name of the event</param>
    /// <param name="handler">A delegate containing a link to the consuming event method</param>
    public NamedEvent(string name, NamedEventHandler handler)
    {
      this.Name = name;
      this.Handler += handler;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NamedEvent"/> class
    /// </summary>
    /// <param name="name">The name of the event</param>
    /// <param name="handler">A delegate containing a link to the consuming event method</param>
    public NamedEvent(string name, NamedEventReturnHandler handler)
    {
      this.Name = name;
      this.ReturnHandler += handler;
    }

    /// <summary>
    /// Gets or sets a list of subscriber subscribed to this <see cref="NamedEvent"/>
    /// </summary>
    public List<string> Subscribers
    {
      get;
      set;
    } = new List<string>();

    /// <summary>
    /// Gets or sets the name of this <see cref="NamedEvent"/>
    /// </summary>
    public string Name
    {
      get;
      set;
    }

    /// <summary>
    /// The event triggered when this <see cref="NamedEvent"/> is executed
    /// </summary>
    public event NamedEventHandler Handler;

    /// <summary>
    /// The event triggered when this <see cref="NamedEvent"/> is executed
    /// </summary>
    public event NamedEventReturnHandler ReturnHandler;

    /// <summary>
    /// Executes this <see cref="NamedEvent"/>
    /// </summary>
    /// <param name="sender">The object initiating the execution</param>
    /// <param name="parameters">Collection of <see cref="NamedEventParameter"/> items</param>
    public void Execute(object sender, params NamedEventParameter[] parameters)
    {
      this.Handler?.Invoke(sender, parameters);
    }

    /// <summary>
    /// Executes this <see cref="NamedEvent"/>
    /// </summary>
    /// <param name="sender">The object initiating the execution</param>
    /// <param name="parameters">Collection of <see cref="NamedEventParameter"/> items</param>
    /// <returns>A result from the event execution</returns>
    public object ExecuteWithReturn(object sender, params NamedEventParameter[] parameters)
    {
      return this.ReturnHandler?.Invoke(sender, parameters);
    }

    /// <summary>
    /// Clears this <see cref="NamedEvent"/> and its subscribers
    /// </summary>
    public void UnSubscribe()
    {
      this.Subscribers.Clear();

      if (this.Handler != null)
        this.Handler = null;
    }
  }
}
