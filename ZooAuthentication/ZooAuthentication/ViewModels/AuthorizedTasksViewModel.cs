using System.IO;
using System.Linq;
using System.Windows.Input;
using ZooAuthentication.Classes;
using ZooAuthentication.Services;

namespace ZooAuthentication.ViewModels
{
  /// <summary>
  /// The view model that will control the UI to display what the user is authorized to do based on their role
  /// </summary>
  public class AuthorizedTasksViewModel : ViewModelBase
  {
    /// <summary>
    /// The username of who is logged in
    /// </summary>
    private string user = string.Empty;

    /// <summary>
    /// The greeting from the authorized file
    /// </summary>
    private string greeting = string.Empty;

    /// <summary>
    /// The details of what the user is authorized to do/see
    /// </summary>
    private string details = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizedTasksViewModel"/> class
    /// </summary>
    /// <param name="user">The username</param>
    /// <param name="role">The role to which the user is assigned</param>
    public AuthorizedTasksViewModel(string user, string role)
    {
      User = user;

      // Wireup the logout command for the UI to the appropriate handler method
      LogoutCommand = new RelayCommand(Logout);

      // Read in the greeting and details from the correct role file
      LoadValuesFromFile(role);
    }

    /// <summary>
    /// Gets or sets the user name
    /// </summary>
    public string User
    {
      get => user;
      set
      {
        user = value;
        NotifyPropertyChanged();
      }
    }

    /// <summary>
    /// Gets or sets the greeting for the role type
    /// </summary>
    public string Greeting
    {
      get => greeting;
      set
      {
        greeting = value;
        NotifyPropertyChanged();
      }
    }

    /// <summary>
    /// Gets or sets the details for the role type
    /// </summary>
    public string Details
    {
      get => details;
      set
      {
        details = value;
        NotifyPropertyChanged();
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="RelayCommand"/> that will be used to invoke the logout method
    /// </summary>
    public ICommand LogoutCommand { get; set; }

    /// <summary>
    /// Method invoked from the UI to logout the user
    /// </summary>
    private void Logout(object parameter)
    {
      // Execute the named event to logout the user. Consumed by the Main View 
      // which resets the screen to allow a new user to login
      ServiceContainer.Instance.GetService<NamedEventService>()
        .Execute(this, "Logout");
    }

    /// <summary>
    /// Reads in the authorization values from the correct role file to display to the user
    /// </summary>
    /// <param name="role">The name of the role</param>
    private void LoadValuesFromFile(string role)
    {
      // Set the name of the file to open based on the name of the role
      string filename = $"{role}.txt";

      // Read the lines from the file
      var lines = File.ReadAllLines(Path.Combine("..", "..", "Files", filename));

      // First line of the file is the greeting
      Greeting = lines.First();
      // Last line of the file contains the details of what is authorized for the user
      Details = lines.Last();
    }
  }
}
