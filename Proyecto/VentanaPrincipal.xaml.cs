using System.Windows;
using System.Windows.Input;

namespace Proyecto
{
    public partial class VentanaPrincipal
    {

        public VentanaPrincipal()
        {
            InitializeComponent();
            Main.Content = new Information();
            if (SqlQueries.UserLevel() == "Empleado") // supervisor
            {
                UserBttn.IsEnabled = false;
                ReportBttn.IsEnabled = false;
            }

            if (SqlQueries.UserLevel() == "Contable") // contable
            {
                UserBttn.IsEnabled = false;
                CategoriesBttn.IsEnabled = false;
                ProductsBttn.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeScreen (object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ShowReports(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.RemoveBackEntry();
            Main.Content = new Reports();
        }

        private void Show_Categories(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.RemoveBackEntry();
            Main.Content = new Categories();
        }

        private void ShowProductos(object sender, RoutedEventArgs e)
        {
                Main.NavigationService.RemoveBackEntry();
                Main.Content = new Productos();
        }

        private void Show_Usuarios(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.RemoveBackEntry();
            Main.Content = new Usuarios();
        }

        private void Log_Out(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
            
        }
    }
}
