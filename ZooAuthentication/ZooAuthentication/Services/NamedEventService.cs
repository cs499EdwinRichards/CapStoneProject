using System.Collections.Generic;
using System.Linq;
using ZooAuthentication.NamedEvents;

namespace ZooAuthentication.Services
{
  /// <summary>
  /// Service provides a centralized <see cref="NamedEvent"/> subscription and execution point
  /// </summary>
  public class NamedEventService
  {
    /// <summary>
    /// List of subscriptions contained within the <see cref="NamedEventService"/>
    /// </summary>
    private List<NamedEvent> subscriptions = new List<NamedEvent>();

    /// <summary>
    /// Locates or creates a <see cref="NamedEvent"/> and subscribes a consumer
    /// </summary>
    /// <param name="subscriber">Identifier of the subscriber class</param>
    /// <param name="name">The name of the <see cref="NamedEvent"/> to subscribe to</param>
    /// <param name="handler">The <see cref="NamedEventHandler"/> containing the link to the consuming method</param>
    public void Subscribe(string subscriber, string name, NamedEventHandler handler)
    {
      NamedEvent subscription = subscriptions.FirstOrDefault(s => s.Name == name);

      if (subscription != null)
      {
        if (!subscription.Subscribers.Contains(subscriber))
          subscription.Subscribers.Add(subscriber);

        subscription.Handler -= handler;
        subscription.Handler += handler;
      }
      else
      {
        subscription = new NamedEvent(name, handler);
        subscription.Subscribers.Add(subscriber);
        subscriptions.Add(subscription);
      }
    }

    /// <summary>
    /// Locates or creates a <see cref="NamedEvent"/> and subscribes a consumer
    /// </summary>
    /// <param name="subscriber">Identifier of the subscriber class</param>
    /// <param name="name">The name of the <see cref="NamedEvent"/> to subscribe to</param>
    /// <param name="handler">The <see cref="NamedEventReturnHandler"/> containing the link to the consuming method</param>
    public void SubscribeWithReturn(string subscriber, string name, NamedEventReturnHandler handler)
    {
      NamedEvent subscription = subscriptions.FirstOrDefault(s => s.Name == name);

      if (subscription != null)
      {
        if (!subscription.Subscribers.Contains(subscriber))
          subscription.Subscribers.Add(subscriber);

        subscription.ReturnHandler -= handler;
        subscription.ReturnHandler += handler;
      }
      else
      {
        subscription = new NamedEvent(name, handler);
        subscription.Subscribers.Add(subscriber);
        subscriptions.Add(subscription);
      }
    }

    /// <summary>
    /// Locates a <see cref="NamedEvent"/> and unsubscribes a consumer
    /// </summary>
    /// <param name="subscriber">Identifier of the subscriber class</param>
    /// <param name="name">The name of the <see cref="NamedEvent"/> to unsubscribes from</param>
    /// <param name="handler">The <see cref="NamedEventHandler"/> containing the link to the consuming method</param>
    public void Unsubscribe(string subscriber, string name, NamedEventHandler handler)
    {
      NamedEvent subscription = subscriptions.FirstOrDefault(s => s.Name == name);

      if (subscription != null)
      {
        subscription.Subscribers.Remove(subscriber);
        subscription.Handler -= handler;
      }
    }

    /// <summary>
    /// Determines if the subscriber class is subscribed to the <see cref="NamedEvent"/>
    /// </summary>
    /// <param name="subscriber">Identifier of the subscriber class</param>
    /// <param name="name">The name of the <see cref="NamedEvent"/></param>
    /// <returns>If the cubscriber is subscribed to the <see cref="NamedEvent"/>, true; otherwise, false</returns>
    public bool IsSubscribed(string subscriber, string name)
    {
      NamedEvent subscription = subscriptions.FirstOrDefault(s => s.Name == name);

      if (subscription != null)
        return subscription.Subscribers.Contains(subscriber);
      else
        return false;
    }

    /// <summary>
    /// Executes the <see cref="NamedEvent"/>
    /// </summary>
    /// <param name="sender">The object initiating the execution</param>
    /// <param name="name">The name of the <see cref="NamedEvent"/> to execute</param>
    /// <param name="parameters">Collection of <see cref="NamedEventParameter"/> items</param>
    public void Execute(object sender, string name, params NamedEventParameter[] parameters)
    {
      NamedEvent subscription = subscriptions.FirstOrDefault(s => s.Name == name);

      if (subscription != null)
        subscription?.Execute(sender, parameters);
    }

    /// <summary>
    /// Executes the <see cref="NamedEvent"/>
    /// </summary>
    /// <param name="sender">The object initiating the execution</param>
    /// <param name="name">The name of the <see cref="NamedEvent"/> to execute</param>
    /// <param name="parameters">Collection of <see cref="NamedEventParameter"/> items</param>
    public object ExecuteWithReturn(object sender, string name, params NamedEventParameter[] parameters)
    {
      NamedEvent subscription = subscriptions.FirstOrDefault(s => s.Name == name);

      if (subscription != null)
        return subscription?.ExecuteWithReturn(sender, parameters);

      return null;
    }

    /// <summary>
    /// Clears all subscriptions for all subscribers
    /// </summary>
    public void Clear()
    {
      foreach (var subscription in this.subscriptions)
        subscription.UnSubscribe();

      this.subscriptions.Clear();
      this.subscriptions = new List<NamedEvent>();
    }
  }
}
