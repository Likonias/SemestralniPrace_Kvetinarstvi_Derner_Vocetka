using System.Data;
using System.Windows;
using Oracle.ManagedDataAccess.Client;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        OracleConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            connection = GetConnection();
            connection.Open();
            updateDataGrid();
        }
        public static OracleConnection GetConnection()
        {
            string connectionString = "User Id=st67018;Password=abcde;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521))(CONNECT_DATA=(SID=BDAS)(SERVER=DEDICATED)))";
            return new OracleConnection(connectionString);
        }

        private string read(OracleDataReader reader, int columnIndex)
        {
            return reader.IsDBNull(columnIndex) ? "..." : reader.GetString(columnIndex);
        }

        private void updateDataGrid()
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from zakaznici";
            cmd.CommandType = System.Data.CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            myDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();

        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

            Register register = new Register();
            register.ShowDialog();

        }
    }
}
