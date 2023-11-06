using System.Windows;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            LoginViewModel viewModel = new LoginViewModel();

            DataContext = viewModel;
        }
    }
}
