using System;
using System.Windows.Input;

namespace ZooAuthentication.Classes
{
  /// <summary>
  /// Class that facilitates relaying commands from UI events to handler methods
  /// </summary>
  /// <seealso cref="ICommand" />
  public class RelayCommand : ICommand
  {
    private Action<object> execute;
    private Func<object, bool> canExecute;

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class
    /// </summary>
    /// <param name="execute">The method to execute</param>
    /// <param name="canExecute">An action to determine if this <see cref="RelayCommand"/> can be executed</param>
    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
      this.execute = execute;
      this.canExecute = canExecute;
    }

    /// <summary>
    /// Defines the method that determines whether the command can execute in its current state
    /// </summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" /></param>
    /// <returns>
    ///   <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />
    /// </returns>
    public bool CanExecute(object parameter)
    {
      return this.canExecute == null || this.canExecute(parameter);
    }

    /// <summary>
    /// Defines the method to be called when the command is invoked
    /// </summary>
    /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" /></param>
    public void Execute(object parameter)
    {
      this.execute(parameter);
    }
  }
}
