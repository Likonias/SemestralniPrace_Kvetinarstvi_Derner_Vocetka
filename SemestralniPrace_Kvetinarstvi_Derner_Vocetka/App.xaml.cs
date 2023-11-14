using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Views;
using System.Windows;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            Navigation navigation = new Navigation();

            navigation.CurrentViewModel = new MainViewModel(navigation);

            MainWindow = new MainWindow()
            {
                DataContext = navigation.CurrentViewModel
            };


            base.OnStartup(e);
        }
    }
}
