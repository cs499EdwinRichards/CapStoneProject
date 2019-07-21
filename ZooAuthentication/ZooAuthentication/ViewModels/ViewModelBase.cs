using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ZooAuthentication.ViewModels
{
  /// <summary>
  /// The base class to use for any view models in the program
  /// </summary>
  public class ViewModelBase : INotifyPropertyChanged
  {

    /// <summary>
    /// Represents an error message to display when the viewmodel encounters an error
    /// </summary>
    private string errorMessage = string.Empty;

    /// <summary>
    /// Occurs when a property value changes
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Gets or sets the value of the error message to display
    /// </summary>
    public string ErrorMessage
    {
      get => errorMessage;
      set
      {
        errorMessage = value;
        NotifyPropertyChanged();
        NotifyPropertyChanged(nameof(ErrorVisibility));
      }
    }

    /// <summary>
    /// Gets the visibility of the error message
    /// </summary>
    public Visibility ErrorVisibility
    {
      get => string.IsNullOrWhiteSpace(ErrorMessage) ? 
        Visibility.Collapsed : Visibility.Visible;
    }

    /// <summary>
    /// Notifies the Framwork that a property has changed and the UI needs to be updated
    /// </summary>
    /// <param name="property">The name of the property that was updated</param>
    protected void NotifyPropertyChanged([CallerMemberName] string property = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
  }
}
