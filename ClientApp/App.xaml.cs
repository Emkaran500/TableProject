using ClientApp.ViewModels;
using ClientApp.Views;
using SimpleInjector;
using System.Windows;

namespace ClientApp
{
    public partial class App : Application
    {
        //public static Container Container { get; set; } = new Container();
        protected override void OnStartup(StartupEventArgs e)
        {
            //this.RegisterContainer();

            //this.Start<AuthorizationViewModel>();

            //base.OnStartup(e);
        }

        //private void Start<T>() where T : ViewModelBase
        //{
        //    var mainView = new MainWindow();
        //    var mainViewModel = Container.GetInstance<MainViewModel>();
        //    mainViewModel.ActiveViewModel = Container.GetInstance<T>();

        //    mainView.DataContext = mainViewModel;

        //    mainView.ShowDialog();
        //}

        //private void RegisterContainer()
        //{
        //    Container.RegisterSingleton<MainViewModel>();
        //    Container.RegisterSingleton<OverviewViewModel>();

        //    Container.Verify();
        //}

    }
}
