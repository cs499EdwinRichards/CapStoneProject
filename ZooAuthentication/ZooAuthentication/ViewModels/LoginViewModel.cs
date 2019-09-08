using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ZooAuthentication.Classes;
using ZooAuthentication.DataLayer;
using ZooAuthentication.Functions;
using ZooAuthentication.Models;
using ZooAuthentication.NamedEvents;
using ZooAuthentication.Services;

namespace ZooAuthentication.ViewModels
{
  /// <summary>
  /// The viewmodel that controls the UI for the login screen
  /// </summary>
  class LoginViewModel : ViewModelBase
  {
    /// <summary>
    /// The username entered by the user
    /// </summary>
    private string username = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginViewModel"/> class
    /// </summary>
    public LoginViewModel()
    {
      // Wire up the command to authenticate the user with the appropriate handler method
      AuthenticateCommand = new RelayCommand(Authenticate);
      UnlockCommand = new RelayCommand(Unlock, param => Username.Length > 0);
    }

    /// <summary>
    /// Gets or sets the username entered by the user for authentication
    /// </summary>
    public string Username
    {
      get => username;
      set
      {
        username = value;
        NotifyPropertyChanged();
      }
    }

    /// <summary>
    /// Sets the password entered by the user
    /// </summary>
    public SecureString Password { private get; set; } = new SecureString();

    /// <summary>
    /// Gets or sets the <see cref="RelayCommand"/> to invoke the authenticate event
    /// </summary>
    public ICommand AuthenticateCommand { get; set; }

    public ICommand UnlockCommand { get; set; }

    /// <summary>
    /// Executes an attempt to authenticate the user
    /// </summary>
    private void Authenticate(object parameter)
    {
      // Clear any previous error message
      ErrorMessage = string.Empty;

      // Ensure the user has entered the required information and display an error if needed
      if (string.IsNullOrWhiteSpace(Username))
      {
        ErrorMessage = "Please enter a username before logging in";
        return;
      }

      if (Password.Length == 0)
      {
        ErrorMessage = "Please enter a password before logging in";
        return;
      }

      // The byte array that will contain the result of hashing the entered password
      byte[] hash;

      /* This next section is used to retrieve the password from the password box.
       * Due to security reasons, it is not safe to store passwords as standard
       * strings data types. The password box returns a SecureString instead so it
       * needs to be converted to a byte array for use with the hashing algorithm.
       * The following code was modified from the example I located online at
       * https://codereview.stackexchange.com/questions/107860/converting-a-securestring-to-a-byte-array
       */
      IntPtr unmanagedString = IntPtr.Zero;
      try
      {
        // Get the bytes from the secure string
        unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(Password);

        // Apply the hashing algorithm
        hash = SHA256.Create().ComputeHash(
          Encoding.UTF8.GetBytes(Marshal.PtrToStringUni(unmanagedString)));

        Function loginFunction = new authenticate_user();

        var results = new List<AuthenticateResult>();

        try
        {
          loginFunction["@user"].Value = Username;
          loginFunction["@password"].Value = hash;

          results = loginFunction.ExecuteData<AuthenticateResult>();

          if (results.Count == 0)
            throw new Exception("Nothing returned during authentication");
        }
        catch (Exception ex)
        {
#if DEBUG
          System.Diagnostics.Debug.WriteLine($"Database Exception: {ex.Message}");
#endif
          ErrorMessage = "An error occurred in the database during authentication";
          return;
        }

        var loginResult = results.First();

        if (!loginResult.Success)
        {
          ErrorMessage = "Authentication failed!";
          return;
        }

        // Execute the named event to indicate successful login. Consumed by the Main Window
        // which will change the screen to display the authorized tasks for the given role
        ServiceContainer.Instance.GetService<NamedEventService>()
          .Execute(this, "LoginSuccess",
          new NamedEventParameter("user", Username),
          new NamedEventParameter("greeting", loginResult.RoleGreeting),
          new NamedEventParameter("tasks", loginResult.RoleTasks));
      }
      // Handle any errors
      catch (Exception ex)
      {
        // Print the the output window if debugging
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"Unhandled Exception: {ex.Message}");
#endif
        ErrorMessage = "An unknown error occurred";
      }
      finally
      {
        // Make sure to free the pointer used to conver the secure string to a byte array
        if (unmanagedString != IntPtr.Zero)
        {
          Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
        }
      }
    }

    private void Unlock(object parameter)
    {
      ErrorMessage = string.Empty;

      var result = -1;

      try
      {
        Function unlockFunction = new unlock_user();

        unlockFunction["@user"].Value = Username;

        result = unlockFunction.ExecuteNonQuery();

        if (result > 0)
          MessageBox.Show($"Successfully unlocked user '{Username}'. Try logging in again.", "Unlock User");
        else
          MessageBox.Show($"Unable to unlocked user '{Username}'. Try again or log in with a different user account.", "Unlock User", 
            MessageBoxButton.OK, MessageBoxImage.Exclamation);
      }
      catch (Exception ex)
      {
#if DEBUG
        Debug.WriteLine($"Error unlocking user '{Username}': {ex.Message}");
#endif
        ErrorMessage = "There was an error unlocking the user. Try again later or log in with a different user account.";
      }
    }
  }
}
