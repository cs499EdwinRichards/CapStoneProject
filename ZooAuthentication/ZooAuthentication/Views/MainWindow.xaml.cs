using System.Windows;
using ZooAuthentication.DataLayer;
using ZooAuthentication.Services;
using ZooAuthentication.ViewModels;

namespace ZooAuthentication.Views
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      Database.ConnectionString = @"Server=DSI02W10\SQLDEV2017;Database=ZooAuthentication;Trusted_Connection=True;";

      DataContext = new LoginViewModel();

      ServiceContainer.Instance.AddService(new NamedEventService());

      var eventService = ServiceContainer.Instance.GetService<NamedEventService>();

      eventService.Subscribe("MainWindowViewModel", "LoginSuccess",
        (o, p) =>
        {
          DataContext = new AuthorizedTasksViewModel((string)p[0].Value, (string)p[1].Value, (string)p[2].Value);
        });

      eventService.Subscribe("MainWindowViewModel", "Logout",
        (o, p) =>
        {
          DataContext = new LoginViewModel();
        });
    }
  }
}
