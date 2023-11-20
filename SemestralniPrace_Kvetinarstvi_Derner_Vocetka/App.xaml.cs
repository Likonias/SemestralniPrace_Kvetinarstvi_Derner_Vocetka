using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Views;
using System;
using System.Windows;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore navigationStore;
        private readonly NavigationBarViewModel navigationBarViewModel;
        public App()
        {
            navigationStore = new NavigationStore();
            navigationBarViewModel = new NavigationBarViewModel(CreateLoginNavigationService(), CreateRegisterNavigationService());
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            //TODO this should be redone to a home view or something like that
            //NavigationService<HomeVIewModel> homeNavigationService = CreateHomeNavigationService();
            //homeNavigationService.Navigate();
            navigationStore.CurrentViewModel = new MainViewModel(navigationBarViewModel, navigationStore);

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private NavigationService<LoginViewModel> CreateLoginNavigationService()
        {
            return new NavigationService<LoginViewModel>(navigationStore, () => new LoginViewModel(navigationStore));
        }


        private NavigationService<RegisterViewModel> CreateRegisterNavigationService()
        {
            return new NavigationService<RegisterViewModel>(navigationStore, () => new RegisterViewModel(navigationStore));
        }
    }
}
