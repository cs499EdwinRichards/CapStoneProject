using System;
using System.Collections.Generic;

namespace ZooAuthentication.Services
{
  /// <summary>
  /// Class containing a static reference to some service objects that can be referenced throughout the application
  /// </summary>
  public class ServiceContainer
  {
    /// <summary>
    /// Contains the services registered
    /// </summary>
    private Dictionary<Type, object> services = new Dictionary<Type, object>();

    /// <summary>
    /// Gets the reference to the static <see cref="ServiceContainer"/>
    /// </summary>
    public static readonly ServiceContainer Instance = new ServiceContainer();

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceContainer"/> class
    /// </summary>
    public ServiceContainer()
    {
    }

    /// <summary>
    /// Adds an instance of a service to the static container
    /// </summary>
    /// <typeparam name="TService">The type of the service to add</typeparam>
    /// <param name="implementation">An object reference</param>
    public void AddService<TService>(TService implementation)
      where TService : class
    {
      services[typeof(TService)] = implementation;
    }

    /// <summary>
    /// Checks to see if the service type is in the container
    /// </summary>
    /// <typeparam name="TService">The type of the service to search for</typeparam>
    public bool Contains<TService>()
      where TService : class
    {
      return services.ContainsKey(typeof(TService));
    }

    /// <summary>
    /// Gets the reference to the instance of the service from the static container
    /// </summary>
    /// <typeparam name="TService">The type of the service to return</typeparam>
    public TService GetService<TService>()
        where TService : class
    {
      services.TryGetValue(typeof(TService), out object service);

      return service as TService;
    }
  }
}
