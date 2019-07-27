using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ZooAuthentication.Classes;
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
    /// The list of valid credentials from which we can authenticate
    /// </summary>
    private List<Credential> credentials = new List<Credential>();

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginViewModel"/> class
    /// </summary>
    public LoginViewModel()
    {
      // Wire up the command to authenticate the user with the appropriate handler method
      AuthenticateCommand = new RelayCommand(Authenticate);
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
    public SecureString Password { private get; set; }

    /// <summary>
    /// Gets the number of login attempts processed
    /// </summary>
    public int Attempts { get; private set; } = 0;

    /// <summary>
    /// Gets or sets the <see cref="RelayCommand"/> to invoke the authenticate event
    /// </summary>
    public ICommand AuthenticateCommand { get; set; }

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

      if (credentials.Count == 0 && !InitializeCredentials())
      {
        ErrorMessage = "No credentials located. Login not possible.";
        return;
      }

      Attempts++;

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

        // Return the credential object with a matching username and password if it exists
        var match = credentials.FirstOrDefault(c => c.User == Username &&
          c.Pass.ToLower() == BitConverter.ToString(hash).Replace("-", "").ToLower());

        if (match == null)
        {
          // Login failed
          if (Attempts >= 3)
          {
            MessageBox.Show("You have exceeded the maximum number of allowed attempts.\n" +
              "The program will now close.\n\nPlease try again later.",
              "Authentication Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
          }
          else
            ErrorMessage = "Invalid username or password";
          return;
        }
        else
        {
          // Execute the named event to indicate successful login. Consumed by the Main Window
          // which will change the screen to display the authorized tasks for the given role
          ServiceContainer.Instance.GetService<NamedEventService>()
            .Execute(this, "LoginSuccess",
            new NamedEventParameter("user", match.User),
            new NamedEventParameter("role", match.Role));
        }
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

    /// <summary>
    /// Initializes the list of credentials from the credentials text file
    /// </summary>
    /// <returns><see langword="true"/> if the credentials were initialized successfully, 
    /// otherwise <see langword="false"/></returns>
    private bool InitializeCredentials()
    {
      try
      {
        // Get all the lines from the file
        var lines = File.ReadAllLines(Path.Combine("..", "..", "Files", "credentials.txt"));

        foreach (var line in lines)
        {
          // Split up the line into individual fields
          var fields = line.Split('\t');
          // Add a new credential object using the fields for the username, hashed password, and role
          credentials.Add(new Credential(fields[0], fields[1], fields.Last()));
        }

        return credentials.Count > 0;
      }
      catch (FileNotFoundException)
      {
        ErrorMessage = "Unable to locate the credentials file";
      }
      catch (Exception ex)
      {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"Unhandled Exception: {ex.Message}");
#endif
        ErrorMessage = "An unknown error occurred";
      }

      return false;
    }
  }
}
